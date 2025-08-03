using UnityEngine;

public class CastProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileLifetime = 5f;
    
    [Header("Input Handler")]
    [SerializeField] private PlayerInputHandler inputHandler;
    
    private Vector2 direction;
    private PlayerStats playerStats;
    
    void Start()
    {
        // If no fire point is assigned, use the player's position
        if (firePoint == null)
        {
            firePoint = transform;
        }

        playerStats = GetComponent<PlayerStats>();
        
        // Subscribe to input events
        if (inputHandler != null)
        {
            inputHandler.OnFirePressed.AddListener(OnFirePressed);
            Debug.Log("CastProjectile: Subscribed to PlayerInputHandler events");
        }
        else
        {
            Debug.LogError("CastProjectile: No PlayerInputHandler assigned!");
        }
    }
    
    void OnDestroy()
    {
        // Unsubscribe from events
        if (inputHandler != null)
        {
            inputHandler.OnFirePressed.RemoveListener(OnFirePressed);
        }
    }
    
    private void OnFirePressed(Vector2 aimDirection)
    {
        Debug.Log("CastProjectile: Received fire event with direction: " + aimDirection);
        direction = aimDirection;
        if (playerStats.mana >= projectilePrefab.GetComponent<Projectile>().manaCost)
        {
            playerStats.useMana(projectilePrefab.GetComponent<Projectile>().manaCost);
            FireProjectile();
        }
    }
    
    public void FireProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile prefab is not assigned!");
            return;
        }
        
        // Create spawn position with Z = 0 for 2D
        Vector3 spawnPosition = new Vector3(firePoint.position.x, firePoint.position.y, 0f);
        
        // Instantiate projectile at fire point
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        
        // Get the projectile component and set its direction and speed
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        if (projectileComponent != null)
        {
            projectileComponent.Initialize(direction, projectileSpeed, projectileLifetime);
        }
        else
        {
            // If no Projectile component, use Rigidbody2D
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
                Destroy(projectile, projectileLifetime);
            }
        }
        
        Debug.Log($"Fired projectile in direction: {direction}");
    }
    
    // Optional: Visualize the fire point in the editor
    void OnDrawGizmosSelected()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(firePoint.position, 0.1f);
        }
    }
}
