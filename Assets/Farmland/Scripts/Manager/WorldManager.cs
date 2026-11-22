using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Flags]
public enum NeighborFlags
{
    None = 0,
    Front = 1,
    Right = 2,
    Back = 4,
    Left = 8,
}

public enum ChunkShape
{
    None,
    TopRight,
    BottomRight,
    BottomLeft,
    TopLeft,
    Top,
    Right,
    Bottom,
    Left,
    Four,
}

public class WorldManager : MonoBehaviour
{
    private const float SaveIntervalSeconds = 1f;

    //
    [SerializeField]
    private Transform world;

    [Header("Grid Settings")]
    [SerializeField, Min(1)]
    private int gridSize = 1;

    [SerializeField, Min(1)]
    private int gridScale = 1;

    private WorldData worldData;
    private Chunks[,] grid;
    private string dataPath;
    private bool shouldSaveWorld;

    private void OnEnable()
    {
        Chunks.onChunkUnlock += ChunkUnlockCallBack;
        Chunks.onChunkPriceChange += ChunkPriceChangeCallBack;
    }

    private void OnDisable()
    {
        Chunks.onChunkUnlock -= ChunkUnlockCallBack;
        Chunks.onChunkPriceChange -= ChunkPriceChangeCallBack;
    }

    private void Start()
    {
        if (world == null || gridSize <= 0 || gridScale <= 0)
        {
            Debug.LogError("Assign World and set Grid Size / Grid Scale above zero.", this);
            enabled = false;
            return;
        }

        dataPath = Path.Combine(Application.dataPath + "/WorldData.txt");
        LoadWorld();
        InitializeWorld();
        InvokeRepeating(nameof(TryToSaveWorld), SaveIntervalSeconds, SaveIntervalSeconds);
    }

    private void ChunkUnlockCallBack()
    {
        InitializeChunkWalls();
        UpdateGridRender();
        SaveWorld();
    }

    private void ChunkPriceChangeCallBack() => shouldSaveWorld = true;

    private void InitializeWorld()
    {
        int priceIndex = 0;

        for (int i = 0; i < world.childCount; i++)
        {
            Chunks chunk = world.GetChild(i).GetComponent<Chunks>();
            if (chunk == null)
                continue;

            if (priceIndex >= worldData.chunkPrice.Count)
            {
                Debug.LogError("Saved chunk data does not match the scene.");
                return;
            }

            chunk.Initialize(worldData.chunkPrice[priceIndex]);
            priceIndex++;

            InitializeGrid();
            UpdateGridRender();
            InitializeChunkWall();
        }
    }

    private void InitializeGrid()
    {
        grid = new Chunks[gridSize, gridSize];
        for (int i = 0; i < world.childCount; i++)
        {
            Chunks chunk = world.GetChild(i).GetComponent<Chunks>();

            if (chunk == null)
                continue;

            Vector3 pos = chunk.transform.position;

            Vector2Int chunkGridPosition = new Vector2Int(
                Mathf.FloorToInt(pos.x / gridScale),
                Mathf.FloorToInt(pos.z / gridScale)
            );

            chunkGridPosition += new Vector2Int(gridSize / 2, gridSize / 2);

            if (!IsGridPositionValid(chunkGridPosition.x, chunkGridPosition.y))
            {
                Debug.LogWarning(
                    $"Chunk '{chunk.name}' maps to {chunkGridPosition}, outside the {gridSize}x{gridSize} grid. Increase gridSize or check its position.",
                    chunk
                );
                continue;
            }

            grid[chunkGridPosition.x, chunkGridPosition.y] = chunk;
        }
    }

    private void UpdateGridRender()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Chunks chunk = grid[i, j];

                if (chunk == null || chunk.IsChunkUnlocked())
                    continue;

                Chunks frontChunk = IsGridPositionValid(i, j + 1) ? grid[i, j + 1] : null;
                Chunks rightChunk = IsGridPositionValid(i + 1, j) ? grid[i + 1, j] : null;
                Chunks backChunk = IsGridPositionValid(i, j - 1) ? grid[i, j - 1] : null;
                Chunks leftChunk = IsGridPositionValid(i - 1, j) ? grid[i - 1, j] : null;

                if (frontChunk != null && frontChunk.IsChunkUnlocked())
                    chunk.DisplayLockedElement();
                else if (rightChunk != null && rightChunk.IsChunkUnlocked())
                    chunk.DisplayLockedElement();
                else if (backChunk != null && backChunk.IsChunkUnlocked())
                    chunk.DisplayLockedElement();
                else if (leftChunk != null && leftChunk.IsChunkUnlocked())
                    chunk.DisplayLockedElement();
            }
        }
    }

    private void InitializeChunkWall()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Chunks chunk = grid[i, j];

                if (chunk == null)
                    continue;

                int configuration = (int)GetWallConfiguration(i, j);
                chunk.UpdateWalls(configuration);
            }
        }
    }

    private void InitializeChunkWalls()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Chunks chunk = grid[x, y];
                if (chunk != null)
                    chunk.UpdateWalls((int)GetWallConfiguration(x, y));
            }
        }
    }

    private NeighborFlags GetWallConfiguration(int i, int j)
    {
        NeighborFlags configuration = NeighborFlags.None;

        if (IsNeighborUnlocked(i, j + 1))
            configuration |= NeighborFlags.Front;
        if (IsNeighborUnlocked(i + 1, j))
            configuration |= NeighborFlags.Right;
        if (IsNeighborUnlocked(i, j - 1))
            configuration |= NeighborFlags.Back;
        if (IsNeighborUnlocked(i - 1, j))
            configuration |= NeighborFlags.Left;

        return configuration;
    }

    private bool IsNeighborUnlocked(int i, int j)
    {
        if (!IsGridPositionValid(i, j))
            return false;

        Chunks neighbor = grid[i, j];
        return neighbor != null && neighbor.IsChunkUnlocked();
    }

    private bool IsGridPositionValid(int x, int y) =>
        x >= 0 && x < gridSize && y >= 0 && y < gridSize;

    private WorldData CreateDefaultWorldData()
    {
        WorldData newData = new WorldData();

        for (int i = 0; i < world.childCount; i++)
        {
            Chunks chunk = world.GetChild(i).GetComponent<Chunks>();

            if (chunk != null)
                newData.chunkPrice.Add(chunk.GetInitialPrice());
        }

        string json = JsonUtility.ToJson(newData, true);
        File.WriteAllText(dataPath, json);

        return newData;
    }

    private void LoadWorld()
    {
        worldData = null;
        //dataPath = Path.Combine(Application.dataPath + "/WorldData.txt");
        //Debug.Log("World data path: " + dataPath);
        string data = "";
        if (!File.Exists(dataPath))
        {
            worldData = CreateDefaultWorldData();
            SaveWorld();
        }

        try
        {
            data = File.ReadAllText(dataPath);
            worldData = JsonUtility.FromJson<WorldData>(data);
        }
        catch (Exception e)
        {
            Debug.LogWarning("World data corrupted, resetting: " + e.Message);
            worldData = null;
        }

        if (worldData == null || worldData.chunkPrice == null)
        {
            worldData = CreateDefaultWorldData();
            SaveWorld();
            return;
        }

        if (worldData.chunkPrice.Count < world.childCount)
            UpdateData();
    }

    private void UpdateData()
    {
        int missingData = world.childCount - worldData.chunkPrice.Count;
        for (int i = 0; i < missingData; i++)
        {
            int chunkIndex = world.childCount - missingData + i;
            int chunkPrice = world.GetChild(chunkIndex).GetComponent<Chunks>().GetInitialPrice();
            worldData.chunkPrice.Add(chunkPrice);
        }
    }

    private void TryToSaveWorld()
    {
        if (!shouldSaveWorld)
            return;

        SaveWorld();
    }

    public void SaveWorld()
    {
        if (worldData.chunkPrice.Count != world.childCount)
            worldData = new WorldData();

        for (int i = 0; i < world.childCount; i++)
        {
            int chunkCurrentPrice = world.GetChild(i).GetComponent<Chunks>().GetCurrentPrice();

            if (worldData.chunkPrice.Count > i)
                worldData.chunkPrice[i] = chunkCurrentPrice;
            else
                worldData.chunkPrice.Add(chunkCurrentPrice);
        }

        string data = JsonUtility.ToJson(worldData, true);

        File.WriteAllText(dataPath, data);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause && worldData != null)
            SaveWorld();
    }

    private void OnApplicationQuit()
    {
        if (worldData != null)
            SaveWorld();
    }
}
