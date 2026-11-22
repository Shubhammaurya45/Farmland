using System;
using System.Collections;
using UnityEngine;

public class CropTile : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Transform cropParent;
    private MeshRenderer meshRenderer;
    private Coroutine colorCoroutine;

    private Crop crop;

    public CropTileState currentCropState;

    [Header("Events")]
    public static Action<CropData> onCropHarvest;

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
        currentCropState = CropTileState.Watered;
        StartColorTransition(Color.white * 0.3f, 1f);
        crop.ScaleUp();
    }

    public void Harvest(CropData cropData)
    {
        currentCropState = CropTileState.Empty;
        crop.ScaleDown();
        StartColorTransition(Color.white, 1f);
        onCropHarvest.Invoke(cropData);
    }

    private void StartColorTransition(Color targetColor, float duration)
    {
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
        }

        colorCoroutine = StartCoroutine(LerpColor(targetColor, duration));
    }

    private IEnumerator LerpColor(Color targetColor, float duration)
    {
        Color startColor = meshRenderer.material.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);
            meshRenderer.material.color = Color.Lerp(startColor, targetColor, progress);
            yield return null;
        }

        meshRenderer.material.color = targetColor;
        colorCoroutine = null;
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

    public bool IsHarvest()
    {
        return currentCropState == CropTileState.Harvest;
    }
}
