using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public bool isCursorLocked;

    public bool isZoomedIn;

    public float gunMovementSpeed = 1f;

    public Transform gunBase;
    public Transform gunHolder;

    float mouseX;
    float mouseY;
    float xRotation;

    public float maxFireRate = 0.25f;
    public float minFireRate = 0.75f;
    float fireRate = 0.25f;

    float nextTimeToFire;

    RaycastHit hit;

    public Transform muzzleTransform;
    public float fireRange = 100f;
    public float damageAmount = 20f;
    public GameObject bulletGameobject;
    public ParticleSystem muzzleFlashParticleSystem;

    public Transform gunSightTransform;
    public float bouncingOffset = -0.2f;
    public float bouncingSmoothness = 1f;
    public float bouncingSpeed = 0.1f;
    float gunZPos;
    float gunZPosTarget;
    bool shouldBounceBackward;
    bool isBouncingCoroutineRunning;

    bool isFiring;
    bool stopFiring;
    public float firingAllowanceTime = 3f;
    float firingAllowanceTimer;
    public float firingDetoriationRate = 0.1f;

    public AudioSource firingAudioSource;
    public AudioSource gunHeatAudioSource;

    private bool isGunHeatACPlaying;

    void Start()
    {
        Cursor.lockState = isCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;

        fireRate = maxFireRate;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
            isZoomedIn = true;
        else
            isZoomedIn = false;

        GunMovement();

        Shoot();
    }

    void GunMovement()
    {
        mouseX = Input.GetAxis("Mouse X") * gunMovementSpeed * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * gunMovementSpeed * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 1f);

        gunBase.Rotate(Vector3.up, mouseX);
        gunHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void Shoot()
    {
        if (Input.GetButton("Fire1"))
        {
            isFiring = true;

            if (Time.time > nextTimeToFire && !stopFiring)
            {
                nextTimeToFire = fireRate + Time.time;

                muzzleFlashParticleSystem.Play();

                Fire();

                firingAudioSource.Play();

                if (!isBouncingCoroutineRunning)
                    StartCoroutine(SwitchBouncingBoolean());
            }
        }
        else
            isFiring = false;

        GunBouncingAnimation();
        GunHeat();
    }

    void Fire()
    {
        Instantiate(bulletGameobject, muzzleTransform.position, Quaternion.LookRotation(muzzleTransform.right));

        if(Physics.Raycast(muzzleTransform.position, muzzleTransform.right, out hit, fireRange))
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
                target.TakeDamage(damageAmount);
        }
    }

    void GunBouncingAnimation()
    {
        gunZPos = Mathf.Clamp(gunZPos, bouncingOffset, 0f);

        if (shouldBounceBackward)
            gunZPosTarget = bouncingOffset;
        else
            gunZPosTarget = 0f;

        gunZPos = Mathf.Lerp(gunZPos, gunZPosTarget, bouncingSmoothness * Time.deltaTime);

        gunSightTransform.localPosition = new Vector3(0f, 0f, gunZPos);
    }

    IEnumerator SwitchBouncingBoolean()
    {
        isBouncingCoroutineRunning = true;
        shouldBounceBackward = true;

        yield return new WaitForSeconds(bouncingSpeed);

        shouldBounceBackward = false;
        isBouncingCoroutineRunning = false;
    }

    void GunHeat()
    {
        if (isFiring)
        {
            firingAllowanceTimer += Time.deltaTime;

            if (firingAllowanceTimer >= firingAllowanceTime)
            {
                fireRate += Time.deltaTime * firingDetoriationRate;

                if (fireRate >= minFireRate)
                    stopFiring = true;
            }
        }
        else
        {
            firingAllowanceTimer = 0f;
            fireRate = maxFireRate;
            stopFiring = false;
            isGunHeatACPlaying = false;
        }


        if (stopFiring && !isGunHeatACPlaying)
        {
            isGunHeatACPlaying = true;
            gunHeatAudioSource.Play();
        }
    }
}