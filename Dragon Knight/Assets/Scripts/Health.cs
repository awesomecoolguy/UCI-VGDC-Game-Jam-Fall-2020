using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public int maxHealth = 3;
    protected int currentHealth;
    public bool damageable = true;
<<<<<<< Updated upstream
=======
    public List<AudioClip> damagedSounds;

    private System.Random rng = new System.Random();
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
    public virtual void OnDamaged() {}
=======
    public virtual void OnDamaged()
    {
        if (damagedSounds.Count > 0)
        {
            int index = rng.Next(0, damagedSounds.Count);
            AudioSource.PlayClipAtPoint(damagedSounds[index], transform.position);
        }
    }
>>>>>>> Stashed changes

    /// <summary>
    /// Triggers when the health amount changes.
    /// </summary>
    public virtual void OnHealthChanged() {}

    /// <summary>
    /// Triggers when the health amount is 0.
    /// </summary>
    public abstract void OnDie();
}
