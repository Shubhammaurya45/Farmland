using System;
using TMPro;
using UnityEngine;

public class UICashContainer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI coinText;

    private void OnEnable()
    {
        CashManager.updateCoinUI += UpdateCoin;
    }

    private void OnDisable()
    {
        CashManager.updateCoinUI -= UpdateCoin;
    }

    public void UpdateCoin(int amount)
    {
        coinText.text = amount.ToString();
    }
}
