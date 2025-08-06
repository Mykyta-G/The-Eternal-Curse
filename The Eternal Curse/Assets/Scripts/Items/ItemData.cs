using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string itemName;
    public Sprite itemSprite;
    public int quantity;
    public int maxStackSize = 99;
    
    public ItemData(string name, Sprite sprite, int qty = 1)
    {
        itemName = name;
        itemSprite = sprite;
        quantity = qty;
    }
    
    public bool IsEmpty => string.IsNullOrEmpty(itemName) || itemSprite == null;
    
    public bool CanStackWith(ItemData other)
    {
        return !IsEmpty && !other.IsEmpty && itemName == other.itemName;
    }
}