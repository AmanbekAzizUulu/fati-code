using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SingleChoiceButton : MonoBehaviour
{
    [SerializeField] private Button buttonComponent;
    [SerializeField] private GameObject selectedFrame;
    [SerializeField] private GameObject notSelectedFrame;
    [SerializeField] private TextMeshProUGUI textComponent;

    public UnityEvent<int> onSelect = new UnityEvent<int>();

    private int id;
    private bool isSelected;
    public void Init(int id, string option)
    {
        this.id = id;
        isSelected = false;
        textComponent.text = option;
        SetActiveSelectedFrame(isSelected);
        buttonComponent.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        isSelected = !isSelected;
        SetActiveSelectedFrame(isSelected);

        onSelect?.Invoke(isSelected ? id : -1);
    }
    public void SetActiveSelectedFrame(bool value)
    {
        selectedFrame.SetActive(value);
        notSelectedFrame.SetActive(!value);
        isSelected = value;
    }
}
