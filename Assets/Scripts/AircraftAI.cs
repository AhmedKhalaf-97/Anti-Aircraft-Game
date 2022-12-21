using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftAI : MonoBehaviour
{
    public Waypoints waypoints;

    public float movingSpeed = 0.1f;
    public float rotationSpeed = 5f;

    float movingSmoothness;
    bool isPointsAssigned;

    Vector3 startingPointPosition;
    Transform middlePoint;
    Transform endPoint;

    Transform _middlePoint;
    Transform _endPoint;

    public Transform waypointTarget;

    Vector3 targetDirection;
    Quaternion angleAxis;

    float pitchingAngle;
    float rollingAngle;
    float yawingAngle;

    [Header("Propeller Fans")]
    public Transform propellerTransform;
    public float propellerSpeed = 10f;

    Transform myTransform;

    void Awake()
    {
        myTransform = transform;

        waypointTarget.parent = null;
    }

    void Start()
    {
        waypoints.airplaneTransform = myTransform;
    }

    void Update()
    {
        if(waypoints == null)
        {
            print("Assign waypoints. The AI won't work without it.");
            return;
        }

        SetTrackPointsForMoving();
        Moving();
        AdjustRotationByAngleAxis();

        RotatePropellerFans();
    }

    void Moving()
    {
        movingSmoothness += Time.deltaTime * movingSpeed;

        myTransform.position = GetQuadCurve(movingSmoothness);

        waypointTarget.position = GetQuadCurve(movingSmoothness * 1.5f);
    }

    Vector3 GetQuadCurve(float _movingSmoothness)
    {
        return QuadCurve(startingPointPosition, middlePoint.position, endPoint.position, _movingSmoothness);
    }

    void SetTrackPointsForMoving()
    {
        _middlePoint = waypoints.ExpectedNextWaypoint();
        _endPoint = waypoints.FarWaypoint();

        if (!isPointsAssigned)
        {
            movingSmoothness = 0;

            startingPointPosition = myTransform.position;
            middlePoint = _middlePoint;
            endPoint = _endPoint;

            isPointsAssigned = true;
        }


        if (Vector3.Distance(waypointTarget.position, endPoint.position) <= 10)
            isPointsAssigned = false;

    }

    void AdjustRotationByAngleAxis()
    {
        targetDirection = waypointTarget.position - myTransform.position;

        pitchingAngle = Mathf.Atan2(Mathf.Abs(targetDirection.z), targetDirection.y) * Mathf.Rad2Deg - 90;
        rollingAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
        yawingAngle = Mathf.Atan2(targetDirection.z, -targetDirection.x) * Mathf.Rad2Deg - 90;

        pitchingAngle = Mathf.Clamp(pitchingAngle, -60f, 60f);
        rollingAngle = Mathf.Clamp(rollingAngle, -30f, 30f); //we can limit rolling angle to 0f, to prevent flipping upside down.
        //yawingAngle = Mathf.Clamp(yawingAngle, -5f, 5f);    We can't limit yawing angle in our case, because it's responsible for looking into the right direction.

        angleAxis = Quaternion.Euler(pitchingAngle, yawingAngle, rollingAngle);

        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, angleAxis, Time.deltaTime * rotationSpeed);

        Debug.DrawRay(myTransform.position, targetDirection, Color.green);
    }

    public Vector3 QuadCurve(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 p0 = Vector3.Lerp(a, b, t);
        Vector3 p1 = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(p0, p1, t);
    }

    void RotatePropellerFans()
    {
        propellerTransform.Rotate(Vector3.forward, propellerSpeed);
    }
}