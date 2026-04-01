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
        Vector3 direction = _playerController.transform.position - transform.position;
        transform.position += direction.normalized * _speed * Time.deltaTime;

        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)-90f;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    public void Die()
    {
        Destroy(gameObject);
    }

}
