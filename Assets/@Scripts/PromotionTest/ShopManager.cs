using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [SerializeField] private ItemData[] _shopItems;

    public ItemData[] ShopItems => _shopItems;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

            return;
        }

        Instance = this;
    }

    public bool BuyItem(ItemData item)
    {
        if (item == null)
        {
            return false;
        }

        if (CurrencyManager.Instance == null || InventoryManager.Instance == null)
        {
            return false;
        }

        if (item.Price < 0)
        {
            return false;
        }

        bool isSelling = false;

        foreach (ItemData shopItem in _shopItems)
        {
            if (shopItem == item)
            {
                isSelling = true;

                break;
            }
        }

        if (!isSelling)
        {
            return false;
        }

        if (CurrencyManager.Instance.Gold < item.Price)
        {
            Debug.Log("골드가 부족합니다.");

            return false;
        }

        if (!CurrencyManager.Instance.SpendGold(item.Price))
        {
            return false;
        }

        if (!InventoryManager.Instance.AddItem(item, 1))
        {
            CurrencyManager.Instance.AddGold(item.Price);

            return false;
        }

        Debug.Log(item.ItemName + " 구매 완료!");

        return true;
    }
}
