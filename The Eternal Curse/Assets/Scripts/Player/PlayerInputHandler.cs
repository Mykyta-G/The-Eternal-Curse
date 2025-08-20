using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference fireAction;
    [SerializeField] private InputActionReference aimAction;
    [SerializeField] private InputActionReference inventoryAction;
    
    [Header("Inventory UI")]
    [SerializeField] private GameObject inventoryUI; // Assign your inventory root/panel here
    [SerializeField] private bool pauseWhileInventoryOpen = false;
    [SerializeField] private bool useCanvasGroupToggle = true; // Safer: does not deactivate GameObject
    
    [Header("Aiming")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float controllerDeadzone = 0.1f;
    
    [Header("Events")]
    public UnityEvent<Vector2> OnFirePressed;
    public UnityEvent<Vector2> OnAimChanged;
    public UnityEvent<Vector2> OnInventoryPressed;
    private Vector2 aimDirection;
    private bool isAiming = false;
    
    void Awake()
    {
        // Setup camera
        if (playerCamera == null) playerCamera = Camera.main;
        // Sync inventory state with current UI active state
        if (inventoryUI != null)
        {
            inventoryOpen = inventoryUI.activeSelf;
        }
    }
    
    void OnEnable()
    {
        // Enable all input actions
        if (fireAction != null) fireAction.action.Enable();
        if (aimAction != null) aimAction.action.Enable();

        var playerInput = GetComponent<PlayerInput>();
        bool usingSendMessages = playerInput != null && playerInput.notificationBehavior == PlayerNotifications.SendMessages;
        if (!usingSendMessages && inventoryAction != null)
        {
            inventoryAction.action.Enable();
        }
        
        // Subscribe to events
        if (fireAction != null) fireAction.action.performed += OnFire;
        if (!usingSendMessages && inventoryAction != null) inventoryAction.action.performed += OnInventory;
        
        Debug.Log("PlayerInputHandler enabled - Input actions should be active");
    }
    
    void OnDisable()
    {
        // Unsubscribe from events
        if (fireAction != null) fireAction.action.performed -= OnFire;

        var playerInput = GetComponent<PlayerInput>();
        bool usingSendMessages = playerInput != null && playerInput.notificationBehavior == PlayerNotifications.SendMessages;
        if (!usingSendMessages && inventoryAction != null) inventoryAction.action.performed -= OnInventory;
        
        // Disable all input actions
        if (fireAction != null) fireAction.action.Disable();
        if (aimAction != null) aimAction.action.Disable();
        if (!usingSendMessages && inventoryAction != null) inventoryAction.action.Disable();
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
    
    private bool inventoryOpen = false;
    
    // Called when subscribing directly to InputAction callbacks
    private void OnInventory(InputAction.CallbackContext context)
    {
        ToggleInventory();
    }
    
    // Called by PlayerInput when Behavior = Send Messages (InputValue variant)
    public void OnInventory(InputValue value)
    {
        // Toggle only on press, ignore release to avoid double toggles
        if (value.isPressed)
        {
            ToggleInventory();
        }
    }
    
    private void ToggleInventory()
    {
        var playerInput = GetComponent<PlayerInput>();
        string currentMap = (playerInput != null && playerInput.currentActionMap != null) ? playerInput.currentActionMap.name : "(none)";
        string nextState = inventoryOpen ? "Close" : "Open";
        Debug.Log($"Inventory input received. CurrentActionMap={currentMap}. Next: {nextState} inventory.");
        
        inventoryOpen = !inventoryOpen;
        if (inventoryUI != null)
        {
            bool wouldDisableSelf = inventoryUI == gameObject || transform.IsChildOf(inventoryUI.transform);
            if (useCanvasGroupToggle || wouldDisableSelf)
            {
                var canvasGroup = inventoryUI.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = inventoryUI.AddComponent<CanvasGroup>();
                }
                // Ensure object is active when opening so CanvasGroup can show it
                if (inventoryOpen && !inventoryUI.activeSelf)
                {
                    inventoryUI.SetActive(true);
                }
                canvasGroup.alpha = inventoryOpen ? 1f : 0f;
                canvasGroup.interactable = inventoryOpen;
                canvasGroup.blocksRaycasts = inventoryOpen;
            }
            else
            {
                inventoryUI.SetActive(inventoryOpen);
            }
        }
        if (pauseWhileInventoryOpen)
        {
            Time.timeScale = inventoryOpen ? 0f : 1f;
        }
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