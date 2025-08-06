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
        inventoryUI.InitializeInventoryUI(inventorySize);

        inventoryUI.Hide();

        AddTestItem();
        
        // Add listener for inventory input
        if (inputHandler != null)
        {
            inputHandler.OnInvPressed.AddListener(OnInvPressed);
        }
        else
        {
            Debug.LogError("InventoryController: No PlayerInputHandler assigned!");
        }
    }

    private void Update()
    {
        // Update drag preview position if dragging
        if (inventoryUI.IsDragging)
        {
            // Use Mouse.current.position for the new Input System
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            inventoryUI.UpdateDragPreview(mousePosition);
        }
    }

    private void OnInvPressed(Vector2 _)
    {
        Debug.Log("Inventory input received, toggling inventory.");
        inventoryUI.ToggleInventory();
    }

    public void AddTestItem()
    {
        // Create a test item (you'll need to assign sprites in the inspector)
        ItemData testItem = new ItemData("Test Item", null, 5);
        inventoryUI.AddItem(testItem);
        Debug.Log("Test item added to inventory.");
    }
}
