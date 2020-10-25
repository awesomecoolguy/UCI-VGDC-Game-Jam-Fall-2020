using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDeterminer : MonoBehaviour
{
    private int currentBuildIndex;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Get();
        currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            int nextIndex = currentBuildIndex + 1;
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(nextIndex);
            else
                gameManager.BackToStart();
        }
    }

}
