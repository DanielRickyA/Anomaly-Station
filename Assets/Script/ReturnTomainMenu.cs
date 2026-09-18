using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ReturnTomainMenu : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.escapeKey.isPressed || Gamepad.current != null && Gamepad.current.startButton.isPressed)
        {
            SceneManager.LoadScene(0);
        }
    }
}
