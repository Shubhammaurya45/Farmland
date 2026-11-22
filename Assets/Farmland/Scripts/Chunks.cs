using System;
using TMPro;
using UnityEngine;

public class Chunks : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private GameObject lockedElement;

    [SerializeField]
    private GameObject unlockedElement;

    [SerializeField]
    private GameObject ChunkPrefab;

    [SerializeField]
    private TextMeshProUGUI chunksPriceText;
    private ChunkWalls chunkWalls;

    [Header("Chunk Setting")]
    [SerializeField]
    private int initialPrice;

    private int currentPrice;
    private bool chunkUnlocked = false;

    public static Action onChunkUnlock;
    public static Action onChunkPriceChange;

    private void Start()
    {
        chunkWalls = GetComponent<ChunkWalls>();
        currentPrice = initialPrice;
        chunksPriceText.text = initialPrice.ToString();
    }

    public void Initialize(int loadPrice)
    {
        currentPrice = loadPrice;
        chunksPriceText.text = loadPrice.ToString();
        if (currentPrice <= 0)
            UnlockChunk(false);
    }

    public void UpdateWalls(int configuration)
    {
        if (chunkWalls != null)
            chunkWalls.Configure(configuration);
    }

    public void DisplayLockedElement()
    {
        unlockedElement.SetActive(false);
        lockedElement.SetActive(true);
    }

    public void TryToUnlock()
    {
        if (CashManager.Instance.GetCoin() <= 0)
            return;

        currentPrice--;
        CashManager.Instance.ReduceCoin(1);
        onChunkPriceChange?.Invoke();
        chunksPriceText.text = currentPrice.ToString();

        if (currentPrice <= 0)
            UnlockChunk();
    }

    public int GetInitialPrice()
    {
        return initialPrice;
    }

    public int GetCurrentPrice()
    {
        return currentPrice;
    }

    public bool IsChunkUnlocked()
    {
        return chunkUnlocked;
    }

    private void UnlockChunk(bool triggerAction = true)
    {
        lockedElement.SetActive(false);
        unlockedElement.SetActive(true);
        chunkUnlocked = true;

        if (triggerAction)
            onChunkUnlock?.Invoke();
    }
}
