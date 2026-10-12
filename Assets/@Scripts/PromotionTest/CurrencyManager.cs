using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int _startGold = 100;

    private int _gold;

    public int Gold => _gold;

    public event Action OnGoldChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _gold = _startGold;
    }

    public void AddGold(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        _gold += amount;
        OnGoldChanged?.Invoke();
    }

    public bool SpendGold(int amount)
    {
        if (amount < 0 || _gold < amount)
        {
            return false;
        }

        _gold -= amount;
        OnGoldChanged?.Invoke();

        return true;
    }
}
