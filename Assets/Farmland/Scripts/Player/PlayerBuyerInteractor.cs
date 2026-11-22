using UnityEngine;

public class PlayerBuyerInteractor : MonoBehaviour
{
    [SerializeField]
    private InventoryManager inventoryManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Buyer"))
        {
            SellCrop();
        }
    }

    private void SellCrop()
    {
        Inventory inventory = inventoryManager.GetInventory();
        InventoryItem[] items = inventory.GetInventoryItems();

        int coinsEarned = 0;

        for (int i = 0; i < items.Length; i++)
        {
            int itemPrice = DataManager.Instance.GetCropPriceFromCropType(items[i].cropType);
            coinsEarned += itemPrice * items[i].amount;
        }
        TransationEffectManager.Instance.PlayCoinParticles(coinsEarned);
        //CashManager.Instance.AddCoin(coinsEarned);
        inventoryManager.ClearInventory();
    }
}
