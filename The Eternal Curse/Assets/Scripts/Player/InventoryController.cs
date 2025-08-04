using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryPage inventoryUI;

    [SerializeField] public PlayerInputHandler inputHandler;

    [Header("Settings")]
    public int inventorySize = 10;

    private void Start()
    {
        // Initialize the inventory with empty slots
        inventoryUI.InitializeInventoryUI(inventorySize);

        // Hook into the inventory input event
        if (inputHandler != null)
        {
            inputHandler.OnInvPressed.AddListener(OnInvPressed);
        }
        else
        {
            Debug.LogError("InventoryController: No PlayerInputHandler assigned!");
        }
    }

    private void OnInvPressed(Vector2 _)
    {
        Debug.Log("Inventory input received, toggling inventory.");
        inventoryUI.ToggleInventory();
    }
}
