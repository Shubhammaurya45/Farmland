using System;
using System.IO;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Inventory inventory;
    private InventoryDisplay inventoryDisplay;
    private string dataPath;

    private void Start()
    {
        dataPath = Path.Combine(Application.dataPath + "/InventoryData.txt");

        LoadInventory();
        ConfigureInventoryDisplay();

        CropTile.onCropHarvest += CropHarvestCallBack;
        Tree.onFruitHarvest += CropHarvestCallBack;
    }

    private void OnDestroy()
    {
        if (inventoryDisplay != null && inventory != null)
            inventoryDisplay.UnsubscribeFromInventory(inventory);

        CropTile.onCropHarvest -= CropHarvestCallBack;
        Tree.onFruitHarvest -= CropHarvestCallBack;
    }

    private void ConfigureInventoryDisplay()
    {
        inventoryDisplay = GetComponent<InventoryDisplay>();
        inventoryDisplay.SetCropDisplay(inventory);
        inventoryDisplay.SubscribeToInventory(inventory);
    }

    public void ClearInventory()
    {
        inventory.Clear();
        inventoryDisplay.SetCropDisplay(inventory);
        SaveInventory();
    }

    private void CropHarvestCallBack(CropData cropData)
    {
        inventory.CropHarvestedCallBack(cropData);
        SaveInventory();
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    private void LoadInventory()
    {
        if (!File.Exists(dataPath))
        {
            inventory = new Inventory();
            return;
        }

        try
        {
            string data = File.ReadAllText(dataPath);

            inventory = string.IsNullOrWhiteSpace(data)
                ? new Inventory()
                : JsonUtility.FromJson<Inventory>(data);

            if (inventory == null)
                inventory = new Inventory();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Could not load inventory: {e.Message}");
            inventory = new Inventory();
        }
    }

    private void SaveInventory()
    {
        try
        {
            string data = JsonUtility.ToJson(inventory);
            File.WriteAllText(dataPath, data);
        }
        catch (Exception e)
        {
            Debug.LogError($"Could not save inventory: {e.Message}");
        }
    }
}
