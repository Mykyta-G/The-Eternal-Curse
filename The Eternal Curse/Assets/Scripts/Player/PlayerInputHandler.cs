using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference fireAction;
    [SerializeField] private InputActionReference aimAction;
    
    [Header("Aiming")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float controllerDeadzone = 0.1f;
    
    [Header("Events")]
    public UnityEvent<Vector2> OnFirePressed;
    public UnityEvent<Vector2> OnAimChanged;
    
    private Vector2 aimDirection;
    private bool isAiming = false;
    
    void Awake()
    {
        // Setup camera
        if (playerCamera == null) playerCamera = Camera.main;
    }
    
    void OnEnable()
    {
        // Enable all input actions
        if (fireAction != null) fireAction.action.Enable();
        if (aimAction != null) aimAction.action.Enable();
        
        // Subscribe to events
        if (fireAction != null) fireAction.action.performed += OnFire;
        
        Debug.Log("PlayerInputHandler enabled - Input actions should be active");
    }
    
    void OnDisable()
    {
        // Unsubscribe from events
        if (fireAction != null) fireAction.action.performed -= OnFire;
        
        // Disable all input actions
        if (fireAction != null) fireAction.action.Disable();
        if (aimAction != null) aimAction.action.Disable();
    }
    
    void Update()
    {
        // Handle aiming (both mouse and joystick)
        if (aimAction != null)
        {
            Vector2 input = aimAction.action.ReadValue<Vector2>();
            
            // Check if it's mouse input (large values) or joystick input (smaller values)
            if (input.magnitude > 100f) // Mouse input typically has large values
            {
                // Mouse input - convert screen position to world direction
                if (playerCamera != null)
                {
                    Vector2 worldMousePos = playerCamera.ScreenToWorldPoint(input);
                    aimDirection = (worldMousePos - (Vector2)transform.position).normalized;
                    isAiming = true;
                }
            }
            else if (input.magnitude > controllerDeadzone) // Joystick input
            {
                // Joystick input - use directly as direction
                aimDirection = input.normalized;
                isAiming = true;
            }
        }
        
        // If not aiming, use a default direction (forward)
        if (!isAiming)
        {
            aimDirection = transform.right; // Default to facing right
        }
        
        // Notify listeners of aim direction changes
        OnAimChanged?.Invoke(aimDirection);
    }
    
    private void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire button pressed! Aim direction: " + aimDirection);
        OnFirePressed?.Invoke(aimDirection);
    }
    
    // Public method to get current aim direction
    public Vector2 GetAimDirection()
    {
        return aimDirection;
    }
    
    // Public method to check if currently aiming
    public bool IsAiming()
    {
        return isAiming;
    }
    
    // Visualize aim direction in editor
    void OnDrawGizmosSelected()
    {
        if (isAiming)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.1f);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, aimDirection * 2f);
        }
    }
} 