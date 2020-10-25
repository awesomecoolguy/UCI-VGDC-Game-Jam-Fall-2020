using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public string startMenuScene;
    public GameObject gameUI;
    public GameObject gameMenu;

    [Header("SFX & BGM")]
    public AudioClip gameMenuSound;
    public AudioClip startBGM;
    public AudioClip gameBGM;

    private string currentSceneName;
    private int score = 0;
    private int lastScore;
    private GameUI gameUiInstance;
    private GameMenu gameMenuInstance;
    private AudioSource audioSource;

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
        // Get reference.
        audioSource = GetComponent<AudioSource>();

        // Initialize level when the scene is changed.
        currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.activeSceneChanged += (Scene currentScene, Scene nextScene) =>
        {
            currentSceneName = nextScene.name;
            InitializeLevel();
        };
        InitializeLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && currentSceneName != startMenuScene)
            ToggleGameMenu();
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
            PlayBGM(startBGM);
            score = 0;
            return;
        }

        // Save last score.
        lastScore = score;

        // Play BGM.
        if (audioSource.clip != gameBGM)
            PlayBGM(gameBGM);

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
    /// Sets the flame gauge amount in the UI.
    /// </summary>
    /// <param name="amount">Current flame gauge amount.</param>
    public void SetFlameGauge(int amount)
    {
        gameUiInstance.SetFlameGauge(amount);
    }

    /// <summary>
    /// Sets the flame gauge max in the UI.
    /// </summary>
    /// <param name="amount">Max flame gauge amount.</param>
    public void SetFlameGaugeMax(int amount)
    {
        gameUiInstance.SetFlameGaugeMax(amount);
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    /// <summary>
    /// Toggles game menu.
    /// </summary>
    public void ToggleGameMenu()
    {
        PlaySFX(gameMenuSound);
        
        // If game menu is opened already, destroy and resume game.
        if (gameMenuInstance != null)
        {
            Destroy(gameMenuInstance.gameObject);
            ResumeGame();
            return;
        }

        // Otherwise, pause and create game menu.
        PauseGame();
        GameObject gameMenuObject = Instantiate(gameMenu);
        gameMenuInstance = gameMenuObject.GetComponent<GameMenu>();
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

    /// <summary>
    /// Returns player to the start menu.
    /// </summary>
    public void BackToStart()
    {
        SceneManager.LoadScene(startMenuScene);
    }

    /// <summary>
    /// Play an audio clip one time.
    /// </summary>
    /// <param name="clip">Clip to play.</param>
    public void PlaySFX(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Play an audio clip for BGM.
    /// </summary>
    /// <param name="clip">Clip to play.</param>
    public void PlayBGM(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
