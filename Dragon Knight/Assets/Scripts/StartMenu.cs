using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public string startScene;
    public Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(() => SceneManager.LoadScene(startScene));
    }
}
