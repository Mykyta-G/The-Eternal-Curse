using UnityEngine;
using System.Collections.Generic;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] 
    private InventoryItem itemPrefab;

    [SerializeField]
    private RectTransform contentPanel;

    List<InventoryItem> listOfItems = new List<InventoryItem>();

    void Start(){

    }

    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            InventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            listOfItems.Add(uiItem);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
