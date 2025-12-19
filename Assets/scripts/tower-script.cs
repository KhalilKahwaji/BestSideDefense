using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    [Header("Tower Stats")]
    public float damage = 10f;
    public float attackSpeed = 1f; // Attacks per second
    public float range = 5f;
    public int price = 100; // Cost to build this tower

    [Header("References")]
    public Transform firePoint; // Where bullets spawn from
    public GameObject bulletPrefab; // Bullet object to instantiate (optional if using child)

    [Header("Debug")]
    public bool showRange = true;

    private Transform target;
    private float fireCountdown = 0f;
    private Vector3 buildPosition; // Position where tower was placed
    private GameObject bulletTemplate; // Reference to bullet child

    void Start()
    {
        // Store the position where tower was placed
        buildPosition = transform.position;

        // Snap to grid center if using tilemap placement
        // This ensures tower stays centered on the tile
        transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            transform.position.z
        );

        // Get bullet from first child if bulletPrefab is not assigned
        if (bulletPrefab == null && transform.childCount > 0)
        {
            bulletTemplate = transform.GetChild(0).gameObject;
            bulletTemplate.SetActive(false); // Make sure it's disabled
        }

        // Start looking for enemies
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Public method for tower placement system to set position
    public void SetBuildPosition(Vector3 position)
    {
        buildPosition = position;
        transform.position = position;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        // Find the closest enemy within range
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (target == null)
            return;

        // Check if target is still in range
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        if (distanceToTarget > range)
        {
            target = null;
            return;
        }

        // Rotate tower to face target
        RotateTowardsTarget();

        // Handle shooting
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / attackSpeed;
        }

        fireCountdown -= Time.deltaTime;
    }

    void RotateTowardsTarget()
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Adjust rotation based on your sprite orientation
        // This assumes the tower/gun faces right by default
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot()
    {
        GameObject bulletSource = bulletPrefab != null ? bulletPrefab : bulletTemplate;

        if (bulletSource == null)
        {
            Debug.LogWarning("No bullet assigned! Add bullet prefab or make bullet the first child of tower.");
            return;
        }

        // Spawn bullet at fire point
        Transform spawnPoint = firePoint != null ? firePoint : transform;
        GameObject bulletObj = Instantiate(bulletSource, spawnPoint.position, spawnPoint.rotation);
        bulletObj.SetActive(true); // Make sure instantiated bullet is active

        // Store target reference for the bullet to use later
        // The bullet script will need to access this when you create it
        bulletObj.SendMessage("SetTarget", target, SendMessageOptions.DontRequireReceiver);
        bulletObj.SendMessage("SetDamage", damage, SendMessageOptions.DontRequireReceiver);
    }

    void OnDrawGizmosSelected()
    {
        if (showRange)
        {
            // Draw range circle in editor
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}