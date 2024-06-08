using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndUI : MonoBehaviour
{
    [SerializeField] private List<LevelEndSummaryItem> _listItems;
    
    public void OnGameComplete(RunState run, PlayerState player)
    {
        gameObject.SetActive(true);
        
        for (int i = 0; i < 3; i++)
        {
            float levelTime = run.CurrentTimes[i];
            float bestTime = run.BestTimes[i];
            
            _listItems[i].SetTimes(levelTime, bestTime);
        }
    }

    public void ButtonPressAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void ButtonPressShop()
    {
        
    }

    public void ButtonPressMenu()
    {
        
    }
}
