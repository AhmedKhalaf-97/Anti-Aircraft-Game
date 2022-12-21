using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float bulletSpeed = 150f;
    float bulletRange = 100f;

    float destroyTime = 5f;

    GameObject explosionGameobject;
    Transform myTransform;

    void Awake()
    {
        myTransform = transform;
        explosionGameobject = myTransform.Find("BulletExplosion").gameObject;
    }

    void Update()
    {
        myTransform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);

        if (Vector3.Distance(Vector3.zero, myTransform.position) > bulletRange)
            EnableExplosionParticleSystem();
    }

    void EnableExplosionParticleSystem()
    {
        bulletSpeed = 0f;

        explosionGameobject.SetActive(true);

        DestroyGameobject();
    }

    void DestroyGameobject()
    {
        Destroy(gameObject, destroyTime);
    }
}
