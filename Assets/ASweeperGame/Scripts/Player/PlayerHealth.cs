using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private EventReference _deathSound;

    public void Die()
    {
        RuntimeManager.PlayOneShot(_deathSound);
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.GameOver);
    }
}
