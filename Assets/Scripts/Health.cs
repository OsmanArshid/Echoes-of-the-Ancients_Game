using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health
    private float currentHealth;

    [Header("Damage Modifier")]
    public float damageMultiplier = 1f; // Multiplier to adjust damage taken (default 1x)

    private void Start()
    {
        currentHealth = maxHealth; // Initialize health
    }

    public void TakeDamage(float damage)
    {
        float adjustedDamage = damage * damageMultiplier; // Adjust damage using the multiplier
        currentHealth -= adjustedDamage; // Reduce health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health stays within range

        Debug.Log($"{gameObject.name} took {adjustedDamage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die(); // Call death logic when health is zero
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        Destroy(gameObject); // Destroy the object (enemy or player) when health reaches zero
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth; // Useful for displaying health bars
    }
}
