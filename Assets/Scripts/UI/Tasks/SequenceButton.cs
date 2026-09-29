using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SequenceButton : MonoBehaviour
{
    [SerializeField] private Button buttonComponent;
    [SerializeField] private RectTransform buttonRect;
    [SerializeField] private GameObject selectedFrame;
    [SerializeField] private GameObject notSelectedFrame;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private float padding = 40f;

    public UnityEvent<SequenceButton> onSelect = new UnityEvent<SequenceButton>();

    public int ID {  get; private set; }
    public bool IsSelected { get; private set; }
    public void Init(int id, string option)
    {
        ID = id;
        IsSelected = false;
        textComponent.text = option;
        SetActiveSelectedFrame(IsSelected);
        buttonComponent.onClick.AddListener(OnClick);
        UpdateSize();
    }
    private void UpdateSize()
    {
        float textWidth = textComponent.preferredWidth;
        buttonRect.sizeDelta = new Vector2(textWidth + padding, buttonRect.sizeDelta.y);
    }
    private void OnClick()
    {
        IsSelected = !IsSelected;
        SetActiveSelectedFrame(IsSelected);

        onSelect?.Invoke(this);
    }
    private void SetActiveSelectedFrame(bool value)
    {
        selectedFrame.SetActive(value);
        notSelectedFrame.SetActive(!value);
    }
}
