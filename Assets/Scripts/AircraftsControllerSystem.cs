using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftsControllerSystem : MonoBehaviour
{
    public GameObject aircraftPrefab;

    int totalWaypointsCount;

    public int spawnedAircraftsLimit = 2;

    private int aircraftsCount;
    private float nextTimeToCheck_AACount;
    readonly float checkRate_AACount = 1f;

    public float instantiateRate = 5f;
    float nextTimeToInstantiate;

    Waypoints[] allWaypointsArray;
    private readonly List<Waypoints> availableWaypointsList = new List<Waypoints>();

    Transform myTransform;


    private void Awake()
    {        
        myTransform = transform;
        totalWaypointsCount = myTransform.childCount;

        LoadWaypointsList();
    }

    void Update()
    {
        UpdateAvailableWaypoints();

        RegulateAircraftsInstantiation();
    }

    void LoadWaypointsList()
    {
        allWaypointsArray = new Waypoints[totalWaypointsCount];

        for (int i = 0; i < totalWaypointsCount; i++)
        {
            allWaypointsArray[i] = myTransform.GetChild(i).GetComponent<Waypoints>();
        }
    }

    void UpdateAvailableWaypoints()
    {
        for (int i = 0; i < totalWaypointsCount; i++)
        {
            if (allWaypointsArray[i].isAvailable && !availableWaypointsList.Contains(allWaypointsArray[i]))
                availableWaypointsList.Add(allWaypointsArray[i]);

            if (!allWaypointsArray[i].isAvailable && availableWaypointsList.Contains(allWaypointsArray[i]))
                availableWaypointsList.Remove(allWaypointsArray[i]);
        }
    }

    void CheckSpawnedAircraftCount()
    {
        if (Time.time > nextTimeToCheck_AACount)
        {
            nextTimeToCheck_AACount = checkRate_AACount + Time.time;
            aircraftsCount = GameObject.FindGameObjectsWithTag("Airplane").Length;

        }
    }

    void RegulateAircraftsInstantiation()
    {
        CheckSpawnedAircraftCount();

        if (aircraftsCount >= spawnedAircraftsLimit)
            return;

        for (int i = 0; (i < allWaypointsArray.Length) && (Time.time > nextTimeToInstantiate); i++)
        {
            nextTimeToInstantiate = instantiateRate + Time.time;

            InstantiateAircraft();
        }
    }

    void InstantiateAircraft()
    {
        if (availableWaypointsList.Count == 0)
            return;

        var availableWaypoint = availableWaypointsList[Random.Range(0, availableWaypointsList.Count)];

        Transform aircraftTransform = Instantiate(aircraftPrefab, availableWaypoint.transform.GetChild(0).position, Quaternion.identity).transform;

        aircraftTransform.GetComponent<AircraftAI>().waypoints = availableWaypoint;
    }
}