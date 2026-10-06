using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class WaterParticle : MonoBehaviour
{
    public static Action<Vector3[]> onWaterCollided;

    private void OnParticleCollision(GameObject other)
    {
        if (other == null)
            return;

        ParticleSystem ps = GetComponent<ParticleSystem>();

        if (ps == null)
            return;

        List<ParticleCollisionEvent> collisionEvent = new List<ParticleCollisionEvent>();

        int collisionAmount = ps.GetCollisionEvents(other, collisionEvent);

        if (collisionAmount == 0)
            return;

        Vector3[] collisionPosition = new Vector3[collisionAmount];

        for (int i = 0; i < collisionAmount; i++)
        {
            collisionPosition[i] = collisionEvent[i].intersection;
        }

        onWaterCollided?.Invoke(collisionPosition);
    }
}
