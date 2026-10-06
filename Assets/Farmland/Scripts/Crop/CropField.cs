using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public enum CropFieldState
{
    Empty,
    FullSown,
    FullWatered,
}

public class CropField : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Transform tileParent;

    [SerializeField]
    private CropData cropData;

    [Header("Setting")]
    private List<CropTile> cropTiles = new List<CropTile>();
    private int tilesSown = 0;
    private int tilesWatered = 0;
    public CropFieldState fieldState;

    [Header("Action")]
    public static Action<CropField> onFullySown;
    public static Action<CropField> onFullyWatered;

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
            cropTiles.Add(tileParent.GetChild(i).GetComponent<CropTile>());
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
        onFullySown.Invoke(this);
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
        Debug.Log("HI");
        cropTile.Water(cropData);
        tilesWatered++;

        if (tilesWatered == cropTiles.Count)
            FieldFullWatered();
    }

    private void FieldFullWatered()
    {
        fieldState = CropFieldState.FullWatered;
        onFullyWatered.Invoke(this);
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
