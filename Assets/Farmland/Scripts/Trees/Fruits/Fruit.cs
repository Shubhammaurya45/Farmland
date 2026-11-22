using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField]
    private CropData cropData;
    public CropData CropData => cropData;

    [SerializeField]
    private Renderer appleRenderer;

    [SerializeField]
    private Rigidbody rigidbody;

    [SerializeField]
    private float shakeMultiplier;

    public void Shake(float shakeMagnitude)
    {
        float realShakeMagnitude = shakeMagnitude * shakeMultiplier;
        appleRenderer.material.SetFloat("_Magnitude", realShakeMagnitude);
    }

    public void Release()
    {
        rigidbody.isKinematic = false;
        appleRenderer.material.SetFloat("_Magnitude", 0);
    }

    public bool IsFree()
    {
        return !rigidbody.isKinematic;
    }
}
