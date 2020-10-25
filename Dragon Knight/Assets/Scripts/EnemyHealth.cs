using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health
{
    public Slider healthSlider;

    public void OnHealthChanged(int currentHealth)
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
