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

    public static int enemyId = 1;

    public static bool IsHalfwayPosition;

    internal static void WinAndFinishHalfwayMove()
    {
        DestroyEnemy();
        TryMove(DestinationId, false);
        IsWayEnemyPassed = true;
    }

    internal static void ReturnHalfwayMove()
    {
        TryMove(CurrentLocationId, true);
        IsWayEnemyPassed = true;
    }
    
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

    private static void DestroyEnemy()
    {
        string nameString = "Enemy" + enemyId;
        GameObject enemyIcon = GameObject.Find(nameString);
        enemyIcon.SetActive(false);
        Destroy(enemyIcon);

        for(int i =0; i<EnemiesStructure.LocationEnemies.Length; i++)
        {
            if (EnemiesStructure.LocationEnemies[i] == enemyId)
            {
                EnemiesStructure.LocationEnemies[i] = 0;
            }
        }


        for (int i = 0; i < EnemiesStructure.WaysEnemies.Length; i++)
        {
            for (int j = 0; j < EnemiesStructure.WaysEnemies[i].Length; j++)
            {
                if (EnemiesStructure.WaysEnemies[i][j] == enemyId)
                {
                    EnemiesStructure.WaysEnemies[i][j] = 0;
                }
            }
        }

            //проход и удаление в межлокациях
        }

        private void CheckWayEnemy()
    {
        IsWayEnemyPassed = true;
        enemyId = EnemiesStructure.WaysEnemies[CurrentLocationId][DestinationId];
        if (enemyId > 0)
        {

            IsHalfwayPosition = true;
            IsMoving = false;

            Holder.SetActive(false);

            SceneManager.LoadSceneAsync("M3Scene", LoadSceneMode.Additive);
        }
        else
        {   
        }
    }

    private void CheckLocationEnemy()
    {
        enemyId = EnemiesStructure.LocationEnemies[DestinationId];
        if(enemyId>0)
        {
            Holder.SetActive(false);

            SceneManager.LoadScene("M3Scene", LoadSceneMode.Additive);
        }
        else
        {
            CurrentLocationId = DestinationId;
        }
        
    }

    public static void TryMove(int targetId, bool MovingBack)
    {
        GameObject locationPoint = GameObject.Find("Loc" + targetId);
        Vector3 finishPoint = locationPoint.transform.position;
        

        if (IsMoving)
        {
            return;
        }

        if(!MovingBack && !IsPassable(targetId))
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
