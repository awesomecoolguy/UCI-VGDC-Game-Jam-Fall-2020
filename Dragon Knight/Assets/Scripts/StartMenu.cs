using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public string startScene;
    public Button startButton;
    public AudioClip clickSound;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Get();

        startButton.onClick.AddListener(() =>
        {
            gameManager.PlaySFX(clickSound);
            SceneManager.LoadScene(startScene);
        });
    }
}
