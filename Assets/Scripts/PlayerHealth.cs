using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    [HideInInspector] public float currentHealth;

    // UI Elements
    public TextMeshProUGUI healthText;        // Reference to the health percentage TextMeshPro object
    public GameObject gameOverPanel;          // Reference to the Game Over panel
    public float dragonDamage = 20f;          // Damage inflicted by the dragon per collision

    // Animator to control death animation
    private Animator animator;                // Reference to the player's animator

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI(); // Initialize the UI

        // Ensure the Game Over panel is hidden initially
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        UpdateHealthUI(); // Update the UI whenever health changes

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        // Update the health text percentage
        if (healthText != null)
        {
            healthText.text = $"Health: {(currentHealth / maxHealth) * 100:F1}%"; // e.g., "75.0%"
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");

        // Show the Game Over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Activate the panel
        }

        // Trigger the death animation
        if (animator != null)
        {
            animator.SetTrigger("DieTrigger"); // Trigger the death animation
        }

        // Wait for the animation to finish and then destroy the player (with an additional 2 seconds)
        StartCoroutine(DestroyPlayerAfterAnimation());
    }

    private IEnumerator DestroyPlayerAfterAnimation()
    {
        // Wait for the death animation to finish (using the animation length)
        if (animator != null)
        {
            float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animationLength + 2f); // Add 2 seconds after animation completes
        }

        // Destroy the player object after the animation and delay
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is the dragon
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(dragonDamage); // Apply damage
        }
    }
}
