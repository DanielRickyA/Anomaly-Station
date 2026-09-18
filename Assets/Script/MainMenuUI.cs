using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private int level1BuildIndex = 1;

    void Start()
    {
        int savedLevel = PlayerPrefs.GetInt("SavedLevel", level1BuildIndex);
        if (savedLevel > level1BuildIndex)
        {
            continueButton.gameObject.SetActive(true);
        }
        else
        {
            continueButton.gameObject.SetActive(false);
        }

        if (playButton != null)
            playButton.onClick.AddListener(OnPlayPressed);

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinuePressed);
    }

    public void OnPlayPressed()
    {
        SceneManager.LoadScene(level1BuildIndex);
    }

    public void OnContinuePressed()
    {
        int savedLevel = PlayerPrefs.GetInt("SavedLevel", level1BuildIndex);
        SceneManager.LoadScene(savedLevel);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("SavedLevel");
        if (continueButton != null) continueButton.gameObject.SetActive(false);
    }
}
