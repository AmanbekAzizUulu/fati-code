using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class TaskJsonImporterWindow : EditorWindow
{
    private string jsonInput;
    private TaskDatabase targetDatabase;

    [MenuItem("Tools/Task JSON Importer")]
    public static void ShowWindow()
    {
        GetWindow<TaskJsonImporterWindow>("Task Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("JSON Input", EditorStyles.boldLabel);

        jsonInput = EditorGUILayout.TextArea(jsonInput, GUILayout.Height(250));

        GUILayout.Space(10);

        targetDatabase = (TaskDatabase)EditorGUILayout.ObjectField(
            "Target Database",
            targetDatabase,
            typeof(TaskDatabase),
            false
        );

        GUILayout.Space(10);

        if (GUILayout.Button("Parse & Apply"))
        {
            ParseAndApply();
        }
    }

    private void ParseAndApply()
    {
        if (string.IsNullOrWhiteSpace(jsonInput))
        {
            Debug.LogError("JSON пустой");
            return;
        }

        if (targetDatabase == null)
        {
            Debug.LogError("Не выбран TaskDatabase");
            return;
        }

        string wrappedJson = "{ \"items\": " + jsonInput + "}";

        RawTaskWrapper wrapper;

        try
        {
            wrapper = JsonUtility.FromJson<RawTaskWrapper>(wrappedJson);
        }
        catch
        {
            Debug.LogError("Ошибка парсинга JSON");
            return;
        }

        if (wrapper == null || wrapper.items == null)
        {
            Debug.LogError("JSON не содержит items");
            return;
        }

        Undo.RecordObject(targetDatabase, "Import Tasks");

        targetDatabase.tasks.Clear();

        int index = 0;

        foreach (var raw in wrapper.items)
        {
            if (raw == null)
            {
                Debug.LogWarning($"Элемент #{index} пустой — пропущен");
                index++;
                continue;
            }

            if (raw.correctOrder != null && raw.correctOrder.Count > 0)
            {
                var task = new SequenceTaskEntry
                {
                    question = raw.question,
                    options = raw.options ?? new List<string>(),
                    correctOrder = raw.correctOrder
                };

                targetDatabase.tasks.Add(task);
            }
            else
            {
                var task = new SingleChoiceTaskEntry
                {
                    question = raw.question,
                    options = raw.options ?? new List<string>(),
                    correctIndex = raw.correctIndex
                };

                targetDatabase.tasks.Add(task);
            }

            index++;
        }

        EditorUtility.SetDirty(targetDatabase);
        AssetDatabase.SaveAssets();

        Debug.Log($"Импорт завершен. Всего задач: {targetDatabase.tasks.Count}");
    }

    [Serializable]
    public class RawTaskWrapper
    {
        public List<RawTaskEntry> items;
    }
    [Serializable]
    public class RawTaskEntry
    {
        public string question;
        public List<string> options;

        public int correctIndex;
        public List<int> correctOrder;
    }
}