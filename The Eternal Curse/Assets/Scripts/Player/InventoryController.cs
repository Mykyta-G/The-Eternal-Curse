using UnityEngine;
using UnityEngine.InputSystem; 

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryPage inventoryUI;

    public int inventorySize = 10;

    private void Start()
    {
        inventoryUI.InitializeInventoryUI(inventorySize);
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Press I to toggle
        if (keyboard.iKey.wasPressedThisFrame)
        {
            if (inventoryUI.isActiveAndEnabled)
                inventoryUI.Hide();
            else
                inventoryUI.Show();
        }

        // Press Escape to close
        if (keyboard.escapeKey.wasPressedThisFrame && inventoryUI.isActiveAndEnabled)
        {
            inventoryUI.Hide();
        }
    }
}
