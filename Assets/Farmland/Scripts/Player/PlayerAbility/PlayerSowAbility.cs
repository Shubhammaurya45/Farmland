using UnityEngine;

public class PlayerSowAbility : PlayerAbility
{
    protected override void SubscribeToEvents()
    {
        SeedParticle.onSeedCollided += SeedCollisionCallBack;
        CropField.onFullySown += CropFieldFullySownCallBack;
    }

    protected override void UnsubscribeFromEvents()
    {
        SeedParticle.onSeedCollided -= SeedCollisionCallBack;
        CropField.onFullySown -= CropFieldFullySownCallBack;
    }

    protected override bool CanUseAbility()
    {
        if (playerToolSelector == null || currentCropField == null)
            return false;

        return playerToolSelector.CanSow() && currentCropField.fieldState == CropFieldState.Empty;
    }

    protected override void PlayAbilityAnimation()
    {
        if (playerAnimator != null)
            playerAnimator.PlaySowAnimation();
    }

    protected override void StopAbilityAnimation()
    {
        if (playerAnimator != null)
            playerAnimator.StopSowAnimation();

        if (animationEvents != null)
            animationEvents.StopSeedPartical();
    }

    private void SeedCollisionCallBack(Vector3[] seedPositions)
    {
        HandleParticleCollision(seedPositions);
    }

    private void CropFieldFullySownCallBack(CropField cropField)
    {
        if (cropField == currentCropField)
        {
            HandleCropFieldComplete();
        }
    }
}
