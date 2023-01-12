using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private static bool IsWayEnemyPassed;

    private static int CurrentLocationId = 1;
    private static int DestinationId = 1;

    private static Vector3 HalfwayHeroPosition;
    private static bool IsHalfwayPosition = false;

    public static GameObject Holder;

    // Start is called before the first frame update
    void Start()
    {
        HeroIcon = GameObject.Find("HeroIconPrefab");
        Holder = GameObject.Find("Holder");
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsMoving || !Holder.activeSelf)
        {
            return;
        }
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        HeroIcon.transform.position = Vector3.Lerp(StartPoint, EndPoint, fractionOfJourney);

        if(!IsWayEnemyPassed && fractionOfJourney>=0.5)
        {
            CheckWayEnemy();
        }
        else if(fractionOfJourney>=1)
        {
            IsMoving = false;
            CheckLocationEnemy();
        }
    }

    private void CheckWayEnemy()
    {
        IsWayEnemyPassed = true;
        int enemyId = EnemiesStructure.WaysEnemies[CurrentLocationId][DestinationId];
        if (enemyId > 0)
        {
            Debug.Log("Monster here: " + enemyId);

            IsHalfwayPosition = true;
            HalfwayHeroPosition = HeroIcon.transform.position;

            Holder.SetActive(false);

            SceneManager.LoadScene("M3Scene", LoadSceneMode.Additive);
        }
        else
        {   
            Debug.Log("No enemy here");
        }
    }

    private void CheckLocationEnemy()
    {
        int enemyId = EnemiesStructure.LocationEnemies[DestinationId];
        if(enemyId>0)
        {
            Debug.Log("Monster here: " + enemyId);
            
            Holder.SetActive(false);

            SceneManager.LoadScene("M3Scene", LoadSceneMode.Additive);
        }
        else
        {
            Debug.Log("No enemy here");
        }
        CurrentLocationId = DestinationId;
    }

    public static void TryMove(Vector3 finishPoint, int targetId)
    {
        if(IsMoving)
        {
            return;
        }

        if(!IsPassable(targetId))
        {
            return;
        }

        DestinationId = targetId;

        startTime = Time.time;
        StartPoint = HeroIcon.transform.position;
        EndPoint = finishPoint;

        EndPoint.z = StartPoint.z;
        journeyLength = Vector3.Distance(StartPoint, EndPoint);

        IsWayEnemyPassed = false;
        IsMoving = true;
    }

    private static bool IsPassable(int DestinationId)
    {
        return (Array.IndexOf(MapStructure.Ways[DestinationId], CurrentLocationId) > -1);
    }
}
