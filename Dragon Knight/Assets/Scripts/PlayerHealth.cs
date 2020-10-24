using System.Collections;
using UnityEngine;

public class PlayerHealth : Health
{
    public float invulnerabilityDuration = 1.0f;
    public float invulnerabilityBlink = 0.10f;

    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;
    private Color originalColor;

    private void Start()
    {
        // Get references.
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameManager.Get();
        originalColor = spriteRenderer.color;

        // Initialzie health.
        OnHealthChanged();
    }

    private IEnumerator BecomeInvulnerable()
    {
        damageable = false;

        for (float i = 0; i < invulnerabilityDuration; i += invulnerabilityBlink)
        {
            if (spriteRenderer.color == originalColor)
                spriteRenderer.color = new Color(0, 0, 0, 0);
            else
                spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(invulnerabilityBlink);
        }

        spriteRenderer.color = originalColor;
        damageable = true;
    }

    public override void OnDamaged()
    {
        StartCoroutine(BecomeInvulnerable());
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
