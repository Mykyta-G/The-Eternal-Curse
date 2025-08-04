using UnityEngine;
using UnityEngine.InputSystem; 

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryPage inventoryUI;

    public int inventorySize = 10;

    public PlayerInputHandler inputHandler;

    private void Start()
    {
        inventoryUI.InitializeInventoryUI(inventorySize);

        if (inputHandler != null){
            inputHandler.OnInvPressed.AddListener(OnInvPressed);
        }
        else
        {
            Debug.LogError("CastProjectile: No PlayerInputHandler assigned!");
        }
    }

    private void Update()
    {

    }
}
