using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private EventReference _deathSound;

    [SerializeField] private List<Collision2D> _collidersBeingTouched;

    public void Die()
    {
        RuntimeManager.PlayOneShot(_deathSound);
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.GameOver);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!_collidersBeingTouched.Contains(collision)) return;

        
    }
}
