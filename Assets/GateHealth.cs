using TMPro;
using UnityEngine;

public class GateHealth2D : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int damagePerEnemy = 10;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;

    public int CurrentHealth { get; private set; }

    private void Start()
    {
        CurrentHealth = maxHealth;
        UpdateHealthText();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(damagePerEnemy);
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        // TEMP TEST (remove later)
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(damagePerEnemy);
        }
    }

    private void TakeDamage(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, maxHealth);
        UpdateHealthText();

        Debug.Log("Gate Health: " + CurrentHealth);

        if (CurrentHealth <= 0)
        {
            GateDestroyed();
        }
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + CurrentHealth;
        }
    }

    private void GateDestroyed()
    {
        Debug.Log("GAME OVER");
        // Stop spawns, show UI, etc.
    }

}