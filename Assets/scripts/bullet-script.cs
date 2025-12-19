using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    public GameObject impactEffect; // Optional impact particle effect

    private Transform target;
    private float damage;

    // Added: store initial direction for piercing bullets
    private Vector2 moveDirection;

    // Pierce value
    public int pierce = 0;

    // Called by Tower script to set the target
    void SetTarget(Transform _target)
    {
        target = _target;

        // Set move direction for piercing bullets
        if (target != null)
        {
            moveDirection = ((Vector2)target.position - (Vector2)transform.position).normalized;
        }
    }

    // Called by Tower script to set the damage
    void SetDamage(float _damage)
    {
        damage = _damage;
    }

    void Update()
    {
        // If target is destroyed or doesn't exist and pierce == 0, destroy bullet
        if (target == null && pierce == 0)
        {
            Destroy(gameObject);
            return;
        }

        // Determine direction
        Vector2 direction = target != null ? ((Vector2)target.position - (Vector2)transform.position).normalized : moveDirection;

        // Determine move destination
        Vector2 destination = target != null ? (Vector2)target.position : (Vector2)transform.position + direction;

        // Move bullet
        float distanceThisFrame = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, destination, distanceThisFrame);

        // Rotate bullet to face movement direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Check if we've reached the target
        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, (Vector2)target.position);
            if (distanceToTarget < 0.1f)
            {
                HitTarget();
            }
        }
    }

    void HitTarget()
    {
        // Spawn impact effect if assigned
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        // Deal damage to enemy
        if (target != null)
        {
            target.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        }

        // Only destroy the bullet if pierce is 0
        if (pierce == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            // If piercing, remove current target so it keeps moving
            target = null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Backup collision detection in case the bullet passes through
        if (collision.CompareTag("Enemy") && collision.transform == target)
        {
            HitTarget();
        }
    }
}
 