using System.Collections;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField]
    private Transform cropRenderer;

    [SerializeField]
    private ParticleSystem harvestParticle;

    public void ScaleUp()
    {
        StartCoroutine(ScaleOutBack(cropRenderer.transform, Vector3.one, 1f));
    }

    public void ScaleDown()
    {
        StartCoroutine(ScaleDownAndDestroy(cropRenderer, Vector3.zero, 1f));

        harvestParticle.transform.parent = null;
        harvestParticle.gameObject.SetActive(true);
        harvestParticle.Play();
    }

    private IEnumerator ScaleOutBack(Transform target, Vector3 targetScale, float duration)
    {
        Vector3 startScale = target.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / duration);
            float easedProgress = EaseOutBack(progress);

            target.localScale = Vector3.Lerp(startScale, targetScale, easedProgress);

            yield return null;
        }

        target.localScale = targetScale;
    }

    private float EaseOutBack(float value)
    {
        const float overshoot = 1.70158f;
        float adjustedValue = value - 1f;

        return adjustedValue * adjustedValue * ((overshoot + 1f) * adjustedValue + overshoot) + 1f;
    }

    private IEnumerator ScaleDownAndDestroy(Transform target, Vector3 targetScale, float duration)
    {
        Vector3 startScale = target.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / duration);
            float easedProgress = EaseInBack(progress);

            target.localScale = Vector3.Lerp(startScale, targetScale, easedProgress);

            yield return null;
        }

        target.localScale = targetScale;
        Destroy(gameObject);
    }

    private float EaseInBack(float value)
    {
        const float overshoot = 1.70158f;

        return value * value * ((overshoot + 1f) * value - overshoot);
    }
}
