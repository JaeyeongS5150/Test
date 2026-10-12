using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("인벤토리")]
    [SerializeField] private List<InventorySlot> _inventory = new List<InventorySlot>();

    public IReadOnlyList<InventorySlot> Inventory => _inventory;

    public event Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public bool AddItem(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0)
        {
            return false;
        }


        foreach (InventorySlot slot in _inventory)
        {
            if (slot.ItemData == item)
            {
                slot.AddQuantity(amount);

                return true;
            }
        }

        _inventory.Add(new InventorySlot(item, amount));
        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool RemoveItem(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0)
        {
            return false;
        }

        for (int i = 0; i < _inventory.Count; i++)
        {
            InventorySlot slot = _inventory[i];

            if (slot.ItemData != item)
            {
                continue;
            }

            if (!slot.RemoveQuantity(amount))
            {
                return false;
            }

            if (slot.Quantity <= 0)
            {
                _inventory.RemoveAt(i);
            }

            OnInventoryChanged?.Invoke();
            return true;
        }

        return false;
    }

    public bool HasItem(ItemData item)
    {
        foreach (InventorySlot slot in _inventory)
        {
            if (slot.ItemData == item && slot.Quantity > 0)
            {
                return true;
            }
        }

        return false;
    }
}