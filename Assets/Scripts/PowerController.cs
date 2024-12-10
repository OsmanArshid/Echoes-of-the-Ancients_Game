using UnityEngine;

public class PowerController : MonoBehaviour
{
    public GameObject fireBulletPrefab;   // Prefab for Fire Power
    public Transform firePoint;          // Point where bullets spawn
    public float bulletSpeed = 10f;      // Speed of the bullets
    public float fireCooldown = 0.5f;      // Cooldown time between shots in seconds

    private float lastFireTime = -Mathf.Infinity;  // Track the last time the power was fired

    void Update()
    {
        // Check if the cooldown time has passed and the player presses the fire key
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= lastFireTime + fireCooldown)
        {
            Shoot(fireBulletPrefab);
            lastFireTime = Time.time;  // Update the last fire time to the current time
        }
    }

    void Shoot(GameObject bulletPrefab)
    {
        // Instantiate the bullet at the fire point with the same rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Ensure the bullet moves in the fire point's forward direction
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Vector3 bulletDirection = firePoint.forward.normalized; // Get the firePoint's forward direction
        rb.velocity = bulletDirection * bulletSpeed;            // Apply velocity to the bullet

        // Destroy the bullet after 5 seconds to prevent clutter
        Destroy(bullet, 5f);
    }
}
