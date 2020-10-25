using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDeterminer : MonoBehaviour
{
    private int currentBuildIndex = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var other = collision.gameObject.GetComponent<PlayerController>();
        if(other != null)
        {
            currentBuildIndex += 1;
            SceneManager.LoadScene(currentBuildIndex);
        }
    }

}
