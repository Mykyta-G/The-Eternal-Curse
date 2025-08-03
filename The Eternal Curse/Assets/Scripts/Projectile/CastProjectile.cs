using UnityEngine;

public class CastProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileLifetime = 5f;
    
    [Header("Input Settings")]
    [SerializeField] private KeyCode fireKey = KeyCode.F;
    
    [Header("Aiming")]
    [SerializeField] private Camera playerCamera;
    
    private Vector2 mousePosition;
    private Vector2 direction;
    
    void Start()
    {
        // If no camera is assigned, try to find the main camera
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
        
        // If no fire point is assigned, use the player's position
        if (firePoint == null)
        {
            firePoint = transform;
        }
    }

    void Update()
    {
        // Get mouse position in world coordinates
        mousePosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        
        // Calculate direction from player to mouse
        direction = (mousePosition - (Vector2)transform.position).normalized;
        
        // Fire projectile when F is pressed
        if (Input.GetKeyDown(fireKey))
        {
            FireProjectile();
        }
    }
    
    void FireProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile prefab is not assigned!");
            return;
        }
        
        // Instantiate projectile at fire point
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        
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
                
                // Destroy projectile after lifetime
                Destroy(projectile, projectileLifetime);
            }
        }
    }
    
    // Optional: Visualize the aim direction in the editor
    void OnDrawGizmosSelected()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(firePoint.position, 0.1f);
            
            // Draw aim direction
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(firePoint.position, direction * 2f);
        }
    }
}
