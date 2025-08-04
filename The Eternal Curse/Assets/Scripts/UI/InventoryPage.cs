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
        // Optional: Start hidden
        Hide();
    }

    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            InventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel, false); // false = keep local scale/pos
            listOfItems.Add(uiItem);
        }
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
