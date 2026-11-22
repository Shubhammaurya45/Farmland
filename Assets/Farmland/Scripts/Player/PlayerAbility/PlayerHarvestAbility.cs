using UnityEngine;

public class PlayerHarvestAbility : PlayerAbility
{
    [SerializeField]
    private Transform harvestRange;
    public bool canHarvest = false;

    protected override void SubscribeToEvents()
    {
        CropField.onFullyHarvest += CropFieldFullyHarvestCallBack;
    }

    protected override void UnsubscribeFromEvents()
    {
        CropField.onFullyHarvest -= CropFieldFullyHarvestCallBack;
    }

    protected override bool CanUseAbility()
    {
        if (playerToolSelector == null || currentCropField == null)
            return false;

        return playerToolSelector.CanHarvest()
            && currentCropField.fieldState == CropFieldState.FullWatered;
    }

    protected override void PlayAbilityAnimation()
    {
        if (playerAnimator != null)
            playerAnimator.PlayHarvestAnimation();
    }

    protected override void StopAbilityAnimation()
    {
        if (playerAnimator != null)
            playerAnimator.StopHarvestAnimation();
    }

    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        if (canHarvest && playerToolSelector.CanHarvest())
        {
            currentCropField.Harvest(harvestRange);
        }
    }

    public void HarvestStartCallBack()
    {
        canHarvest = true;
    }

    public void HarvestStopCallBack()
    {
        canHarvest = false;
    }

    private void CropFieldFullyHarvestCallBack(CropField cropField)
    {
        if (cropField == currentCropField)
            HandleCropFieldComplete();
    }
}
