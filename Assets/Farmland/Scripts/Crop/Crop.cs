using UnityEngine;
using UnityEngine.UIElements;

public class Crop : MonoBehaviour
{
    [SerializeField]
    private Transform cropRenderer;

    public void ScaleUp()
    {
        cropRenderer.localScale = Vector3.one;
    }
}
