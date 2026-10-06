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
        return playerToolSelector != null ? playerToolSelector.CanSow() : false;
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
