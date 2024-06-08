using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerState
{
    public List<int> BestSectionTimes;
    public List<int> BestRunTimes;
    public int BestOverallTime;
    public List<string> UnlockedAchievements;

    private const string PREFS_KEY = "PlayerState";
    
    public void SaveToPrefs()
    {
        string jsonString = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(PREFS_KEY, jsonString);
        Debug.Log(jsonString);
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
        BestSectionTimes = new List<int>();
        BestRunTimes = new List<int>();
        BestOverallTime = 0;
        UnlockedAchievements = new List<string>();
        
        if (PlayerPrefs.HasKey(PREFS_KEY) == false)
            return;
        
        PlayerPrefs.DeleteKey(PREFS_KEY);
        PlayerPrefs.Save();
    }
}
