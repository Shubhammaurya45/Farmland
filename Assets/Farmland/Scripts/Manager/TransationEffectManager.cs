using System.Collections;
using UnityEngine;

public class TransationEffectManager : MonoBehaviour
{
    public static TransationEffectManager Instance;

    [SerializeField]
    private ParticleSystem coinPS;

    [SerializeField]
    private RectTransform coinRectTransform;

    [Header("Setting")]
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private int coinAmount;

    private Camera camera;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        camera = Camera.main;
    }

    public void PlayCoinParticles(int amount)
    {
        if (coinPS.isPlaying || amount == 0)
            return;

        ParticleSystem.Burst burst = coinPS.emission.GetBurst(0);
        burst.count = amount;
        coinPS.emission.SetBurst(0, burst);

        ParticleSystem.MainModule main = coinPS.main;
        main.gravityModifier = 2;
        coinPS.Play();

        coinAmount = amount;
        StartCoroutine(PlayCoinParticlesCoroutine());
    }

    private IEnumerator PlayCoinParticlesCoroutine()
    {
        yield return new WaitForSeconds(1);

        ParticleSystem.MainModule main = coinPS.main;
        main.gravityModifier = 0;

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[coinAmount];

        coinPS.GetParticles(particles);

        Vector3 direction = (coinRectTransform.position - camera.transform.position).normalized;
        Vector3 targetPosition =
            camera.transform.position
            + direction * Vector3.Distance(camera.transform.position, coinPS.transform.position);
        ;

        while (coinPS.isPlaying)
        {
            int aliveCount = coinPS.GetParticles(particles);
            for (int i = 0; i < aliveCount; i++)
            {
                if (particles[i].remainingLifetime <= 0)
                    continue;

                particles[i].position = Vector3.MoveTowards(
                    particles[i].position,
                    targetPosition,
                    moveSpeed * Time.deltaTime
                );

                if (Vector3.Distance(particles[i].position, targetPosition) < 0.01f)
                {
                    particles[i].position += Vector3.up * 1000;
                    CashManager.Instance.AddCoin(1);
                }
            }
            coinPS.SetParticles(particles, aliveCount);
            yield return null;
        }
    }
}
