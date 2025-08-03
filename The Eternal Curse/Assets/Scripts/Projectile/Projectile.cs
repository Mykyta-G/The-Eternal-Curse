using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Properties")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private LayerMask targetLayers = -1;
    
    [Header("Visual Effects")]
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private bool destroyOnHit = true;
    
    private Vector2 direction;
    private float currentLifetime;
    private Rigidbody2D rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        
        // Set up Rigidbody2D for 2D physics
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    
    public void Initialize(Vector2 dir, float projectileSpeed, float projectileLifetime)
    {
        direction = dir.normalized;
        speed = projectileSpeed;
        lifetime = projectileLifetime;
        currentLifetime = 0f;
        
        // Set velocity
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
        
        // Rotate projectile to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    void Update()
    {
        // Update lifetime
        currentLifetime += Time.deltaTime;
        if (currentLifetime >= lifetime)
        {
            DestroyProjectile();
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is on target layers
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {            
            // Spawn hit effect
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
            
            // Destroy projectile
            if (destroyOnHit)
            {
                DestroyProjectile();
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision with walls/obstacles
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        
        if (destroyOnHit)
        {
            DestroyProjectile();
        }
    }
    
    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
    
    // Optional: Visualize projectile path in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, direction * 2f);
    }
} 