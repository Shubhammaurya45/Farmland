using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEvent : MonoBehaviour
{
    [Header("Referance")]
    [SerializeField]
    private ParticleSystem seedPartical;

    [SerializeField]
    private ParticleSystem waterPartical;

    [Header("Events")]
    [SerializeField]
    private UnityEvent startHarvestingEvent;

    [SerializeField]
    private UnityEvent stopHarvestingEvent;

    public void PlaySeedPartical()
    {
        seedPartical.Play();
    }

    public void PlayWaterPartical()
    {
        if (!waterPartical.isPlaying)
            waterPartical.Play();
    }

    public void StopWaterPartical()
    {
        if (waterPartical != null && waterPartical.isPlaying)
            waterPartical.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void StopSeedPartical()
    {
        if (seedPartical != null && seedPartical.isPlaying)
            seedPartical.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void StartHarvestingCallBack()
    {
        startHarvestingEvent?.Invoke();
    }

    public void StopHarvestingCallBack()
    {
        stopHarvestingEvent?.Invoke();
    }
}
