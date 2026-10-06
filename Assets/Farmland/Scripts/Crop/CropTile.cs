using UnityEngine;

public enum CropTileState
{
    Empty,
    Sown,
    Watered,
}

public class CropTile : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Transform cropParent;
    private MeshRenderer meshRenderer;

    private Crop crop;

    public CropTileState currentCropState;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        currentCropState = CropTileState.Empty;
    }

    public void Sow(CropData cropData)
    {
        currentCropState = CropTileState.Sown;
        crop = Instantiate(
            cropData.cropPrefab,
            transform.position,
            Quaternion.identity,
            cropParent
        );
    }

    public void Water(CropData cropData)
    {
        Debug.Log("HI");
        currentCropState = CropTileState.Watered;
        meshRenderer.material.color = Color.white * 0.3f;
        crop.ScaleUp();
    }

    public bool IsEmpty()
    {
        return currentCropState == CropTileState.Empty;
    }

    public bool IsSown()
    {
        return currentCropState == CropTileState.Sown;
    }

    public bool IsWater()
    {
        return currentCropState == CropTileState.Watered;
    }
}
