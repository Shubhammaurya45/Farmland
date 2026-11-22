using System;
using System.Collections.Generic;
using UnityEngine;

public class CropField : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Transform tileParent;

    [SerializeField]
    private CropData cropData;

    [Header("Setting")]
    private List<CropTile> cropTiles = new List<CropTile>();
    private int tilesSown;
    private int tilesWatered;
    private int tilesHarvest;
    public CropFieldState fieldState;

    [Header("Action")]
    public static Action<CropField> onFullySown;
    public static Action<CropField> onFullyWatered;
    public static Action<CropField> onFullyHarvest;

    private void Start()
    {
        fieldState = CropFieldState.Empty;
        StoreTiles();
    }

    private void StoreTiles()
    {
        if (tileParent == null)
            return;

        for (int i = 0; i < tileParent.childCount; i++)
        {
            CropTile cropTile = tileParent.GetChild(i).GetComponent<CropTile>();

            if (cropTile != null)
                cropTiles.Add(cropTile);
        }
    }

    public void SeedsCollidedCallBack(Vector3[] seedPositions)
    {
        for (int i = 0; i < seedPositions.Length; i++)
        {
            CropTile closetCropTile = GetClosetCropTile(seedPositions[i]);

            if (closetCropTile == null)
                continue;
            if (!closetCropTile.IsEmpty())
                continue;

            Sow(closetCropTile);
        }
    }

    private void Sow(CropTile cropTile)
    {
        cropTile.Sow(cropData);
        tilesSown++;

        if (tilesSown == cropTiles.Count)
            FieldFullSown();
    }

    private void FieldFullSown()
    {
        fieldState = CropFieldState.FullSown;
        onFullySown?.Invoke(this);
    }

    public void WaterCollidedCallBack(Vector3[] waterPositions)
    {
        for (int i = 0; i < waterPositions.Length; i++)
        {
            CropTile closetCropTile = GetClosetCropTile(waterPositions[i]);

            if (closetCropTile == null)
                continue;
            if (!closetCropTile.IsSown())
                continue;

            Water(closetCropTile);
        }
    }

    private void Water(CropTile cropTile)
    {
        cropTile.Water(cropData);
        tilesWatered++;

        if (tilesWatered == cropTiles.Count)
            FieldFullWatered();
    }

    private void FieldFullWatered()
    {
        fieldState = CropFieldState.FullWatered;
        onFullyWatered?.Invoke(this);
    }

    public void Harvest(Transform harvestRange)
    {
        float harvestRadius = harvestRange.localScale.x;

        for (int i = 0; i < cropTiles.Count; i++)
        {
            if (cropTiles[i].IsEmpty())
                continue;

            float distanceCropTileSphere = Vector3.Distance(
                harvestRange.position,
                cropTiles[i].transform.position
            );
            if (distanceCropTileSphere <= harvestRadius)
                HarvestTile(cropTiles[i]);
        }
    }

    private void HarvestTile(CropTile cropTile)
    {
        cropTile.Harvest(cropData);
        tilesHarvest++;

        if (tilesHarvest == cropTiles.Count)
            FieldFullHarvest();
    }

    private void FieldFullHarvest()
    {
        tilesSown = 0;
        tilesWatered = 0;
        tilesHarvest = 0;
        fieldState = CropFieldState.Empty;
        onFullyHarvest?.Invoke(this);
    }

    private CropTile GetClosetCropTile(Vector3 seedPosition)
    {
        float minDistance = 5000;
        int closetCropTileIndex = -1;
        for (int i = 0; i < cropTiles.Count; i++)
        {
            CropTile cropTile = cropTiles[i];
            float distanceTileSeed = Vector3.Distance(cropTile.transform.position, seedPosition);
            if (distanceTileSeed < minDistance)
            {
                minDistance = distanceTileSeed;
                closetCropTileIndex = i;
            }
        }
        if (closetCropTileIndex == -1)
        {
            return null;
        }
        return cropTiles[closetCropTileIndex];
    }
}
