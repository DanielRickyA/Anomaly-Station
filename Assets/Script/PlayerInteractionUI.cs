using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerInteractionUI : MonoBehaviour
{
    [SerializeField] private GameObject promptTextContainer;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private MonoBehaviour playerMovementScript;

    private void Start()
    {
        ShowPrompt(false);
    }

    public void ShowPrompt(bool show, string text = "Press E to Interact")
    {
        if (promptTextContainer != null)
        {
            if (promptText != null) promptText.text = text;
            promptTextContainer.SetActive(show);
        }
    }

    public void SetPlayerLock(bool isLocked)
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = !isLocked;
        }
    }

}
