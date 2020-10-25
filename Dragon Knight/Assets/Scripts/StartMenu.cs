using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public string startScene;
    public Button startButton;
    public GameObject howToPlayPanel;
    public GameObject creditsPanel;
    public AudioClip clickSound;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Get();
    }

    public void StartGame()
    {
        gameManager.PlaySFX(clickSound);
        SceneManager.LoadScene(startScene);
    }

    public void ToggleHowToPlay(bool active)
    {
        gameManager.PlaySFX(clickSound);
        howToPlayPanel.SetActive(active);
    }

    public void ToggleCredits(bool active)
    {
        gameManager.PlaySFX(clickSound);
        creditsPanel.SetActive(active);
    }
}
