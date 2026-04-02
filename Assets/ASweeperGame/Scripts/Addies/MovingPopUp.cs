using UnityEngine;
using FMODUnity;

public class MovingPopUp : MonoBehaviour
{
    [SerializeField] private EventReference _popUpCloseSound;
    public void ClosePopUp()
    {
        RuntimeManager.PlayOneShot(_popUpCloseSound);
        gameObject.SetActive(false);

    }
}
