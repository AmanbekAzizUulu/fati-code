using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private LevelEntry currentLevel;
    private TaskDatabase currentTask;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void StartGame(LevelEntry level, TaskDatabase task)
    {
        currentLevel = level;
        currentTask = task;
        LoadingManager.Instance.StartLoading(3);
        SceneManager.Instance.LoadScene("GameScene");
        Invoke(nameof(ShowTask), 3);
    }

    private void ShowTask()
    {
        TaskManager.Instance.StartTask(currentLevel, currentTask);
    }
}
