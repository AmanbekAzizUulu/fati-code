using System;

[Serializable]
public class SingleChoiceTask : ITask
{
    public SingleChoiceTaskEntry Entry { get; private set; }

    private int answer;
    public SingleChoiceTask(SingleChoiceTaskEntry entry)
    {
        Entry = entry;
        answer = -1;
    }

    public void SetAnswer(int answer)
    {
        this.answer = answer;
    }
    public bool Validate()
    {
        return answer == Entry.correctIndex;
    }
}