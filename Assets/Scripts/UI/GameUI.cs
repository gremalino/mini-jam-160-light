using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TimingUI _timeUI;

    public void UpdateTimes(RunState runState)
    {
        _timeUI.UpdateTimes(runState);
    }

    public void OnWorldChange(int newWorld, PlayerState playerState)
    {
        _timeUI.OnWorldChange(newWorld, playerState);
    }
}
