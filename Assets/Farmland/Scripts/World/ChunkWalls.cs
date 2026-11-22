using UnityEngine;

public class ChunkWalls : MonoBehaviour
{
    [SerializeField]
    private GameObject frontWall;

    [SerializeField]
    private GameObject backWall;

    [SerializeField]
    private GameObject rightWall;

    [SerializeField]
    private GameObject leftWall;

    public void Configure(int configuration)
    {
        frontWall.SetActive(IsKthBitSet(configuration, 0));
        rightWall.SetActive(IsKthBitSet(configuration, 1));
        backWall.SetActive(IsKthBitSet(configuration, 2));
        leftWall.SetActive(IsKthBitSet(configuration, 3));
    }

    private bool IsKthBitSet(int configuration, int k)
    {
        if ((configuration & (1 << k)) > 0)
            return false;

        return true;
    }
}
