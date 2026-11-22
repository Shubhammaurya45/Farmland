using System;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public static Action<Tree> playerEnterTreeZone;
    public static Action<Tree> playerExitTreeZone;
    private Chunks chunks;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ChunkTrigger"))
        {
            chunks = other.GetComponentInParent<Chunks>();
            chunks.TryToUnlock();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Tree tree))
        {
            TriggerAppleTreeZone(tree);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Tree tree))
        {
            ExitedAppleTreeZone(tree);
        }
    }

    private void TriggerAppleTreeZone(Tree tree)
    {
        playerEnterTreeZone?.Invoke(tree);
    }

    private void ExitedAppleTreeZone(Tree tree)
    {
        playerExitTreeZone?.Invoke(tree);
    }
}
