using UnityEngine;

public class BombManager : MonoBehaviour
{
    [SerializeField] private Transform _bombSpawnArea;
    [SerializeField] private int _rowCount;
    [SerializeField] private int _columnCount;

    [SerializeField] private float _bombTrySpawnTime = 5f;
    private float _timeSinceLastBombSpawnAttempt = 0f;

    [SerializeField] private GameObject bombPrefab;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeSinceLastBombSpawnAttempt += Time.deltaTime;
        if (_timeSinceLastBombSpawnAttempt >= _bombTrySpawnTime)
        {
            _timeSinceLastBombSpawnAttempt = 0f;
            SpawnBomb();
        }
    }

    private Vector2 GetRandomPositionInSpawnArea()
    {
        Vector2 randomPosition = new Vector2();
        int attempts = 0;

        Vector2 cellSize = new Vector2(
            _bombSpawnArea.localScale.x / _columnCount,
            _bombSpawnArea.localScale.y / _rowCount);
        do
        {
            attempts++;
            int row = Random.Range(0, _rowCount);
            int column = Random.Range(0, _columnCount);

            randomPosition = new Vector2(column * cellSize.x + cellSize.x/2f, row * cellSize.y + cellSize.y / 2f)
                + new Vector2(-_bombSpawnArea.localScale.x / 2f, -_bombSpawnArea.localScale.y/2f)
                + (Vector2)_bombSpawnArea.position;

        } while (IsOverlappingExistingBomb(randomPosition) && attempts < 400);
        return randomPosition;
    }
    private void SpawnBomb()
    {
        Vector2 spawnPosition = GetRandomPositionInSpawnArea();
        Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
    }

    private bool IsOverlappingExistingBomb(Vector2 bombArea)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(bombArea, 0.5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Bomb"))
            {
                return true;
            }
        }
        return false;
    }
}
