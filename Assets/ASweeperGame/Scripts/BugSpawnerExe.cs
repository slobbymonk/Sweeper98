using UnityEngine;

public class BugSpawnerExe : MonoBehaviour
{
    [SerializeField] private GameObject _bugPrefab;
    [SerializeField] private float _spawnInterval = 2f;
    private float _spawnTimer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnInterval)
        {
            SpawnBug();
            _spawnTimer = 0f;
        }
    }

    private void SpawnBug()
    {
        Instantiate(_bugPrefab, transform.position, Quaternion.identity);
    }
}
