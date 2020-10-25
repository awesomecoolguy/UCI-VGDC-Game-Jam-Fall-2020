using System.Collections;
using UnityEngine;

public class PlayerHealth : Health
{
    public float invulnerabilityDuration = 1.0f;
    public float invulnerabilityBlink = 0.10f;

    private SpriteRenderer[] spriteRenderers;
    private GameManager gameManager;

    private void Start()
    {
        // Get references.
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        gameManager = GameManager.Get();

        // Initialize health.
        OnHealthChanged();
    }

    private IEnumerator BecomeInvulnerable()
    {
        damageable = false;
        bool shouldBeInvisible = true;

        for (float i = 0; i < invulnerabilityDuration; i += invulnerabilityBlink)
        {
            foreach (var spriteRenderer in spriteRenderers)
            {
                if (shouldBeInvisible)
                    spriteRenderer.color = NewColorAlpha(spriteRenderer.color, 0);
                else
                    spriteRenderer.color = NewColorAlpha(spriteRenderer.color, 255);
            }
            yield return new WaitForSeconds(invulnerabilityBlink);
            shouldBeInvisible = !shouldBeInvisible;
        }

        // Return to original values.
        foreach (var spriteRenderer in spriteRenderers)
            spriteRenderer.color = NewColorAlpha(spriteRenderer.color, 255);
        damageable = true;
    }

    private Color NewColorAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    public override void OnDamaged()
    {
        base.OnDamaged();
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
