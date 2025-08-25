using UnityEngine;
using UnityEngine.Events;

public class SwordController : MonoBehaviour
{
    [Header("Sword Properties")]
    [SerializeField] private float swingDamage = 25f;
    [SerializeField] private float swingDuration = 0.3f;
    [SerializeField] private float swingRadius = 1.5f;
    [SerializeField] private float swingArc = 180f; // Full arc from start to end
    
    [Header("Collision")]
    [SerializeField] private LayerMask enemyLayers = -1;
    [SerializeField] private bool showDebugGizmos = true;
    
    [Header("Events")]
    public UnityEvent OnSwingStart;
    public UnityEvent OnSwingEnd;
    public UnityEvent OnHitEnemy;
    
    // Private variables
    private bool isSwinging = false;
    private float currentSwingTime = 0f;
    private Vector2 swingDirection;
    private float swingStartAngle;
    private Transform playerTransform;
    
    // Components
    private Collider2D swordCollider;
    private SpriteRenderer swordRenderer;
    
    void Awake()
    {
        // Get or add required components
        swordCollider = GetComponent<Collider2D>();
        if (swordCollider == null)
        {
            swordCollider = gameObject.AddComponent<BoxCollider2D>();
        }
        
        // Set up collider as trigger
        swordCollider.isTrigger = true;
        
        // Get sprite renderer to control visibility
        swordRenderer = GetComponent<SpriteRenderer>();
        
        // Find player transform
        playerTransform = transform.parent;
        if (playerTransform == null)
        {
            playerTransform = transform;
        }
        
        // Start with sword invisible
        SetSwordVisibility(false);
    }
    
    void Update()
    {
        // Update swing animation
        if (isSwinging)
        {
            UpdateSwingAnimation();
        }
    }
    
    public void StartSwing(Vector2 direction, Transform player)
    {
        swingDirection = direction.normalized;
        swingStartAngle = Mathf.Atan2(swingDirection.y, swingDirection.x) * Mathf.Rad2Deg;
        
        // Set the player transform for this swing
        playerTransform = player;
        
        isSwinging = true;
        currentSwingTime = 0f;
        
        // Enable collider during swing
        swordCollider.enabled = true;
        
        // Make sword visible during swing
        SetSwordVisibility(true);
        
        OnSwingStart?.Invoke();
        
        Debug.Log($"Sword swing started in direction: {swingDirection}");
    }
    
    private void UpdateSwingAnimation()
    {
        currentSwingTime += Time.deltaTime;
        float swingProgress = currentSwingTime / swingDuration;
        
        if (swingProgress >= 1f)
        {
            EndSwing();
            return;
        }
        
        // Calculate swing angle - goes from start to end in a smooth arc
        // The middle of the swing (0.5) will be at the cursor direction
        float currentAngle = swingStartAngle + (swingArc * swingProgress);
        Vector2 currentDirection = new Vector2(
            Mathf.Cos(currentAngle * Mathf.Deg2Rad),
            Mathf.Sin(currentAngle * Mathf.Deg2Rad)
        );
        
        // Update sword position - always relative to player center
        UpdateSwordPosition(currentDirection);
        
        // Update sword rotation to face swing direction
        float rotationAngle = currentAngle;
        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
    }
    
    private void UpdateSwordPosition(Vector2 direction)
    {
        if (playerTransform == null) return;
        
        // Position sword at the edge of the swing radius from player center
        Vector2 swordPosition = (Vector2)playerTransform.position + (direction * swingRadius);
        transform.position = swordPosition;
    }
    
    private void EndSwing()
    {
        isSwinging = false;
        swordCollider.enabled = false;
        
        // Make sword invisible before destroying
        SetSwordVisibility(false);
        
        // Destroy the sword GameObject after the swing
        Destroy(gameObject);
    }
    
    private void SetSwordVisibility(bool visible)
    {
        if (swordRenderer != null)
        {
            swordRenderer.enabled = visible;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isSwinging) return;
        
        // Check if we hit an enemy
        if (((1 << other.gameObject.layer) & enemyLayers) != 0)
        {
            // Deal damage
            var enemyHealth = other.GetComponent<EntityHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage((int)swingDamage);
                OnHitEnemy?.Invoke();
                
                Debug.Log($"Sword hit enemy for {swingDamage} damage!");
            }
        }
    }
    
    // Public getters
    public bool IsSwinging() => isSwinging;
    
    // Debug visualization
    void OnDrawGizmosSelected()
    {
        if (!showDebugGizmos) return;
        
        if (playerTransform != null)
        {
            // Draw swing radius
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(playerTransform.position, swingRadius);
            
            // Draw swing arc
            if (isSwinging)
            {
                Gizmos.color = Color.red;
                Vector3 center = playerTransform.position;
                Vector3 startDir = Quaternion.AngleAxis(swingStartAngle, Vector3.forward) * Vector3.right;
                
                for (int i = 0; i <= 20; i++)
                {
                    float angle = swingStartAngle + (swingArc * i / 20f);
                    Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
                    Vector3 pos = center + (dir * swingRadius);
                    
                    if (i > 0)
                    {
                        Gizmos.DrawLine(center + (startDir * swingRadius), pos);
                    }
                    startDir = dir;
                }
            }
        }
    }
}
