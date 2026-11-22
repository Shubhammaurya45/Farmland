using System;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    //
    public static CashManager Instance;

    public static Action<int> updateCoinUI;

    public int totalCoin;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadCoin();
    }

    public int GetCoin()
    {
        return totalCoin;
    }

    public void AddCoin(int amount)
    {
        totalCoin += amount;
        updateCoinUI?.Invoke(totalCoin);
        SaveCoin();
    }

    [NaughtyAttributes.Button]
    public void Add500Coin()
    {
        AddCoin(500);
    }

    public void ReduceCoin(int amount)
    {
        AddCoin(-amount);
    }

    private void LoadCoin()
    {
        totalCoin = PlayerPrefs.GetInt("Coins");
        updateCoinUI?.Invoke(totalCoin);
    }

    private void SaveCoin()
    {
        PlayerPrefs.SetInt("Coins", totalCoin);
    }
}
