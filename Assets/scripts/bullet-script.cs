using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    public GameObject impactEffect; // Optional impact particle effect

    private Transform target;
    private float damage;

    // Called by Tower script to set the target
    void SetTarget(Transform _target)
    {
        target = _target;
    }

    // Called by Tower script to set the damage
    void SetDamage(float _damage)
    {
        damage = _damage;
    }

    void Update()
    {
        // If target is destroyed or doesn't exist, destroy bullet
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Calculate direction to target
        Vector2 direction = (target.position - transform.position).normalized;

        // Move bullet towards target
        float distanceThisFrame = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target.position, distanceThisFrame);

        // Rotate bullet to face movement direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Check if we've reached the target (close enough for hit detection)
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        if (distanceToTarget < 0.1f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        // Spawn impact effect if assigned
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        // Try to deal damage to the enemy using SendMessage
        // This will work once you create an Enemy script with a TakeDamage method
        if (target != null)
        {
            target.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        }

        // Destroy the bullet
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Backup collision detection in case the bullet somehow passes through
        if (collision.CompareTag("Enemy") && collision.transform == target)
        {
            HitTarget();
        }
    }
}