using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public bool isAvailable;

    Transform[] waypointsTransforms;
    int waypointsCount;

    Transform nearestWaypointToAirplane;
    Transform expectedNextWapoint;
    Transform farWaypoint;

    [HideInInspector]
    public Transform airplaneTransform;
    Transform myTransform;


    public Transform NearestWaypoint() { return nearestWaypointToAirplane; }
    public Transform ExpectedNextWaypoint() { return expectedNextWapoint; }
    public Transform FarWaypoint() { return farWaypoint; }


    void Start()
    {
        myTransform = transform;
        waypointsCount = myTransform.childCount;
        waypointsTransforms = new Transform[waypointsCount];

        for (int i = 0; i < waypointsCount; i++)
        {
            waypointsTransforms[i] = myTransform.GetChild(i);
        }

        LoadStartingWaypoints();
    }

    void Update()
    {
        isAvailable = (airplaneTransform == null);

        if (isAvailable)
        {
            LoadStartingWaypoints();
            return;
        }

        TrackAirplaneLocationToWaypoints();
    }

    void LoadStartingWaypoints()
    {
        nearestWaypointToAirplane = waypointsTransforms[0];
        expectedNextWapoint = waypointsTransforms[1];
        farWaypoint = waypointsTransforms[2];
    }

    void TrackAirplaneLocationToWaypoints()
    {
        for (int i = 0; i < waypointsCount; i++)
        {
            float currentDistance = Vector3.Distance(airplaneTransform.position, waypointsTransforms[i].position);
            float lowestDistance = Vector3.Distance(airplaneTransform.position, nearestWaypointToAirplane.position);

            if (currentDistance < lowestDistance)
            {
                nearestWaypointToAirplane = waypointsTransforms[i];

                if (farWaypoint == waypointsTransforms[0])
                    i = -1;

                if (i < (waypointsCount - 2))
                {
                    expectedNextWapoint = waypointsTransforms[i + 1];
                    farWaypoint = waypointsTransforms[i + 2];
                }
                else
                {
                    expectedNextWapoint = waypointsTransforms[waypointsCount - 1];
                    farWaypoint = waypointsTransforms[0];
                }
            }
        }
    }
}