using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Health")]
    public GameObject healthBar;
    public GameObject healthPoint;
    public float healthPointGap = 5;

    private List<GameObject> healthPoints = new List<GameObject>();

    [Header("Score")]
    public Text scoreText;
    public int scoreWidth = 6;

    [Header("Flame Gauge")]
    public Slider flameGauge;

    private GameManager gameManager;

    /// <summary>
    /// Sets the number of health points in the UI.
    /// </summary>
    /// <param name="newHealth">New health value.</param>
    public void SetHealth(int newHealth)
    {
        int diff = newHealth - healthPoints.Count;
        for (int i = 0; i < Math.Abs(diff); i++)
        {
            if (diff > 0)
                AddHealth();
            else
                RemoveHealth();
        }
    }

    private void AddHealth()
    {
        int index = healthPoints.Count;
        GameObject newHealthPoint = Instantiate(healthPoint, healthBar.transform);
        var rectTransform = (RectTransform)newHealthPoint.transform;
        rectTransform.anchoredPosition = new Vector2
        (
            rectTransform.anchoredPosition.x + rectTransform.rect.width * index + healthPointGap * index,
            rectTransform.anchoredPosition.y
        );
        healthPoints.Add(newHealthPoint);
    }

    private void RemoveHealth()
    {
        int index = healthPoints.Count - 1;
        GameObject lastHealthPoint = healthPoints[index];
        healthPoints.RemoveAt(index);
        Destroy(lastHealthPoint);
    }

    /// <summary>
    /// Sets the score in the UI.
    /// </summary>
    /// <param name="newScore">New score value.</param>
    public void SetScore(int newScore)
    {
        scoreText.text = newScore.ToString().PadLeft(scoreWidth, '0');
    }

    /// <summary>
    /// Sets the flame gauge in the UI.
    /// </summary>
    /// <param name="newAmount">New flame gauge amount.</param>
    public void SetFlameGauge(int newAmount)
    {
        flameGauge.value = newAmount;
    }
}
