using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private int _quantity;

    public ItemData ItemData => _itemData;
    public int Quantity => _quantity;

    public InventorySlot(ItemData itemData, int quantity)
    {
        _itemData = itemData;
        _quantity = quantity;
    }

    public void AddQuantity(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        _quantity += amount;
    }

    public bool RemoveQuantity(int amount)
    {
        if (amount <= 0 || _quantity < amount)
        {
            return false;
        }

        _quantity -= amount;

        return true;
    }
}