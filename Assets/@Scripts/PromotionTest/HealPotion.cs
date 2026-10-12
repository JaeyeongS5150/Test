using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPotion : MonoBehaviour
{
    [Header("물약 설정")]
    [SerializeField] private int _healAmount = 20;
    [SerializeField] private float _rotateSpd = 2f;

    private bool _isUsed;

    private void Update()
    {
        RotatePotion();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isUsed || GameManager.Instance == null || !GameManager.Instance.IsPlaying)
        {
            return;
        }

        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        _isUsed = true;

        GameManager.Instance.PlayerHeal(_healAmount);

        Destroy(gameObject);
    }

    private void RotatePotion()
    {
        transform.RotateAround(transform.position, Vector3.up, _rotateSpd * Time.deltaTime);
    }

}
