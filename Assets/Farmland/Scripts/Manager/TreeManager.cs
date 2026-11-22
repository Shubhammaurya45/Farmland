using System;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public static Action<Tree> enteredInTreeZone;
    public static Action exitTreeMode;
    private Tree tree;

    private void OnEnable()
    {
        PlayerDetection.playerEnterTreeZone += PlayerEnterTreeZoneCallBack;
        PlayerDetection.playerExitTreeZone += PlayerExitTreeZoneCallBack;
    }

    private void OnDisable()
    {
        PlayerDetection.playerEnterTreeZone -= PlayerEnterTreeZoneCallBack;
        PlayerDetection.playerExitTreeZone -= PlayerExitTreeZoneCallBack;
    }

    private void PlayerEnterTreeZoneCallBack(Tree tree)
    {
        this.tree = tree;
        UIManager.Instance?.ShowTreeUIContainer();
        UpdateTreeButtonUI();
        UIManager.Instance?.TreeUIButton.onClick.AddListener(TreeClickCallBack);
    }

    private void PlayerExitTreeZoneCallBack(Tree tree)
    {
        UIManager.Instance?.HideTreeUIContainer();
        tree = null;
    }

    public void TreeClickCallBack()
    {
        tree.EnableTreeCamera();
        UIManager.Instance?.HideTreeUIContainer();
        UIManager.Instance?.ShowShakeUIContainer();
        enteredInTreeZone?.Invoke(tree);
    }

    private void UpdateTreeButtonUI()
    {
        Fruit fruit = tree.GetComponentInChildren<Fruit>();
        UIManager.Instance.UpdateFruitIcon(fruit.CropData.cropIcon);
    }

    public void ExitTreeMode()
    {
        exitTreeMode?.Invoke();
        UIManager.Instance?.HideShakeUIContainer();
    }
}
