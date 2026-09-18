using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TicketCounter : MonoBehaviour
{
    [SerializeField] private string counterName;
    [SerializeField] private GameObject correctIndicator;
    [SerializeField] private GameObject wrongIndicator;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successSFX;

    private bool isPlayerNearby = false;
    private bool isTargetCounter = false;
    private bool isGuidanceActive = false;

    private PlayerInteractionUI playerUI;


    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        HideAllIndicators();

    }

    void Update()
    {
        if (isPlayerNearby && isGuidanceActive && isTargetCounter &&
        (Keyboard.current.eKey.wasPressedThisFrame || 
        (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame)))
        {
            Interact();
        }

    }

    private void HideAllIndicators()
    {
        if (correctIndicator != null) correctIndicator.SetActive(false);
        if (wrongIndicator != null) wrongIndicator.SetActive(false);
        isGuidanceActive = false;
    }
    private void Interact()
    {
        if (audioSource != null && successSFX != null)
        {
            audioSource.PlayOneShot(successSFX, 2.0f);
        }

        if (playerUI != null) playerUI.ShowPrompt(false);

        TicketCounter[] allCounters = FindObjectsByType<TicketCounter>(FindObjectsSortMode.None);
        foreach (var counter in allCounters)
        {
            counter.HideAllIndicators();
        }

        StationUIDisplay uiDisplay = FindAnyObjectByType<StationUIDisplay>();
        if (uiDisplay != null)
        {
            uiDisplay.SetQuestState(StationUIDisplay.QuestState.FindCarriage);
            Debug.Log("Tiket berhasil dicetak! Lanjut ke Fase 3: FindCarriage");
        }

        StationManager stationManager = FindAnyObjectByType<StationManager>();
        if (stationManager != null)
        {
            string targetCarriage = stationManager.GetTargetTicket().namaGerbong;
            TrainArea[] allTrainAreas = FindObjectsByType<TrainArea>(FindObjectsSortMode.None);
            foreach (var area in allTrainAreas)
            {
                area.ActivateTrainAreaGuidance(targetCarriage);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isGuidanceActive)
        {
            isPlayerNearby = true;
            playerUI = collision.GetComponent<PlayerInteractionUI>();

            if (playerUI != null)
            {
                if (isTargetCounter) playerUI.ShowPrompt(true);
                else playerUI.ShowPrompt(true, "I think I'm at the wrong counter...");
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (playerUI != null)
            {
                playerUI.ShowPrompt(false);
                playerUI = null;
            }
        }
    }

    public void ActivateCounterGuidance(string targetCounterName)
    {
        isTargetCounter = (counterName == targetCounterName);
        isGuidanceActive = true;

        if (isTargetCounter)
        {
            if (correctIndicator != null) correctIndicator.SetActive(true);
            if (wrongIndicator != null) wrongIndicator.SetActive(false);
        }
        else
        {
            if (correctIndicator != null) correctIndicator.SetActive(false);
            if (wrongIndicator != null) wrongIndicator.SetActive(true);
        }
    }
}
