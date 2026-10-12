using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("아이템 정보")]
    [SerializeField] private int _itemID;
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _icon;
    [SerializeField, TextArea] private string _description;

    [Header("아이템 설정")]
    [SerializeField] private int _price;
    [SerializeField] private int _healAmount;   

    public int ItemID => _itemID;
    public string ItemName => _itemName;
    public Sprite Icon => _icon;
    public string Description => _description;
    public int Price => _price;
    public int HealAmount => _healAmount;
}