using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event Action<CropType, int> OnCropAmountChanged;

    [SerializeField]
    private List<InventoryItem> items = new List<InventoryItem>();

    public void CropHarvestedCallBack(CropData cropData)
    {
        bool cropFound = false;

        for (int i = 0; i < items.Count; i++)
        {
            InventoryItem item = items[i];
            if (item.cropType == cropData.cropType)
            {
                item.amount++;
                OnCropAmountChanged?.Invoke(item.cropType, item.amount);
                cropFound = true;
                break;
            }
        }

        if (cropFound)
            return;

        items.Add(new InventoryItem(cropData.cropType, 1));
        OnCropAmountChanged?.Invoke(cropData.cropType, 1);
    }

    public InventoryItem[] GetInventoryItems()
    {
        return items.ToArray();
    }

    public void Clear()
    {
        items.Clear();
    }
}
