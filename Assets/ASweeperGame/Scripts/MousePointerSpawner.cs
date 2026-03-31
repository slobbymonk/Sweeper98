using UnityEditor;
using UnityEngine;

public class MousePointerSpawner : MonoBehaviour
{
    [SerializeField] GameObject _mousePointerPrefab;
    [SerializeField] Transform _popupDropoffZone;

    void Start()
    {
        GameObject go = Instantiate(_mousePointerPrefab, new Vector3(2, 2, 0), Quaternion.identity);
        go.GetComponentInChildren<StateMachine>().Init(_popupDropoffZone);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
