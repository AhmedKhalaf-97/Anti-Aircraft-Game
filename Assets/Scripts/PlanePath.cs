using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePath : MonoBehaviour
{
    public float speed = 0.5f;

    public Transform planeTransform;

    public Transform pointA;
    public Transform pointB;
    public Transform pointC;

    float t;

    void Update()
    {
        t += Time.deltaTime * speed;

        planeTransform.position = QuadCurve(pointA.position, pointB.position, pointC.position);
    }

    public Vector3 QuadCurve(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 p0 = Vector3.Lerp(a, b, t);
        Vector3 p1 = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(p0, p1, t);
    }
}