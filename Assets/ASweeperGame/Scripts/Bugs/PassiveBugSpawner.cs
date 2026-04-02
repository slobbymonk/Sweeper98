using UnityEngine;

public class PassiveBugSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _bugPrefab;
    [SerializeField] private float _spawnInterval = 3f;
    private float _originalSpawnInterval;
    private float _timer;
    public bool CanSpawn = true;

    public void SetDifficultyScaler(float scaler)
    {
        _spawnInterval = _originalSpawnInterval / scaler;
    }


    private void Start()
    {
        _originalSpawnInterval = _spawnInterval;
    }

    void Update()
    {
        if (!CanSpawn) return;
        _timer += Time.deltaTime;
        if (_timer >= _spawnInterval)
        {
            _timer = 0f;
            SpawnBug();
        }
    }

    private void SpawnBug()
    {
        Camera cam = Camera.main;
        Vector3 spawnPos = GetOffscreenPosition(cam);
        Instantiate(_bugPrefab, spawnPos, Quaternion.identity);
    }

    private Vector3 GetOffscreenPosition(Camera cam)
    {
        int edge = Random.Range(0, 4);
        float margin = 0.1f;

        Vector3 viewportPos = edge switch
        {
            0 => new Vector3(Random.Range(0f, 1f), 1f + margin, 0f),
            1 => new Vector3(Random.Range(0f, 1f), -margin, 0f),
            2 => new Vector3(-margin, Random.Range(0f, 1f), 0f),
            _ => new Vector3(1f + margin, Random.Range(0f, 1f), 0f),
        };

        viewportPos.z = Mathf.Abs(cam.transform.position.z);
        return cam.ViewportToWorldPoint(viewportPos);
    }
}