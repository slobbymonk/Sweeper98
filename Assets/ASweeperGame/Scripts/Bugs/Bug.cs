using UnityEngine;

public class Bug : MonoBehaviour
{
    private PlayerController _playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _rotationSpeed = 5f;
    void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }


    void Update()
    {
        Vector3 targetPosition = _playerController.transform.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _speed * Time.deltaTime);

        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
}
