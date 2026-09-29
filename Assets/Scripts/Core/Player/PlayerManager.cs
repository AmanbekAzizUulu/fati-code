using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public Player Player { get; private set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Player = Load();
    }
    public void Save()
    {
        string json = JsonUtility.ToJson(Player, true);
        PlayerPrefs.SetString("save", json);
        PlayerPrefs.Save();
    }

    public Player Load()
    {
        Player player = null;
        if (!PlayerPrefs.HasKey("save"))
        {
            player = new Player();
        }
        else
        {
            player = JsonUtility.FromJson<Player>(PlayerPrefs.GetString("save"));
        }

        return player;
    }
    public bool HasLevelEntries()
    {
        return Player.levels.Count > 0;
    }
    public void AddNewLevel(LevelEntry entry)
    {
        Player.levels.Add(entry);
        Save();
    }
    public void SetLevelStage(LevelEntry entry, int stage)
    {
        entry.stages = stage;
        Save();
    }
    public void SetLevelAvaibility(int level)
    {
        var entry = GetLevelEntry(level);
        if (entry == null)
            return;

        entry.isAvailable = true;
        Save();
    }
    public LevelEntry GetLevelEntry(int level)
    {
        for(int i = 0; i < Player.levels.Count; i++)
        {
            if(Player.levels[i].level == level)
            {
                return Player.levels[i];
            }
        }
        return null;
    }
    public int GetCurrentLevel()
    {
        return Mathf.Max(Player.levels.Count - 1, 0);
    }
}
