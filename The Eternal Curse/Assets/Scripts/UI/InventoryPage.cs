using UnityEngine;
using System.Collections.Generic;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] 
    private InventoryItem itemPrefab;

    [SerializeField]
    private RectTransform contentPanel;
    
    [SerializeField]
    private DragPreview dragPreview;

    private List<InventoryItem> listOfItems = new List<InventoryItem>();
    private List<ItemData> inventoryData = new List<ItemData>();

    private InventoryItem draggedItem;
    private int draggedItemIndex = -1;
    private bool isDragging = false;

    private bool isVisible = false;

    public bool IsDragging => isDragging;

    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            inventoryData.Add(new ItemData("", null, 0));
        }

        for (int i = 0; i < inventorySize; i++)
        {
            InventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel, false); 
            listOfItems.Add(uiItem);

            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemDroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleItemEndDrag;
            uiItem.OnRightMouseBtnClick += HandleShowItemAction;
        }

        // Make sure inventory is hidden and state synced after initialization
        Hide();
    }

    private void HandleItemSelection(InventoryItem obj)
    {
        Debug.Log($"Selected item: {obj.name}");
    }

    private void HandleBeginDrag(InventoryItem obj)
    {
        int index = listOfItems.IndexOf(obj);
        if (index >= 0 && !inventoryData[index].IsEmpty)
        {
            draggedItem = obj;
            draggedItemIndex = index;
            isDragging = true;
            
            // Show drag preview
            if (dragPreview != null)
            {
                dragPreview.Show(inventoryData[index].itemSprite);
            }
            
            // Make the original item semi-transparent
            obj.GetComponent<CanvasGroup>().alpha = 0.5f;
        }
    }

    private void HandleSwap(InventoryItem targetItem)
    {
        if (draggedItem == null || draggedItemIndex == -1) return;
        
        int targetIndex = listOfItems.IndexOf(targetItem);
        if (targetIndex == -1) return;
        
        // Get the items
        ItemData draggedData = inventoryData[draggedItemIndex];
        ItemData targetData = inventoryData[targetIndex];
        
        // Handle stacking if items are the same
        if (!draggedData.IsEmpty && !targetData.IsEmpty && 
            draggedData.CanStackWith(targetData))
        {
            int totalQuantity = draggedData.quantity + targetData.quantity;
            int maxStack = draggedData.maxStackSize;
            
            if (totalQuantity <= maxStack)
            {
                // Full stack
                targetData.quantity = totalQuantity;
                draggedData = new ItemData("", null, 0);
            }
            else
            {
                // Partial stack
                targetData.quantity = maxStack;
                draggedData.quantity = totalQuantity - maxStack;
            }
        }
        else
        {
            // Swap items
            ItemData temp = draggedData;
            inventoryData[draggedItemIndex] = targetData;
            inventoryData[targetIndex] = temp;
        }
        
        // Update UI
        UpdateItemUI(draggedItemIndex);
        UpdateItemUI(targetIndex);
    }

    private void HandleItemEndDrag(InventoryItem obj)
    {
        if (draggedItem != null)
        {
            // Restore original item opacity
            draggedItem.GetComponent<CanvasGroup>().alpha = 1f;
            
            // Hide drag preview
            if (dragPreview != null)
            {
                dragPreview.Hide();
            }
            
            draggedItem = null;
            draggedItemIndex = -1;
            isDragging = false;
        }
    }

    private void HandleShowItemAction(InventoryItem obj)
    {
        int index = listOfItems.IndexOf(obj);
        if (index >= 0 && !inventoryData[index].IsEmpty)
        {
            Debug.Log($"Right-clicked on: {inventoryData[index].itemName}");
            // Add your item action logic here (use, drop, etc.)
        }
    }

    public void UpdateDragPreview(Vector2 screenPosition)
    {
        if (isDragging && dragPreview != null)
        {
            dragPreview.UpdatePosition(screenPosition);
        }
    }
    
    private void UpdateItemUI(int index)
    {
        if (index < 0 || index >= listOfItems.Count) return;
        
        ItemData data = inventoryData[index];
        InventoryItem uiItem = listOfItems[index];
        
        if (data.IsEmpty)
        {
            uiItem.ResetData();
        }
        else
        {
            uiItem.SetData(data.itemSprite, data.quantity);
        }
    }

    public bool AddItem(ItemData item)
    {
        // First try to stack with existing items
        for (int i = 0; i < inventoryData.Count; i++)
        {
            if (!inventoryData[i].IsEmpty && inventoryData[i].CanStackWith(item))
            {
                int spaceInStack = inventoryData[i].maxStackSize - inventoryData[i].quantity;
                if (spaceInStack > 0)
                {
                    int amountToAdd = Mathf.Min(spaceInStack, item.quantity);
                    inventoryData[i].quantity += amountToAdd;
                    item.quantity -= amountToAdd;
                    
                    UpdateItemUI(i);
                    
                    if (item.quantity <= 0) return true;
                }
            }
        }
        
        // Find empty slot
        for (int i = 0; i < inventoryData.Count; i++)
        {
            if (inventoryData[i].IsEmpty)
            {
                inventoryData[i] = new ItemData(item.itemName, item.itemSprite, item.quantity);
                UpdateItemUI(i);
                return true;
            }
        }
        
        return false; // Inventory full
    }
    
    // Method to remove items from inventory
    public bool RemoveItem(int index, int quantity = 1)
    {
        if (index < 0 || index >= inventoryData.Count || inventoryData[index].IsEmpty)
            return false;
            
        if (inventoryData[index].quantity <= quantity)
        {
            inventoryData[index] = new ItemData("", null, 0);
        }
        else
        {
            inventoryData[index].quantity -= quantity;
        }
        
        UpdateItemUI(index);
        return true;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        isVisible = true;
        Debug.Log("Inventory shown, isVisible = " + isVisible);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        isVisible = false;
        Debug.Log("Inventory hidden, isVisible = " + isVisible);
    }

    public void ToggleInventory()
    {
        if (isVisible)
            Hide();
        else
            Show();
    }
}
