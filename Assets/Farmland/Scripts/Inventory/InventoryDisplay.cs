using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField]
    private UICropContainer uiCropContainerPrefab;

    [SerializeField]
    private Transform uiCropContainerParent;

    private readonly Dictionary<CropType, UICropContainer> cropContainers =
        new Dictionary<CropType, UICropContainer>();

    public void SetCropDisplay(Inventory inventory)
    {
        foreach (UICropContainer container in cropContainers.Values)
            Destroy(container.gameObject);

        cropContainers.Clear();

        InventoryItem[] items = inventory.GetInventoryItems();

        for (int i = 0; i < items.Length; i++)
        {
            CreateCropContainer(items[i].cropType, items[i].amount);
        }
    }

    public void SubscribeToInventory(Inventory inventory)
    {
        inventory.OnCropAmountChanged += UpdateCropAmount;
    }

    public void UnsubscribeFromInventory(Inventory inventory)
    {
        inventory.OnCropAmountChanged -= UpdateCropAmount;
    }

    private void CreateCropContainer(CropType cropType, int amount)
    {
        if (cropContainers.TryGetValue(cropType, out UICropContainer existing))
        {
            existing.UpdateCropUI(amount);
            return;
        }

        UICropContainer cropContainer = Instantiate(uiCropContainerPrefab, uiCropContainerParent);

        Sprite cropSprite = DataManager.Instance.GetCropSpriteFromCropType(cropType);
        cropContainer.SetCropUIData(cropSprite, amount);
        cropContainers.Add(cropType, cropContainer);
    }

    private void UpdateCropAmount(CropType cropType, int amount)
    {
        if (cropContainers.TryGetValue(cropType, out UICropContainer cropContainer))
        {
            cropContainer.UpdateCropUI(amount);
            return;
        }

        CreateCropContainer(cropType, amount);
    }
}
