using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SecurityNPC : MonoBehaviour
{
    [SerializeField] private GameObject highlightIndicator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip interactSFX;
    private bool isPlayerNearby = false;
    private bool hasInteracted = false;
    private PlayerInteractionUI playerUI;

    void Start()
    {
        if (highlightIndicator != null)
        {
            highlightIndicator.SetActive(true);
        }
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isPlayerNearby && !hasInteracted &&
        (Keyboard.current.eKey.wasPressedThisFrame || 
        (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame)))
        {
            Interact();
        }
    }

    private void Interact()
    {
        hasInteracted = true;


        if (audioSource != null && interactSFX != null)
        {
            audioSource.PlayOneShot(interactSFX);
        }

        if (highlightIndicator != null)
        {
            highlightIndicator.SetActive(false);
        }

        if (playerUI != null)
        {
            playerUI.ShowPrompt(false);
        }

        StationUIDisplay uiDisplay = FindAnyObjectByType<StationUIDisplay>();
        if (uiDisplay != null)
        {
            uiDisplay.SetQuestState(StationUIDisplay.QuestState.FindCounter);
            Debug.Log("Interaksi Security Berhasil! Quest lanjut ke FindCounter.");
        }

        StationManager stationManager = FindAnyObjectByType<StationManager>();
        if (stationManager != null)
        {
            string currentTargetLoket = stationManager.GetTargetTicket().namaLoket;

            TicketCounter[] allCounters = FindObjectsByType<TicketCounter>(FindObjectsSortMode.None);
            foreach (var counter in allCounters)
            {
                counter.ActivateCounterGuidance(currentTargetLoket);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasInteracted)
        {
            isPlayerNearby = true;
            playerUI = collision.GetComponent<PlayerInteractionUI>();
            if (playerUI != null)
            {
                playerUI.ShowPrompt(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
        if (playerUI != null)
        {
            playerUI.ShowPrompt(false);
            playerUI = null;
        }
    }
}
