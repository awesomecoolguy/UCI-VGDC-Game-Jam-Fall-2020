using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public int maxHealth = 3;
    protected int currentHealth;
    public bool damageable = true;

    private void Awake()
    {
        currentHealth = maxHealth;    
    }

    /// <summary>
    /// Heals the target.
    /// </summary>
    /// <param name="amount">Heal amount. Default to 1.</param>
    public void Heal(int amount = 1)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        OnHealthChanged();
    }

    /// <summary>
    /// Damages the target.
    /// </summary>
    /// <param name="amount">Damage amount. Default to 1.</param>
    public void Damage(int amount = 1)
    {
        if (!damageable)
            return;

        currentHealth -= amount;
        if (currentHealth < 0)
            currentHealth = 0;
        OnDamaged();
        OnHealthChanged();
        if (currentHealth == 0)
            OnDie();
    }

    /// <summary>
    /// Triggers when the target is damaged.
    /// </summary>
    public virtual void OnDamaged() {}

    /// <summary>
    /// Triggers when the health amount changes.
    /// </summary>
    public virtual void OnHealthChanged() {}

    /// <summary>
    /// Triggers when the health amount is 0.
    /// </summary>
    public abstract void OnDie();
}
