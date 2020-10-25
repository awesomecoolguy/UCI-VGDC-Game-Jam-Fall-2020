using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public Button continueButton;
    public Button restartButton;
    public Button startMenuButton;
    public AudioClip clickSound;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Get();
    }

    private void Start()
    {
        // Hook the buttons to GameManager functions.
        continueButton.onClick.AddListener(() =>
        {
            gameManager.PlaySFX(clickSound);
            gameManager.ToggleGameMenu();
        });
        restartButton.onClick.AddListener(() =>
        {
            gameManager.PlaySFX(clickSound);
            gameManager.GameOver();
            gameManager.ResumeGame();
        });
        startMenuButton.onClick.AddListener(() =>
        {
            gameManager.PlaySFX(clickSound);
            gameManager.BackToStart();
            gameManager.ResumeGame();
        });
    }
}
