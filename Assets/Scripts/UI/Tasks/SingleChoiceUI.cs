using System.Collections.Generic;
using UnityEngine;

public class SingleChoiceUI : MonoBehaviour
{
    [SerializeField] private SingleChoiceButton prefab;
    [SerializeField] private Transform container;

    private List<SingleChoiceButton> options = new List<SingleChoiceButton>();
    private SingleChoiceTask task;
    public void Init(SingleChoiceTask task)
    {
        this.task = task;
        for (int i = 0; i < task.Entry.options.Count; i++)
        {
            var option = Instantiate(prefab, container);
            option.onSelect.AddListener(OnSelectOption);
            option.Init(i, task.Entry.options[i]);
            options.Add(option);
        }
    }

    private void OnSelectOption(int id)
    {
        for (int i = 0; i < options.Count; i++)
        {
            if (id == i)
                continue;

            options[i].SetActiveSelectedFrame(false);
        }

        task.SetAnswer(id);
        TaskManager.Instance.SetInteractableCheckButton(id != -1);
    }
}
