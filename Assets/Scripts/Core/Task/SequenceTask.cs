using System;
using System.Collections.Generic;

[Serializable]
public class SequenceTask : ITask
{
    public SequenceTaskEntry Entry { get; private set; }

    private List<int> answer = new List<int>();
    public SequenceTask(SequenceTaskEntry entry)
    {
        Entry = entry;
    }
    public void SetAnswer(List<int> answer)
    {
        this.answer = answer;
    }

    public bool Validate()
    {
        if (answer.Count != Entry.correctOrder.Count)
            return false;

        for (int i = 0; i < answer.Count; i++)
        {
            if (answer[i] != Entry.correctOrder[i])
                return false;
        }

        return true;
    }
}