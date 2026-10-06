using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerToolSelector))]
public class PlayerAbility : MonoBehaviour
{
    protected PlayerAnimator playerAnimator;
    protected PlayerToolSelector playerToolSelector;

    protected CropField currentCropField;

    /// <summary>
    /// Flag to track if the ability is currently active.
    /// Used to prevent animation from restarting after completion.
    /// </summary>
    private bool isAbilityActive = false;

    protected virtual void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();

        SubscribeToEvents();
    }

    protected virtual void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    protected virtual void SubscribeToEvents() { }

    protected virtual void UnsubscribeFromEvents() { }

    protected virtual bool CanUseAbility()
    {
        return false;
    }

    protected virtual void PlayAbilityAnimation() { }

    protected virtual void StopAbilityAnimation() { }

    protected void HandleParticleCollision(Vector3[] collisionPositions)
    {
        if (currentCropField == null)
            return;

        if (playerToolSelector.CanSow())
            currentCropField.SeedsCollidedCallBack(collisionPositions);

        if (playerToolSelector.CanWater())
            currentCropField.WaterCollidedCallBack(collisionPositions);
    }

    protected void HandleCropFieldComplete()
    {
        playerToolSelector.DefaultTool();
        StopAbilityAnimation();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            currentCropField = other.GetComponent<CropField>();
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (CanUseAbility())
            PlayAbilityAnimation();
        else
            StopAbilityAnimation();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            StopAbilityAnimation();
            playerToolSelector.HideToolPanel();
            currentCropField = null;
        }
    }
}
