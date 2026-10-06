using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [Header("Referance")]
    [SerializeField]
    private ParticleSystem seedPartical;

    [SerializeField]
    private ParticleSystem waterPartical;

    public void PlaySeedPartical()
    {
        seedPartical.Play();
    }

    public void PlayWaterPartical()
    {
        waterPartical.Play();
    }
}
