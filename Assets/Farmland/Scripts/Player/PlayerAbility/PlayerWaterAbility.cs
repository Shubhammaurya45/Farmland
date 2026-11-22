using UnityEngine;

public class PlayerWaterAbility : PlayerAbility
{
    protected override void SubscribeToEvents()
    {
        WaterParticle.onWaterCollided += WaterCollisionCallBack;
        CropField.onFullyWatered += CropFieldFullyWateredCallBack;
    }

    protected override void UnsubscribeFromEvents()
    {
        WaterParticle.onWaterCollided -= WaterCollisionCallBack;
        CropField.onFullyWatered -= CropFieldFullyWateredCallBack;
    }

    protected override bool CanUseAbility()
    {
        if (playerToolSelector == null || currentCropField == null)
            return false;

        return playerToolSelector.CanWater()
            && currentCropField.fieldState == CropFieldState.FullSown;
    }

    protected override void PlayAbilityAnimation()
    {
        if (playerAnimator != null)
            playerAnimator.PlayWaterAnimation();
    }

    protected override void StopAbilityAnimation()
    {
        if (playerAnimator != null)
            playerAnimator.StopWaterAnimation();

        if (animationEvents != null)
            animationEvents.StopWaterPartical();
    }

    private void WaterCollisionCallBack(Vector3[] waterPositions)
    {
        HandleParticleCollision(waterPositions);
    }

    private void CropFieldFullyWateredCallBack(CropField cropField)
    {
        if (cropField == currentCropField)
            HandleCropFieldComplete();
    }
}
