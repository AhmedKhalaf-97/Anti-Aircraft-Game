using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 100f;

    public ParticleSystem impactExplosion;
    public GameObject wreckedPlaneGO;

    GameObject planeTargetGO;

    private void Start()
    {
        planeTargetGO = GetComponent<AircraftAI>().waypointTarget.gameObject;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        PlayImpactExplosion();

        HitMarker.hitMarker.TargetHit();

        if (health <= 0f)
            DestroyPlane();
    }

    void DestroyPlane()
    {
        HitMarker.hitMarker.TargetDestroyed();

        wreckedPlaneGO.SetActive(true);
        wreckedPlaneGO.transform.parent = null;

        Destroy(gameObject);
        Destroy(planeTargetGO);
    }

    void PlayImpactExplosion()
    {
        impactExplosion.Play();
    }
}