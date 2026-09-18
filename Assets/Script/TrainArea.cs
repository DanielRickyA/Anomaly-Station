using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainArea : MonoBehaviour
{
    [SerializeField] private string carriageName;
    [SerializeField] private GameObject correctIndicator;
    [SerializeField] private GameObject wrongIndicator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip winSFX;
    [SerializeField] private float delayBeforeNextLevel = 2f;

    private bool isTargetCarriage = false;
    private bool isGuidanceActive = false;
    private bool hasWon = false;
    private PlayerInteractionUI playerUI;


    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        HideAllIndicators();
    }

    public void HideAllIndicators()
    {
        if (correctIndicator != null) correctIndicator.SetActive(false);
        if (wrongIndicator != null) wrongIndicator.SetActive(false);
        isGuidanceActive = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isGuidanceActive && !hasWon)
        {
            playerUI = collision.GetComponent<PlayerInteractionUI>();

            if (isTargetCarriage)
            {
                hasWon = true;

                if (audioSource != null && winSFX != null)
                {
                    audioSource.PlayOneShot(winSFX, 2.0f);
                }

                TimeManager timeManager = FindAnyObjectByType<TimeManager>();
                if (timeManager != null)
                {
                    timeManager.StopTimer();
                }

                Debug.Log("Pemain Masuk Gerbong yang Benar!");

                SaveAndLoadNextLevel();
            }
            else
            {
                if (playerUI != null)
                {
                    playerUI.ShowPrompt(true, "I think this is the wrong carriage...");
                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerUI != null)
            {
                playerUI.ShowPrompt(false);
                playerUI = null;
            }
        }
    }

    private void SaveAndLoadNextLevel()
    {
        int curreentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextScene = curreentSceneIndex + 1;
        PlayerPrefs.SetInt("SavedLevel", nextScene);
        PlayerPrefs.Save();
        Invoke("LoadNextLevel", delayBeforeNextLevel);
    }

    private void LoadNextLevel()
    {
        int curreentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextScene = curreentSceneIndex + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
            nextScene = 0;

        SceneManager.LoadScene(nextScene);
    }

    public void ActivateTrainAreaGuidance(string targetCarriageName)
    {
        isGuidanceActive = true;
        isTargetCarriage = (carriageName == targetCarriageName);

        if (isTargetCarriage)
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
