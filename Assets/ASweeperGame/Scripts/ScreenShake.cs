using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [Header("Spring Settings")]
    [SerializeField] private float springStrength = 50f; 
    [SerializeField] private float damping = 5f;         
    [SerializeField] private float mass = 3f;            
    [SerializeField] private float _maxVelocity = 10;

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition = Vector3.zero;

    private List<ShakeInstance> activeShakes = new List<ShakeInstance>();

    public static ScreenShake Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void LateUpdate()
    {
        float dt = Time.deltaTime;

        targetPosition = Vector3.zero;

        for (int i = activeShakes.Count - 1; i >= 0; i--)
        {
            ShakeInstance shake = activeShakes[i];

            if (shake.type == ShakeType.Target)
            {
                // Target-based shake (explosions, jitter)
                targetPosition += (Vector3)(Random.insideUnitCircle * shake.intensity);

                if (shake.duration > 0f)
                {
                    shake.remainingTime -= dt;
                    if (shake.remainingTime <= 0f)
                    {
                        activeShakes.RemoveAt(i);
                        continue;
                    }
                }
            }
        }

        // Spring physics toward origin + target offset
        Vector3 displacement = targetPosition - transform.localPosition;
        Vector3 springForce = displacement * springStrength;
        Vector3 dampingForce = -velocity * damping;
        Vector3 acceleration = (springForce + dampingForce) / mass;

        velocity += acceleration * dt;
        velocity = Vector3.ClampMagnitude(velocity, _maxVelocity);
        transform.localPosition += velocity * dt;
    }

    /// <summary>
    /// Adds a normal target-based shake (e.g., explosion).
    /// </summary>
    public void ShakeTarget(string shakeId, float intensity = .3f, float duration = .2f)
    {
        ShakeInstance shake = new ShakeInstance
        {
            id = shakeId,
            type = ShakeType.Target,
            intensity = intensity,
            duration = duration,
            remainingTime = duration
        };
        activeShakes.Add(shake);
    }

    /// <summary>
    /// Adds an impulse punch (applied immediately to velocity).
    /// </summary>
    public void ShakePunch(Vector2 direction, float intensity = 1.5f)
    {
        velocity += (Vector3)direction.normalized * intensity * 100 * Time.deltaTime;
    }

    /// <summary>
    /// Removes a shake by its ID.
    /// </summary>
    public void RemoveShake(string shakeId)
    {
        activeShakes.RemoveAll(s => s.id == shakeId);
    }

    private enum ShakeType { Target, Impulse }

    private class ShakeInstance
    {
        public string id;
        public ShakeType type;
        public float intensity;
        public float duration;
        public float remainingTime;
        public Vector2 direction;
    }
}
