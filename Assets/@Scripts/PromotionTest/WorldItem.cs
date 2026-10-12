using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [Header("아이템 설정")]
    [SerializeField] private ItemData _itemData;
    [SerializeField] private int _amount = 1;

    private bool _isCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (_isCollected || GameManager.Instance == null || !GameManager.Instance.IsPlaying)
        {
            return;
        }

        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        if (InventoryManager.Instance == null)
        {
            return;
        }

        if (InventoryManager.Instance.AddItem(_itemData, _amount))
        {
            _isCollected = true;
            Destroy(gameObject);
        }
    }
}
