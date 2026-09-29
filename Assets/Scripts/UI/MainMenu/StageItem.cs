using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageItem : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject opened;
    [SerializeField] private GameObject closed;
    [Header("Components")]
    [SerializeField] private Button buttonComponent;
    [SerializeField] private Image imageComponent;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private Image[] stages;
    [Header("Properties")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color targetColor;
    [SerializeField] private Color stageDisabledColor;
    [SerializeField] private Color stageEnabledColor;

    private TaskDatabase task;
    private LevelEntry entry;

    public void Init(TaskDatabase task, LevelEntry entry)
    {
        this.task = task;
        this.entry = entry;
        buttonComponent.onClick.AddListener(OnClick);
        UpdateUI();
    }
    private void UpdateUI()
    {
        opened.SetActive(entry.isAvailable);
        closed.SetActive(!entry.isAvailable);

        if (entry.isAvailable)
        {
            textComponent.text = $"{entry.level + 1}";
            imageComponent.color = PlayerManager.Instance.GetCurrentLevel() == entry.level ? targetColor : defaultColor;
        }

        UpdateStages();
    }
    private void UpdateStages()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].color = entry.stages > i ? stageEnabledColor : stageDisabledColor;
        }
    }
    private void OnClick()
    {
        if (!entry.isAvailable)
            return;

        LevelManager.Instance.StartGame(entry, task);
    }
}
