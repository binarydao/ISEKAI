using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLogic : MonoBehaviour
{
    private static GameObject HeroIcon;

    // Movement speed in units per second.
    private static float speed = 5.0F;
    // Time when the movement started.
    private static float startTime;
    // Total distance between the markers.
    private static float journeyLength;

    private static Vector3 StartPoint;
    private static Vector3 EndPoint;

    private static bool IsMoving = false;

    private static int CurrentLocationId = 1;

    // Start is called before the first frame update
    void Start()
    {
        HeroIcon = GameObject.Find("HeroIconPrefab");
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsMoving)
        {
            return;
        }
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        HeroIcon.transform.position = Vector3.Lerp(StartPoint, EndPoint, fractionOfJourney);

        if(fractionOfJourney>=1)
        {
            IsMoving = false;
        }
    }

    public static void TryMove(Vector3 finishPoint, int DestinationId)
    {
        if(IsMoving)
        {
            return;
        }

        if(!IsPassable(DestinationId))
        {
            return;
        }

        CurrentLocationId = DestinationId;

        startTime = Time.time;
        StartPoint = HeroIcon.transform.position;
        EndPoint = finishPoint;

        Debug.Log("EndPoint.z: " + EndPoint.z);
        EndPoint.z = StartPoint.z;
        Debug.Log("EndPoint.z: " + EndPoint.z);

        journeyLength = Vector3.Distance(StartPoint, EndPoint);
        
        IsMoving = true;
    }

    private static bool IsPassable(int DestinationId)
    {
        return (Array.IndexOf(MapStructure.Ways[DestinationId], CurrentLocationId) > -1);
    }
}
