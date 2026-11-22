using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICropContainer : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Image cropIcon;

    [SerializeField]
    private TextMeshProUGUI cropAmount;

    public static Event updateCropUI;

    public void SetCropUIData(Sprite cropIcon, int cropAmount)
    {
        this.cropIcon.sprite = cropIcon;
        this.cropAmount.text = cropAmount.ToString();
    }

    public void UpdateCropUI(int amount)
    {
        cropAmount.text = amount.ToString();
    }
}
