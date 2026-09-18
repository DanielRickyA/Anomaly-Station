using UnityEngine;
using System.Collections.Generic;

public class StationManager : MonoBehaviour
{
    [SerializeField] private TicketData[] masterTicketList;
    [SerializeField] private int currentLevel = 0;
    private TicketData targetTicket;

    private void Awake()
    {
        GenerateRandomTarget();
    }

    private void GenerateRandomTarget()
    {
        int maxIndex = Mathf.Clamp(currentLevel + 1, 1, masterTicketList.Length);
        int randomIndex = Random.Range(0, maxIndex);
        targetTicket = masterTicketList[randomIndex];
        Debug.Log("Target Player harus naik: " + targetTicket.namaKereta);
        Debug.Log("Loket yang harus dicari: " + targetTicket.namaLoket);
        Debug.Log("Gerbong yang didapat: " + targetTicket.namaGerbong);
    }

    public TicketData GetTargetTicket()
    {
        return targetTicket;
    }
}
