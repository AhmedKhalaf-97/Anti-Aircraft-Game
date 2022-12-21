using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftBullet : MonoBehaviour
{
    float bulletSpeed = 300f;

    float damageAmount = 3f;

    Vector3 dir;
    Vector3 offset;

    [HideInInspector]
    public Transform aircraftOriginTransform;

    Transform playerTransform;
    Transform myTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            ApplyDamageToPlayer();
    }

    private void Awake()
    {
        myTransform = transform;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        ChangeBulletLineToRandomZPosition();
    }

    private void Start()
    {
        AdjustBulletDirection();
    }

    private void Update()
    {
        myTransform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    void ChangeBulletLineToRandomZPosition()
    {
        myTransform.GetChild(0).localPosition = new Vector3(0f, 0f, Random.Range(-10, 10));
    }

    void AdjustBulletDirection()
    {
        offset = new Vector3((int)(myTransform.position.x - aircraftOriginTransform.position.x), 0f, 0f);

        dir = ((playerTransform.position + offset) - myTransform.position);

        myTransform.rotation = Quaternion.LookRotation(dir);
    }

    void ApplyDamageToPlayer()
    {
        if (!DamageIndicatorSystem.checkIfObjectInSight(aircraftOriginTransform))
            DamageIndicatorSystem.createIndicator(aircraftOriginTransform);

        PlayerHealth.playerHealth.TakeDamage(damageAmount);

        Destroy(gameObject, 2f);
    }
}