using UnityEngine;

public class PlayerHealth : Health
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Get();
        OnHealthChanged();
    }

    public override void OnHealthChanged()
    {
        gameManager.SetHealth(currentHealth);
    }

    public override void OnDie()
    {
        // TODO: Play animation here.
        gameManager.GameOver();
    }
}
