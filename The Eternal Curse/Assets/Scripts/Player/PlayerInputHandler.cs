using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference fireAction;
    [SerializeField] private InputActionReference aimAction;
    [SerializeField] private InputActionReference inventoryAction;
    
    [Header("Aiming")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float controllerDeadzone = 0.1f;
    
    [Header("Events")]
    public UnityEvent<Vector2> OnFirePressed;
    public UnityEvent<Vector2> OnAimChanged;
    public UnityEvent<Vector2> OnInvPressed;  
    
    private Vector2 aimDirection;
    private bool isAiming = false;
    
    void Awake()
    {
        if (playerCamera == null) playerCamera = Camera.main;
    }
    
    void OnEnable()
    {
        if (fireAction != null) fireAction.action.Enable();
        if (aimAction != null) aimAction.action.Enable();
        if (inventoryAction != null) inventoryAction.action.Enable();
        
        if (fireAction != null) fireAction.action.performed += OnFire;
        if (inventoryAction != null) inventoryAction.action.performed += HandleInventory;
        
        Debug.Log("PlayerInputHandler enabled - Input actions should be active");
    }
    
    void OnDisable()
    {
        if (fireAction != null) fireAction.action.performed -= OnFire;
        if (inventoryAction != null) inventoryAction.action.performed -= HandleInventory;
        
        if (fireAction != null) fireAction.action.Disable();
        if (aimAction != null) aimAction.action.Disable();
        if (inventoryAction != null) inventoryAction.action.Disable();
    }
    
    void Update()
    {
        if (aimAction != null)
        {
            Vector2 input = aimAction.action.ReadValue<Vector2>();
            
            if (input.magnitude > 100f)
            {
                if (playerCamera != null)
                {
                    Vector2 worldMousePos = playerCamera.ScreenToWorldPoint(input);
                    aimDirection = (worldMousePos - (Vector2)transform.position).normalized;
                    isAiming = true;
                }
            }
            else if (input.magnitude > controllerDeadzone)
            {
                aimDirection = input.normalized;
                isAiming = true;
            }
        }

        if (!isAiming)
        {
            aimDirection = transform.right;
        }

        OnAimChanged?.Invoke(aimDirection);
    }
    
    private void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire button pressed! Aim direction: " + aimDirection);
        OnFirePressed?.Invoke(aimDirection);
    }

    public void OnInventory() { }

    private void HandleInventory(InputAction.CallbackContext context)
    {
        Debug.Log("Inventory button pressed!");
        OnInvPressed?.Invoke(aimDirection);
    }

    public Vector2 GetAimDirection()
    {
        return aimDirection;
    }

    public bool IsAiming()
    {
        return isAiming;
    }

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
