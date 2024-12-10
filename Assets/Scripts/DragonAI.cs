using UnityEngine;

public class DragonAI : MonoBehaviour
{
    public Transform player; // Assign the player's Transform in the Inspector
    public float flyHeight = 10f; // Height at which the dragon flies
    public float speed = 5f; // Speed of the dragon when moving toward the player
    public float detectionRadius = 20f; // Radius within which the player is detected

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        FlyAtHeight(); // Maintain the dragon's flying height

        if (PlayerDetected())
        {
            MoveTowardsPlayer(); // Move toward the player when detected
        }
    }

    void FlyAtHeight()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, flyHeight, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
    }

    bool PlayerDetected()
    {
        if (player == null) return false;

        float distance = Vector3.Distance(transform.position, player.position);
        return distance <= detectionRadius;
    }

    void MoveTowardsPlayer()
    {
        Vector3 targetPosition = new Vector3(player.position.x, flyHeight, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Optionally rotate towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
    }
}
