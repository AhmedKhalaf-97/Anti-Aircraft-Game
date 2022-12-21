using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftWeapon : MonoBehaviour
{
    public GameObject aircraftBullet;
    public ParticleSystem[] muzzleFlashPS;
    public Transform[] aircraftGunsTransforms;

    public float sightDistance = 300f;

    public float fireRate = 0.5f;
    float nextTimeToFire;

    private LayerMask rayCastLayerMask;

    public float firingProbability = 50f;
    bool shouldFire;

    RaycastHit hit;

    Transform myTransform;


    void Start()
    {
        myTransform = transform;

        rayCastLayerMask = LayerMask.GetMask("Default");

        InvokeRepeating("DecideFiringProbability", 0f, 30f);
    }


    void Update()
    {
        if (!shouldFire)
            return;

        CheckIfFacingPlayer();
    }

    void CheckIfFacingPlayer()
    {
        if (Physics.Raycast(myTransform.position, myTransform.forward, out hit, sightDistance, rayCastLayerMask))
        {
            if (hit.transform.CompareTag("Player"))
                Fire();
        }
    }

    void Fire()
    {
        if(Time.time > nextTimeToFire)
        {
            nextTimeToFire = fireRate + Time.time;

            for (int i = 0; i < aircraftGunsTransforms.Length; i++)
            {
                GameObject aircraftBulletGO =  Instantiate(aircraftBullet, aircraftGunsTransforms[i].position, Quaternion.LookRotation(aircraftGunsTransforms[i].forward));
                aircraftBulletGO.GetComponent<AircraftBullet>().aircraftOriginTransform = myTransform;

                muzzleFlashPS[i].Play();
            }
        }
    }

    void DecideFiringProbability() //Invoked in start method.
    {
        shouldFire = Random.Range(0, 100) > firingProbability;
    }
}