using UnityEngine;

public class LevelContainer : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _bottomRight;
    [SerializeField] private Transform _topLeft;

    public Vector3 SpawnPosition => _spawnPoint.position;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
