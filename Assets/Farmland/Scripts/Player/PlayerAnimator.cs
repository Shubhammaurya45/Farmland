using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Animator animator;

    public void SetPlayerMoveAnimation(Vector3 moveVector)
    {
        animator.SetFloat("moveSpeed", moveVector.magnitude);
    }

    public void PlaySowAnimation()
    {
        animator.SetLayerWeight(1, 1);
    }

    public void StopSowAnimation()
    {
        animator.SetLayerWeight(1, 0);
    }

    public void PlayWaterAnimation()
    {
        animator.SetLayerWeight(2, 1);
    }

    public void StopWaterAnimation()
    {
        animator.SetLayerWeight(2, 0);
    }

    public void PlayHarvestAnimation()
    {
        //
    }

    public void StopHarvestAnimation()
    {
        //
    }
}
