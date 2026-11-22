using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [Header("Data")]
    [SerializeField]
    private CropData[] cropData;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Sprite GetCropSpriteFromCropType(CropType cropType)
    {
        for (int i = 0; i < cropData.Length; i++)
        {
            if (cropData[i].cropType == cropType)
            {
                return cropData[i].cropIcon;
            }
        }
        return null;
    }

    public int GetCropPriceFromCropType(CropType cropType)
    {
        for (int i = 0; i < cropData.Length; i++)
        {
            if (cropData[i].cropType == cropType)
            {
                return cropData[i].cropPrice;
            }
        }
        return 0;
    }
}
