using System;
using UnityEngine;

public class LevelCompleteArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() == null)
            return;
        
        FindFirstObjectByType<RunManager>().OnLevelComplete();
    }
}
