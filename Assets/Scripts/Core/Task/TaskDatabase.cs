using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tasks/Task Database")]
public class TaskDatabase : ScriptableObject
{
    [SerializeReference]
    public List<TaskEntry> tasks = new();
    [SerializeField]
    public bool isRandom = false;
}