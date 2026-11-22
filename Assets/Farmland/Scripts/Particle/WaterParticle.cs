using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class WaterParticle : MonoBehaviour
{
    public static Action<Vector3[]> onWaterCollided;

    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void OnParticleCollision(GameObject other)
    {
        if (other == null)
            return;

        ParticleSystem ps = GetComponent<ParticleSystem>();

        if (ps == null)
            return;

        collisionEvents.Clear();
        int collisionAmount = ps.GetCollisionEvents(other, collisionEvents);

        if (collisionAmount == 0)
            return;

        Vector3[] collisionPositions = new Vector3[collisionAmount];
        for (int i = 0; i < collisionAmount; i++)
        {
            collisionPositions[i] = collisionEvents[i].intersection;

            onWaterCollided?.Invoke(collisionPositions);
        }
    }
}
