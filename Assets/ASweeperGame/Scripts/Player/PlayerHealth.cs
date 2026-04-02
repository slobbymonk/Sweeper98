using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private EventReference _deathSound;

    [SerializeField] private List<Collision2D> _collidersBeingTouched = new List<Collision2D>();

    private bool _isDead;

    public void Die()
    {
        if(_isDead) return;

        _isDead = true;
        RuntimeManager.PlayOneShot(_deathSound);
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.GameOver);
    }

    private void Update()
    {
        if (!IsSqueezed()) return;

        Die();
    }



    private Dictionary<Collider2D, Vector2> _contactNormals = new Dictionary<Collider2D, Vector2>();

    bool IsSqueezed()
    {
        var normals = new List<Vector2>(_contactNormals.Values);
        for (int i = 0; i < normals.Count; i++)
            for (int o = i + 1; o < normals.Count; o++)
                if (Vector2.Dot(normals[i], normals[o]) < -0.5f)
                    return true;
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bug"))
        {
            Destroy(collision.otherCollider);
            Die();
        }

        UpdateNormal(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        UpdateNormal(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _contactNormals.Remove(collision.collider);
    }

    private void UpdateNormal(Collision2D collision)
    {
        Vector2 avg = Vector2.zero;
        foreach (ContactPoint2D contact in collision.contacts)
            avg += contact.normal;
        _contactNormals[collision.collider] = avg.normalized;
    }
}
