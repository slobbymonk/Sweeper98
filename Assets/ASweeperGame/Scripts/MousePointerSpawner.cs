using UnityEditor;
using UnityEngine;

public class MousePointerSpawner : MonoBehaviour
{
    [SerializeField] GameObject _mousePointerPrefab;
    [SerializeField] Transform _popupDropoffZone;

    int _nrOfMousePointers = 0;
    void Start()
    {
        SpawnMousePointer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDifficultyScalar(float scalar)
    {
        if(scalar > 2 && _nrOfMousePointers == 1)
        {
            SpawnMousePointer();
        }
        else if(scalar > 2.9 && _nrOfMousePointers == 2)
        {
            SpawnMousePointer();
        }
    }


    private void SpawnMousePointer()
    {
        _nrOfMousePointers++;
        GameObject newMousePointer = Instantiate(_mousePointerPrefab, transform.position, Quaternion.identity);
        StateMachine stateMachine = newMousePointer.GetComponentInChildren<StateMachine>();
        stateMachine.Init(_popupDropoffZone);
    }
}
