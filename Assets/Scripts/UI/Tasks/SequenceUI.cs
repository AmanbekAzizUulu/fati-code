using System.Collections.Generic;
using UnityEngine;

public class SequenceUI : MonoBehaviour
{
    [SerializeField] private SequenceButton prefab;
    [SerializeField] private FlexibleGridLayout answersContainer;
    [SerializeField] private FlexibleGridLayout optionsContainer;

    private SequenceTask task;
    public void Init(SequenceTask task)
    {
        this.task = task;
        for (int i = 0; i < task.Entry.options.Count; i++)
        {
            var option = Instantiate(prefab, optionsContainer.transform);
            option.onSelect.AddListener(OnSelectOption);
            option.Init(i, task.Entry.options[i]);
        }

        Invoke(nameof(UpdateUI), 0.1f);
    }

    private void UpdateUI()
    {
        optionsContainer.UpdateLayout();
    }

    private void OnSelectOption(SequenceButton button)
    {
        if (button.IsSelected)
        {
            button.transform.SetParent(answersContainer.transform);
            answersContainer.UpdateLayout();
        }
        else
        {
            button.transform.SetParent(optionsContainer.transform);
            optionsContainer.UpdateLayout();
        }

        var answer = GetAnswer();
        task.SetAnswer(answer);
        TaskManager.Instance.SetInteractableCheckButton(answer.Count > 0);
    }

    private List<int> GetAnswer()
    {
        var answer = new List<int>();
        for (int i = 0; i < answersContainer.transform.childCount; i++)
        {
            Transform child = answersContainer.transform.GetChild(i);

            if (child.TryGetComponent(out SequenceButton button))
            {
                answer.Add(button.ID);
            }
        }
        return answer;
    }
}
