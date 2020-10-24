using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public string startMenuScene;
    public GameObject gameUI;
    public GameObject gameOverMenu;

    private string currentSceneName;
    private int score = 0;
    private int lastScore;
    private GameUI gameUiInstance;

    private void Awake()
    {
        // Make sure there is always only one instance of GameManager.
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Make sure the GameManager persists across scenes.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Initialize level when the scene is changed.
        currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.activeSceneChanged += (Scene currentScene, Scene nextScene) =>
        {
            currentSceneName = nextScene.name;
            InitializeLevel();
        };
        InitializeLevel();
    }

    /// <summary>
    /// Gets the GameManager instance.
    /// </summary>
    public static GameManager Get()
    {
        return instance;
    }

    private void InitializeLevel()
    {
        // Don't create UI on start menu scene.
        if (currentSceneName == startMenuScene)
        {
            score = 0;
            return;
        }

        // Save last score.
        lastScore = score;

        // Clone Game UI prefab.
        GameObject uiObject = Instantiate(gameUI);
        uiObject.name = "Game UI";
        uiObject.transform.SetAsFirstSibling();
        gameUiInstance = uiObject.GetComponent<GameUI>();

        // Initialize UI variables.
        gameUiInstance.SetScore(score);
    }

    /// <summary>
    /// Sets the player health in the UI.
    /// </summary>
    /// <param name="amount">Health amount.</param>
    public void SetHealth(int newHealth)
    {
        gameUiInstance.SetHealth(newHealth);
    }

    /// <summary>
    /// Adds to the current score.
    /// </summary>
    /// <param name="amount">Amount to be added</param>
    public void AddScore(int amount)
    {
        score += amount;
        gameUiInstance.SetScore(score);
    }

    /// <summary>
    /// Triggers that the game is over.
    /// </summary>
    public void GameOver()
    {
        // Restart scene.
        score = lastScore;
        SceneManager.LoadScene(currentSceneName);
    }

    /// <summary>
    /// Triggers that the game is ending.
    /// </summary>
    public void GameEnding()
    {
        score = 0;
    }
}
