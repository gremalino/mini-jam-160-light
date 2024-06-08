using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerState
{
    public List<int> BestTimes;
    public int BestOverallTime;
    public List<string> UnlockedAchievements;

    private const string PREFS_KEY = "PlayerState";
    
    public void SaveToPrefs()
    {
        string jsonString = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(PREFS_KEY, jsonString);
        PlayerPrefs.Save();
    }

    public void LoadFromPrefs()
    {
        if (PlayerPrefs.HasKey(PREFS_KEY) == false)
            return;

        string jsonString = PlayerPrefs.GetString(PREFS_KEY);
        JsonUtility.FromJsonOverwrite(jsonString, this);
    }

    public void Wipe()
    {
        if (PlayerPrefs.HasKey(PREFS_KEY) == false)
            return;
        
        PlayerPrefs.DeleteKey(PREFS_KEY);
        PlayerPrefs.Save();
    }
}
