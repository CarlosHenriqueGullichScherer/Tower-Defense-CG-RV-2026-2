using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class RankingEntry
{
    public string playerName;
    public int score;
    public int kills;
    public float time;
    public bool victory;
    public string date;
}

[Serializable]
public class RankingData
{
    public List<RankingEntry> entries = new List<RankingEntry>();
}

public static class RankingManager
{
    public const int MaxEntries = 10;

    private static string Key(Difficulty difficulty)
    {
        return "ranking_" + difficulty;
    }

    public static List<RankingEntry> Load(Difficulty difficulty)
    {
        string json = PlayerPrefs.GetString(Key(difficulty), "");
        if (string.IsNullOrEmpty(json))
            return new List<RankingEntry>();

        RankingData data = JsonUtility.FromJson<RankingData>(json);
        return data != null && data.entries != null ? data.entries : new List<RankingEntry>();
    }

    public static int Add(Difficulty difficulty, RankingEntry entry)
    {
        List<RankingEntry> entries = Load(difficulty);
        entries.Add(entry);

        entries = entries
            .OrderByDescending(e => e.score)
            .ThenBy(e => e.time)
            .Take(MaxEntries)
            .ToList();

        Save(difficulty, entries);
        return entries.IndexOf(entry) + 1;
    }

    public static void Clear()
    {
        foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
            PlayerPrefs.DeleteKey(Key(difficulty));

        PlayerPrefs.Save();
    }

    private static void Save(Difficulty difficulty, List<RankingEntry> entries)
    {
        RankingData data = new RankingData { entries = entries };
        PlayerPrefs.SetString(Key(difficulty), JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }
}
