using System;
using System.Collections.Generic;

[Serializable]
public class Player
{
    public List<LevelEntry> levels = new List<LevelEntry>();
}

[Serializable]
public class LevelEntry
{
    public int level;
    public int stages;
    public bool isAvailable;
    public bool isComplete;

    public LevelEntry(int level, bool isAvailable)
    {
        this.level = level;
        this.isAvailable = isAvailable;
    }
}