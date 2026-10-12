using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private Transform _slotContainer;
    [SerializeField] private InventorySlotUI _slotPrefab;
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _closeButton;

    private ItemData _selectedItem;

    private readonly List<InventorySlotUI> _slots = new List<InventorySlotUI>();

    private void Start()
    {
        InventoryManager.Instance.OnInventoryChanged += RefreshUI;

        _useButton.onClick.AddListener(UseSelectedItem);
        _closeButton.onClick.AddListener(CloseInventory);

        _inventoryPanel.SetActive(false);

        RefreshUI();
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= RefreshUI;
        }

    }

    private void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_inventoryPanel.activeSelf)
            {
                CloseInventory();
            }

            else
            {
                OpenInventory();
            }
        }
    }

    public void OpenInventory()
    {
        if (!GameManager.Instance.IsPlaying)
        { 
            return; 
        }

        _inventoryPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        RefreshUI();
    }

    public void CloseInventory()
    {
        _inventoryPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void RefreshUI()
    {
        foreach (InventorySlotUI slot in _slots)
        {
            if (slot != null) Destroy(slot.gameObject);
        }

        _slots.Clear();

        foreach (InventorySlot slot in InventoryManager.Instance.Inventory)
        {
            InventorySlotUI slotUI = Instantiate(_slotPrefab, _slotContainer);
            slotUI.Initialize(slot, this);
            _slots.Add(slotUI);
        }

        if (_selectedItem != null && !InventoryManager.Instance.HasItem(_selectedItem))
        {
            _selectedItem = null;
        }

        UpdateSelectionUI();
    }

    public void SelectItem(ItemData item)
    {
        _selectedItem = item;
        UpdateSelectionUI();
    }

    private void UpdateSelectionUI()
    {

        if (_selectedItem == null)
        {
            _itemNameText.text = "Select Item";
            _descriptionText.text = "";
            _useButton.interactable = false;

            return;
        }

        _itemNameText.text = _selectedItem.ItemName;
        _descriptionText.text = _selectedItem.Description;
        _useButton.interactable = true;
    }

    private void UseSelectedItem()
    {
        if (_selectedItem == null || !InventoryManager.Instance.HasItem(_selectedItem))
        {
            return;
        }

        if (!GameManager.Instance.IsPlaying)
        {
            return;
        }

        ItemData usedItem = _selectedItem;

        if (usedItem.HealAmount <= 0)
        {
            Debug.Log("사용할 수 없는 아이템입니다.");

            return;
        }

        GameManager.Instance.PlayerHeal(usedItem.HealAmount);

        InventoryManager.Instance.RemoveItem(usedItem, 1);

        Debug.Log(usedItem.ItemName + " Used");
    }
}