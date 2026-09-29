using UnityEngine;

public class StagesPanel : MonoBehaviour
{
    [SerializeField] private StageItem prefab;
    [SerializeField] private Transform container;
    [SerializeField] private TaskDatabase[] tasks;

    private void Start()
    {
        bool hasData = PlayerManager.Instance.HasLevelEntries();

        for (int i = 0; i < tasks.Length; i++)
        {
            LevelEntry entry = null;
            if (hasData)
            {
                entry = PlayerManager.Instance.GetLevelEntry(i);
            }
            else
            {
                entry = new LevelEntry(i, i == 0);
                PlayerManager.Instance.AddNewLevel(entry);
            }
            var stage = Instantiate(prefab, container);
            stage.Init(tasks[i], entry);
        }
    }
}
