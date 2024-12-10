using UnityEngine;

public class GolemIdol : MonoBehaviour
{
    public Transform player;             // Reference to the player's transform
    public float detectionRadius = 3f;   // Distance at which the Golem detects the player
    public float attackDistance = 1f;    // Distance at which Golem should attack the player
    public float moveSpeed = 3f;         // Speed at which the Golem moves

    private Animator animator;           // Reference to the Golem's animator
    private PlayerHealth playerHealth;   // Reference to the PlayerHealth script

    private static readonly int IdleHash = Animator.StringToHash("Standing");
    private static readonly int WalkHash = Animator.StringToHash("Walking");
    private static readonly int AttackHash = Animator.StringToHash("Attack");

    void Start()
    {
        // Initialize the animator
        animator = GetComponent<Animator>();
        animator.speed = 0.5f;

        // Get the PlayerHealth component from the player object
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        // If the player is dead, transition to idle animation and stop movement
        if (playerHealth.currentHealth <= 0)
        {
            Idle();  // Transition to idle state if player is dead
            return;  // Stop further logic execution
        }

        // Always face the player
        FaceTowardsPlayer();

        // Check the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Player is within detection radius, start moving towards the player
            MoveTowardsPlayer();

            if (distanceToPlayer <= attackDistance)
            {
                // Player is close enough to attack, trigger the attack animation
                AttackPlayer();
            }
        }
        else
        {
            // Player is outside detection radius, return to idle state
            Idle();
        }
    }

    void MoveTowardsPlayer()
    {
        // Move the Golem towards the player using Vector3.MoveTowards
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Play walking animation directly
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Standing"))
        {
            animator.Play(WalkHash);
        }
    }

    void AttackPlayer()
    {
        // Stop moving (optional: you can add a slight pause before attacking)
        transform.position = transform.position;  // Stay in place for attack

        // Play the attack animation
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.Play(AttackHash);
        }

        // Optionally, deal damage to the player here
        // DealDamageToPlayer();
    }

    void Idle()
    {
        // Play idle animation directly
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Standing"))
        {
            animator.Play(IdleHash);
        }

        // Optionally, stop the Golem's movement if desired
        // transform.position = transform.position; // Keep the Golem in place
    }

    void FaceTowardsPlayer()
    {
        // Calculate the direction to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Calculate the rotation needed to face the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
    }
}
