using System.Collections.Generic;
using System;

[Serializable]
public abstract class TaskEntry
{
    public string question;
    public List<string> options;
}

[Serializable]
public class SingleChoiceTaskEntry : TaskEntry
{
    public int correctIndex;

    public override string ToString() => $"[Single] {question}";
}
[Serializable]
public class SequenceTaskEntry : TaskEntry
{
    public List<int> correctOrder;

    public override string ToString() => $"[Sequence] {question}";
}