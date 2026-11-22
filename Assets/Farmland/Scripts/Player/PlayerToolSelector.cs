using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerToolSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject toolPanel;

    [SerializeField]
    private List<Button> cropTools = new List<Button>();

    [Header("Settings")]
    [SerializeField]
    private Color selectedToolColor;
    private CropTool activeTool;

    private void Start()
    {
        HideToolPanel();
        SelectTool(0);
    }

    public void ShowToolPanel()
    {
        if (toolPanel != null)
            toolPanel.SetActive(true);
    }

    public void HideToolPanel()
    {
        if (toolPanel != null)
            toolPanel.SetActive(false);

        DefaultTool();
    }

    public void DefaultTool()
    {
        activeTool = CropTool.None;
        foreach (Button button in cropTools)
            button.image.color = Color.white;
    }

    public void SelectTool(int cropToolIndex)
    {
        activeTool = (CropTool)cropToolIndex;
        for (int i = 0; i < cropTools.Count; i++)
            cropTools[i].image.color = (i + 1 == cropToolIndex) ? selectedToolColor : Color.white;
    }

    public bool CanSow()
    {
        return activeTool == CropTool.Sow;
    }

    public bool CanWater()
    {
        return activeTool == CropTool.Water;
    }

    public bool CanHarvest()
    {
        return activeTool == CropTool.Harvest;
    }
}
