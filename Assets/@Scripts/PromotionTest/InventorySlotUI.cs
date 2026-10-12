using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _quantityText;
    [SerializeField] private Button _selectButton;
    [SerializeField] private GameObject _selectedFrame;

    private InventorySlot _slot;
    private InventoryUI _inventoryUI;

    public void Initialize(InventorySlot slot, InventoryUI inventoryUI)
    {
        _slot = slot;
        _inventoryUI = inventoryUI;

        _itemIcon.sprite = slot.ItemData.Icon;
        _itemIcon.enabled = slot.ItemData.Icon != null;
        _quantityText.text = slot.Quantity.ToString();

        _selectButton.onClick.RemoveAllListeners();
        _selectButton.onClick.AddListener(OnSelect);

        SetSelected(false);
    }

    private void OnSelect()
    {
        _inventoryUI.SelectItem(_slot.ItemData);
    }

    public void SetSelected(bool selected)
    {
        if (_selectedFrame != null)
        {
            _selectedFrame.SetActive(selected);
        }
    }
}
