using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CropTool
{
    None,
    Sow,
    Water,
    Harvest,
}

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            ShowToolPanel();
            //currentCropField = other.GetComponent<CropField>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            HideToolPanel();
        }
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
        for (int i = 1; i < cropTools.Count + 1; i++)
            cropTools[i - 1].image.color = Color.white;
    }

    public void SelectTool(int cropToolIndex)
    {
        activeTool = (CropTool)cropToolIndex;
        for (int i = 1; i < cropTools.Count + 1; i++)
            cropTools[i - 1].image.color = i == cropToolIndex ? selectedToolColor : Color.white;
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
