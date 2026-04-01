using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private EventReference _deathSound;

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        RuntimeManager.PlayOneShot(_deathSound);
    }
}
