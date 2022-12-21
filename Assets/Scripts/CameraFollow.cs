using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GunController gunController;

    public float smothness = 5f;

    public float zoomInFOV = 37f;
    public float zoomOutFOV = 60f;

    public Transform gunBaseTransform;
    public Transform gunHolderTransform;
    public Transform cameraHolderTransform;

    Camera mainCamera;

    Transform myTransform;

    void Start()
    {
        myTransform = transform;

        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        CheckIfZoomedIn();

        myTransform.rotation = RotateTransform(myTransform.rotation, gunBaseTransform.rotation);
        cameraHolderTransform.rotation = RotateTransform(cameraHolderTransform.rotation, gunHolderTransform.rotation);
    }

    Quaternion RotateTransform(Quaternion currentRotation, Quaternion targetRotation)
    {
        Quaternion lerpQuaternion = Quaternion.Lerp(currentRotation, targetRotation, smothness * Time.fixedDeltaTime);

        return lerpQuaternion;
    }

    void CheckIfZoomedIn()
    {
        if (gunController.isZoomedIn)
            mainCamera.fieldOfView = zoomInFOV;
        else
            mainCamera.fieldOfView = zoomOutFOV;

    }
}