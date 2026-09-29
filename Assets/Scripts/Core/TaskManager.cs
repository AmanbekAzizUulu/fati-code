using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button checkButton;
    [SerializeField] private Button continueButton;
    [Header("Result")]
    [SerializeField] private RectTransform resultPanel;
    [SerializeField] private GameObject correctFrame;
    [SerializeField] private GameObject notCorrectFrame;
    [SerializeField] private TextMeshProUGUI resultTextComponent;
    [Header("Result properties")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private string[] correctResultText;
    [SerializeField] private string[] notCorrectResultText;

    [Header("Task")]
    [SerializeField] private Transform taskContainer;
    [SerializeField] private SingleChoiceUI singleChoicePrefab;
    [SerializeField] private SequenceUI sequencPrefab;

    private LevelEntry level;
    private TaskDatabase database;
    private List<TaskEntry> tasks;
    private int currentIndex;

    private ITask currentTask;
    private GameObject currentTaskUI;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        checkButton.onClick.AddListener(OnCheckButton);
        continueButton.onClick.AddListener(OnContinueButton);
    }
    public void StartTask(LevelEntry level, TaskDatabase database)
    {
        this.level = level;
        this.database = database;

        tasks = new List<TaskEntry>(database.tasks); 
        if (database.isRandom)
        {
            Shuffle(tasks);
        }

        currentIndex = 0;
        ShowTask(tasks[currentIndex]);
        SetActiveCheckButton(true);
    }
    private void Shuffle(List<TaskEntry> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            var temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    private void OnCheckButton()
    {
        bool isCorrect = currentTask.Validate();
        ShowResult(isCorrect);
        SetActiveCheckButton(!isCorrect);
    }
    private void OnContinueButton()
    {
        currentIndex++;
        if (currentIndex >= tasks.Count)
        {
            PlayerManager.Instance.SetLevelStage(level, GetStages());
            PlayerManager.Instance.SetLevelAvaibility(level.level + 1);
            LoadingManager.Instance.StartLoading();
            SceneManager.Instance.LoadScene("MainMenuScene");
            return;
        }

        ShowTask(tasks[currentIndex]);
        HideResult();
    }
    private int GetStages()
    {
        // TODO: Calculate level stages
        return 3;
    }
    public void SetInteractableCheckButton(bool value)
    {
        checkButton.interactable = value;
    }
    private void SetActiveCheckButton(bool value)
    {
        checkButton.gameObject.SetActive(value);
        continueButton.gameObject.SetActive(!value);
    }
    private void ShowTask(TaskEntry entry)
    {
        if (currentTaskUI != null)
            Destroy(currentTaskUI);

        SetActiveCheckButton(true);
        questionText.text = entry.question;
        if (entry is SingleChoiceTaskEntry single)
        {
            CreateSingleChoice(new SingleChoiceTask(single));
        }
        else if(entry is SequenceTaskEntry sequence)
        {
            CreateSequence(new SequenceTask(sequence));
        }
    }

    private void CreateSingleChoice(SingleChoiceTask task)
    {
        currentTask = task;
        var taskUI = Instantiate(singleChoicePrefab, taskContainer);
        taskUI.Init(task);
        currentTaskUI = taskUI.gameObject;
    }
    private void CreateSequence(SequenceTask task)
    {
        currentTask = task;
        var taskUI = Instantiate(sequencPrefab, taskContainer);
        taskUI.Init(task);
        currentTaskUI = taskUI.gameObject;
    }

    private void ShowResult(bool isCorrect)
    {
        resultPanel.gameObject.SetActive(true);

        correctFrame.SetActive(isCorrect);
        notCorrectFrame.SetActive(!isCorrect);

        resultTextComponent.text = GetResultRandomText(isCorrect);

        StopAllCoroutines();
        StartCoroutine(AnimatePanel());
    }
    private void HideResult()
    {
        resultPanel.gameObject.SetActive(false);
    }
    private IEnumerator AnimatePanel()
    {
        Vector2 startPos = new Vector2(resultPanel.anchoredPosition.x, -350);
        resultPanel.anchoredPosition = startPos;
        Vector2 targetPos = new Vector2(startPos.x, 0f);

        float time = 0f;

        while (time < animationDuration)
        {
            time += Time.deltaTime;
            float t = time / animationDuration;
            float easedT = 1f - Mathf.Pow(1f - t, 3f);

            resultPanel.anchoredPosition = Vector2.Lerp(startPos, targetPos, easedT);

            yield return null;
        }

        resultPanel.anchoredPosition = targetPos;
    }
    private string GetResultRandomText(bool isCorrect)
    {
        return GetRandomText(isCorrect ? correctResultText : notCorrectResultText);
    }
    private string GetRandomText(string[] array)
    {
        if (array == null || array.Length == 0)
            return "";

        return array[Random.Range(0, array.Length)];
    }
}
