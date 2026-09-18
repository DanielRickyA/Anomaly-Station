using UnityEngine;
using TMPro;
using System;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float timeRemaining = 60f;
    [SerializeField] private bool isTimerRunning = false;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gameOverSFX;

    void Start()
    {        
        isTimerRunning = true;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTimerRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            timeRemaining = 0;
            isTimerRunning = false;
            DisplayTime(timeRemaining);
            OnTimeExpired();
        }

    }


    private void DisplayTime(float displayTime)
    {
        float minutes = Mathf.FloorToInt(displayTime / 60);
        float seconds = Mathf.FloorToInt(displayTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    private void OnTimeExpired()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        PlayerInteractionUI playerUI = FindAnyObjectByType<PlayerInteractionUI>();
        if (playerUI != null)
        {
            playerUI.SetPlayerLock(true);
        }

        if (audioSource != null && gameOverSFX != null)
        {
            audioSource.PlayOneShot(gameOverSFX, 2.0f);
        }

        Time.timeScale = 0f;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        Debug.Log("Timer Dihentikan!");
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }
}
