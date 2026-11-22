using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private GameObject treeUIContainer;

    [SerializeField]
    private Button treeUIButton;
    public Button TreeUIButton => treeUIButton;

    [SerializeField]
    private Image fruitIcon;

    [SerializeField]
    private GameObject shakeUIContainer;

    [SerializeField]
    private Slider shakeProgressSlider;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        HideTreeUIContainer();
        HideShakeUIContainer();
        UpdateShakeSliderValue(0);
    }

    public void ShowTreeUIContainer() => treeUIContainer?.SetActive(true);

    public void HideTreeUIContainer() => treeUIContainer?.SetActive(false);

    public void ShowShakeUIContainer() => shakeUIContainer?.SetActive(true);

    public void HideShakeUIContainer() => shakeUIContainer?.SetActive(false);

    public void UpdateShakeSliderValue(float value) => shakeProgressSlider.value = value;

    public void UpdateFruitIcon(Sprite icon) => fruitIcon.sprite = icon;
}
