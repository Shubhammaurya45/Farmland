using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Animator animator;

    private bool sowActive;
    private bool waterActive;
    private bool harvestActive;

    public void SetPlayerMoveAnimation(Vector3 moveVector)
    {
        animator.SetFloat("moveSpeed", moveVector.magnitude);
    }

    public void PlaySowAnimation()
    {
        if (sowActive)
            return; // don't restart every frame

        sowActive = true;
        animator.Play("Sow", 1, 0f); // always start at frame 0
        animator.SetLayerWeight(1, 1f);
    }

    public void StopSowAnimation()
    {
        if (!sowActive)
            return;
        sowActive = false;
        animator.SetLayerWeight(1, 0f);
        animator.Play("Empty", 1, 0f);
        ;
    }

    public void PlayWaterAnimation()
    {
        if (waterActive)
            return;
        waterActive = true;
        animator.Play("Water", 2, 0f);
        animator.SetLayerWeight(2, 1f);
    }

    public void StopWaterAnimation()
    {
        if (!waterActive)
            return;
        waterActive = false;
        animator.SetLayerWeight(2, 0f);
        animator.Play("Empty", 2, 0f);
    }

    public void PlayHarvestAnimation()
    {
        if (harvestActive)
            return;
        harvestActive = true;
        animator.Play("Harvest", 3, 0f);
        animator.SetLayerWeight(3, 1f);
    }

    public void StopHarvestAnimation()
    {
        if (!harvestActive)
            return;
        harvestActive = false;
        animator.SetLayerWeight(3, 0f);
        animator.Play("Empty", 3, 0f);
    }

    public void PlayShakeTreeAnimation()
    {
        animator.SetLayerWeight(4, 1);
        animator.Play("Shake Tree");
    }

    public void StopShakeTreeAnimation()
    {
        animator.SetLayerWeight(4, 0);
        //animator.
    }
}
