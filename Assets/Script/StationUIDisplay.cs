using UnityEngine;
using TMPro;
using System;

public class StationUIDisplay : MonoBehaviour
{
    [SerializeField] private StationManager stationManager;
    [SerializeField] private TMP_Text keretaText;
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private TMP_Text carriageText;
    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private GameObject uiTicketIcon;

    public enum QuestState
    {
        FindSecurity,
        FindCounter,
        FindCarriage
    }
    private QuestState currentState = QuestState.FindSecurity;

    void Start()
    {
        if (stationManager == null)
        {
            stationManager = FindAnyObjectByType<StationManager>();
        }

        SetQuestState(QuestState.FindSecurity);
    }

    public void SetQuestState(QuestState newState)
    {
        currentState = newState;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (stationManager == null) return;

        TicketData activeTicket = stationManager.GetTargetTicket();
        Debug.Log("Active Ticket: " + activeTicket);

        switch (currentState)
        {
            case QuestState.FindSecurity:
                keretaText.text = "Train Name: " + activeTicket.namaKereta;
                counterText.text = "Ticket Counter: ???";
                carriageText.text = "Carriage: ???";
                uiTicketIcon.SetActive(false);
                objectiveText.text = "Ask security for Ticket Counter info!";
                break;

            case QuestState.FindCounter:
                keretaText.text = "Train Name: " + activeTicket.namaKereta;
                counterText.text = "Ticket Counter: " + activeTicket.namaLoket;
                carriageText.text = "Carriage: ???";
                uiTicketIcon.SetActive(false);
                objectiveText.text = "Go to the Ticket Counter to print your ticket!";
                break;

            case QuestState.FindCarriage:
                keretaText.text = "Train Name: " + activeTicket.namaKereta;
                counterText.text = "Ticket Counter: " + activeTicket.namaLoket;
                carriageText.text = "Carriage: " + activeTicket.namaGerbong;
                objectiveText.text = "Find your Carriage before the train leaves!";
                if (uiTicketIcon != null) uiTicketIcon.SetActive(true);
                break;
        }
    }
}
