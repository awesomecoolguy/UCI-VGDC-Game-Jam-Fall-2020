using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health
{
    public Slider healthSlider;

    public override void OnHealthChanged()
    {
        healthSlider.value = currentHealth / maxHealth;
    }

    public override void OnDie()
    {
        // TODO: Play animation here.
        // TODO: Play dying sound here.
        Destroy(gameObject);
    }
}
