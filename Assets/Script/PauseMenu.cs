using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Deteksi tombol Escape dipencet
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                Continue();
            }
            else
            {
                PauseGame(); // Jika belum pause, buka menu pause
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

        TimeManager timeManager = FindAnyObjectByType<TimeManager>();
        if (timeManager != null)
        {
            timeManager.StopTimer();
        }

        PlayerInteractionUI playerUI = FindAnyObjectByType<PlayerInteractionUI>();
        if (playerUI != null)
        {
            playerUI.SetPlayerLock(true);
        }

        Time.timeScale = 0f;
    }

    public void Continue()
    {
        isPaused = false;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        Time.timeScale = 1f;

        PlayerInteractionUI playerUI = FindAnyObjectByType<PlayerInteractionUI>();
        if (playerUI != null)
        {
            playerUI.SetPlayerLock(false);
        }

        TimeManager timeManager = FindAnyObjectByType<TimeManager>();
        if (timeManager != null)
        {
            timeManager.StartTimer();
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
