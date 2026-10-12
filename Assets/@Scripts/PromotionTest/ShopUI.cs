using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [Header("상점 UI")]
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private TMP_Text _shopGoldText;
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _itemPriceText;
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _closeButton;

    [Header("게임 UI")]
    [SerializeField] private TMP_Text _gameGoldText;

    private ItemData _selectedItem;

    private void Start()
    {
        CurrencyManager.Instance.OnGoldChanged += UpdateGoldUI;

        _buyButton.onClick.AddListener(BuyItem);
        _closeButton.onClick.AddListener(CloseShop);

        _shopPanel.SetActive(false);

        if (ShopManager.Instance.ShopItems.Length > 0)
        {
            _selectedItem = ShopManager.Instance.ShopItems[0];
        }

        UpdateItemUI();
        UpdateGoldUI();
    }

    private void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnGoldChanged -= UpdateGoldUI;
        }
    }

    private void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_shopPanel.activeSelf)
            {
                CloseShop();
            }

            else
            {
                OpenShop();
            }
        }
    }

    public void OpenShop()
    {
        if (!GameManager.Instance.IsPlaying)
        {
            return;
        }

        _shopPanel.SetActive(true);
        _messageText.text = "";

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UpdateGoldUI();
    }

    public void CloseShop()
    {
        _shopPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UpdateItemUI()
    {
        if (_selectedItem == null)
        {
            _itemNameText.text = "No Item";
            _itemPriceText.text = "";
            _buyButton.interactable = false;

            return;
        }

        _itemNameText.text = _selectedItem.ItemName;
        _itemPriceText.text = _selectedItem.Price + " Gold";
        _buyButton.interactable = true;
    }

    private void UpdateGoldUI()
    {
        int gold = CurrencyManager.Instance.Gold;

        _gameGoldText.text = "Gold : " + gold;
        _shopGoldText.text = "Gold : " + gold;
    }

    private void BuyItem()
    {
        if (_selectedItem == null)
        {
            return;
        }

        if (ShopManager.Instance.BuyItem(_selectedItem))
        {
            _messageText.text = "Purchase completed!";
        }
        else
        {
            _messageText.text = "Purchase failed! Please check your gold.";
        }
    }
}
