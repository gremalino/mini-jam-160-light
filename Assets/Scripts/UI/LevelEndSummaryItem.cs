using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelEndSummaryItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _worldText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _bestTimeText;
    [SerializeField] private TMP_Text _newRecordText;
    
    public void SetTimes(float currentTime, float previousBest)
    {
        if (currentTime < previousBest || previousBest == 0.0f)
        {
            _newRecordText.gameObject.SetActive(true);
            _bestTimeText.gameObject.SetActive(false);
        }
        else
        {
            _newRecordText.gameObject.SetActive(false);
            _bestTimeText.gameObject.SetActive(true);    
        }

        _timeText.text = GetTimeString(currentTime);
        _bestTimeText.text = "best: " + GetTimeString(previousBest);
    }
    
    private string GetTimeString(float totalSeconds)
    {
        int minutes = (int) totalSeconds / 60;
        int seconds = (int) totalSeconds - (minutes * 60);
        int ms = (int)((totalSeconds - (int) totalSeconds) * 10);

        string secondsString = seconds.ToString("D2");
        return $"{minutes}:{secondsString}.{ms}";
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
