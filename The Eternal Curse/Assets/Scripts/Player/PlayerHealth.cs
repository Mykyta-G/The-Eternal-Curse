using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthBar;
    public bool isInvincible = false; // Invincibility flag
    
    void Start()
    {
        maxHealth = health;
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    public void TakeDamage(float damage)
    {
        if (!isInvincible) // Only take damage if not invincible
        {
            health -= damage;
        }
    }
}
