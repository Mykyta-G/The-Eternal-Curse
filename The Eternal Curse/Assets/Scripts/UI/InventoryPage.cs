using UnityEngine;
using System.Collections.Generic;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] 
    private InventoryItem itemPrefab;

    [SerializeField]
    private RectTransform contentPanel;

    private List<InventoryItem> listOfItems = new List<InventoryItem>();

    private bool isVisible = false;

    void Start()
    {
        Hide();
    }

    public void InitializeInventoryUI(int inventorySize)
    {
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
    }

    private void HandleItemSelection(InventoryItem obj)
    {
        Debug.Log(obj.name);
    }

    private void HandleBeginDrag(InventoryItem obj)
    {
        
    }

    private void HandleSwap(InventoryItem obj)
    {
        
    }

    private void HandleItemEndDrag(InventoryItem obj)
    {
        
    }

    private void HandleShowItemAction(InventoryItem obj)
    {
        
    }

    public void Show()
    {
        gameObject.SetActive(true);
        isVisible = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        isVisible = false;
    }

    public void ToggleInventory()
    {
        if (isVisible)
            Hide();
        else
            Show();
    }
}
