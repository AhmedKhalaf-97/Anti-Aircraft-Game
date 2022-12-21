using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckedPlane : MonoBehaviour
{
    public GameObject destroyedPlanePrefab;

    GameObject planeExplosionParticleEffect;

    Rigidbody myRigidbody;
    Transform myTransform;


    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Ground")
            ExplodePlane();
    }

    void Awake()
    {
        myTransform = transform;
        myRigidbody = GetComponent<Rigidbody>();

        planeExplosionParticleEffect = myTransform.Find("PlaneBigExplosion").gameObject;
    }

    void Start()
    {
        DestroyPlane();

    }

    void DestroyPlane()
    {
        myRigidbody.AddRelativeForce(-Vector3.up * 50f, ForceMode.Impulse);
    }

    void ExplodePlane()
    {
        Instantiate(destroyedPlanePrefab, myTransform.position + Vector3.up, myTransform.rotation);

        planeExplosionParticleEffect.transform.parent = null;
        planeExplosionParticleEffect.SetActive(true);

        Destroy(myTransform.parent.gameObject);
    }
}