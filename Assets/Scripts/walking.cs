using UnityEngine;

public class WalkingSound : MonoBehaviour
{
    public AudioClip walkingSound; // Assign the walking sound effect in the Inspector
    private AudioSource audioSource;
    private CharacterController characterController;

    void Start()
    {
        // Get the AudioSource component attached to the character
        audioSource = GetComponent<AudioSource>();
        // Get the CharacterController or Rigidbody (depending on your character setup)
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if the player is moving
        if (IsMoving() && !audioSource.isPlaying)
        {
            audioSource.clip = walkingSound;
            audioSource.Play();
        }

        // Stop playing sound if not moving
        if (!IsMoving() && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private bool IsMoving()
    {
        // Check character movement using velocity or input
        if (characterController != null)
        {
            return characterController.velocity.magnitude > 0.1f;
        }

        // Alternatively, for Rigidbody-based movement:
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            return rb.velocity.magnitude > 0.1f;
        }

        return false;
    }
}
