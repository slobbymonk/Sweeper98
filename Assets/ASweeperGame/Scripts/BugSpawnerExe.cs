using FMODUnity;
using PrimeTween;
using UnityEngine;

public class BugSpawnerExe : MonoBehaviour
{
    [SerializeField] private GameObject _bugPrefab;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private EventReference _spawnSound;
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

        RuntimeManager.PlayOneShot(_spawnSound);
        Tween.PunchScale(transform, Vector3.one * 1.01f, .2f, 10, true, Ease.OutBounce);
        Tween.PunchLocalRotation(transform, Vector3.one * 10f, .2f);
    }
}
