using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Screenshotter : MonoBehaviour
{
    [Header("Press Space to take a screenshot")]
    [Range(1, 5)]
    [SerializeField] private int size = 1;
    [SerializeField] private string _path;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            string folder = _path;

            string filename =
                "screenshot_" +
                System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") +
                ".png";

            string fullPath = Path.Combine(folder, filename);

            ScreenCapture.CaptureScreenshot(fullPath, size);

            Debug.Log("Picture Taken: " + fullPath);
        }
    }
}
