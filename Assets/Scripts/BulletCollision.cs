using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 20f; // Damage dealt by the bullet
    public GameObject particleEffectPrefab; // Assign your particle prefab in the Inspector
    public AudioClip collisionSound;       // Assign your sound effect in the Inspector
    private AudioSource audioSource;

    private void Start()
    {
        // Ensure there is an AudioSource component on the GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Prevents the sound from playing when the game starts
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Apply damage if the collided object has a Health component
        Debug.Log("Collision detected");
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage); // Apply damage
        }

        // Instantiate the particle effect at the collision point
        if (particleEffectPrefab != null)
        {
            Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
        }

        // Play the collision sound
        if (collisionSound != null)
        {
            audioSource.PlayOneShot(collisionSound);
        }

        // Destroy the bullet
        Destroy(gameObject);
    }
}
