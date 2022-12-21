using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointIllustration : MonoBehaviour
{
    Transform[] waypointsTransformList = new Transform[0];

    void Awake()
    {
        waypointsTransformList = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            waypointsTransformList[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        for (int i = 0; i < waypointsTransformList.Length; i++)
        {
            int x = 0;

            x = i == (waypointsTransformList.Length - 1) ? x = 0 : x = (i + 1);

            Debug.DrawLine(waypointsTransformList[i].position, waypointsTransformList[x].position, Color.red);
        }
    }
}
