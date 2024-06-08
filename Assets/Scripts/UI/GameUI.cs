using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TimingUI _timeUI;

    public void UpdateTimes(RunState runState)
    {
        _timeUI.UpdateTimes(runState);
    }

    public void OnWorldChange(int newWorld)
    {
        _timeUI.OnWorldChange(newWorld);
    }
}
