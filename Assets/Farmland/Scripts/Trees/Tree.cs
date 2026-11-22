using System;
using System.Collections;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private TreeManager treeManager;

    [SerializeField]
    private Renderer[] renderers;

    [SerializeField]
    private Transform fruitParent;

    [SerializeField]
    private GameObject treeCam;

    [Header("Setting")]
    [SerializeField]
    private float maxShakeMagnitude = 0.01f;

    [SerializeField]
    private float shakeDuration = 0.15f;

    [SerializeField]
    private float shakeIncrement;
    private float shakeSliderValue;

    private float shakeMagnitude;
    private Coroutine shakeCoroutine;

    public static Action<CropData> onFruitHarvest;

    private void Start()
    {
        treeManager = FindFirstObjectByType<TreeManager>();
        DisableTreeCamera();
        UpdateMaterial();
    }

    public void EnableTreeCamera() => treeCam.SetActive(true);

    public void DisableTreeCamera() => treeCam.SetActive(false);

    public void Shake()
    {
        StartShake(maxShakeMagnitude);
        shakeSliderValue += shakeIncrement;
        UIManager.Instance.UpdateShakeSliderValue(shakeSliderValue);
        for (int i = 0; i < fruitParent.childCount; i++)
        {
            Fruit currentApple = fruitParent.GetChild(i).GetComponent<Fruit>();

            float applePercent = ((float)i / fruitParent.childCount);
            if (shakeSliderValue > applePercent && !currentApple.IsFree())
                ReleaseApple(currentApple);
        }

        if (shakeSliderValue >= 1)
            ExitTreeMode();
    }

    private void ReleaseApple(Fruit currentApple)
    {
        currentApple.Release();
        onFruitHarvest.Invoke(currentApple.CropData);
    }

    public void StopShake()
    {
        StartShake(0f);
    }

    private void StartShake(float targetMagnitude)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(TweenShake(targetMagnitude));
    }

    private IEnumerator TweenShake(float targetMagnitude)
    {
        float startMagnitude = shakeMagnitude;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / shakeDuration);
            shakeMagnitude = Mathf.Lerp(startMagnitude, targetMagnitude, progress);

            UpdateMaterial();

            yield return null;
        }

        shakeMagnitude = targetMagnitude;
        UpdateMaterial();
        shakeCoroutine = null;
    }

    private void UpdateMaterial()
    {
        foreach (Renderer treeRenderer in renderers)
        {
            if (treeRenderer != null)
                treeRenderer.material.SetFloat("_Magnitude", shakeMagnitude);
        }

        foreach (Transform fruitT in fruitParent)
        {
            Fruit apple = fruitT.GetComponent<Fruit>();

            if (apple.IsFree())
                continue;

            apple.Shake(shakeMagnitude);
        }
    }

    private void ExitTreeMode()
    {
        //
        treeManager.ExitTreeMode();
        DisableTreeCamera();
        StopShake();
    }

    private void OnDisable()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }
    }
}
