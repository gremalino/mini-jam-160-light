using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimingUI : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _timeText;
    [SerializeField] private TMP_Text _personalBestText;
    [SerializeField] private TMP_Text _sumOfBestText;
    [SerializeField] private List<TimeListItem> _listItems;
    
    public void UpdateTimes(RunState runState)
    {
        SetMainTimeString(runState.RunTime);
    }

    public void OnWorldChange(int newWorld)
    {
        for (int i = 0; i < _listItems.Count; i++)
        {
            _listItems[i].SetHighlightActive(i == newWorld);
        }
    }

    private string GetTimeString(float totalSeconds)
    {
        int minutes = (int) totalSeconds / 60;
        int seconds = (int) totalSeconds - (minutes * 60);
        int ms = (int)((totalSeconds - seconds) * 10);

        string secondsString = seconds.ToString("D2");
        return $"{minutes}:{seconds}.{ms}";
    }

    // Yes this is stupid but it allows us to use a non-monospace font without it jittering
    private void SetMainTimeString(float totalSeconds)
    {
        int minutes = (int) totalSeconds / 60;
        int seconds = (int) totalSeconds - (minutes * 60);
        int ms = (int)((totalSeconds - (int) totalSeconds) * 10);

        _timeText[0].text = minutes.ToString();
        
        _timeText[2].text = (seconds / 10).ToString();
        _timeText[3].text = (seconds % 10).ToString();

        _timeText[5].text = ms.ToString();
    }
}
