using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesStructure : MonoBehaviour
{
    public static int[] LocationEnemies = new int[] 
    {  0, //0
        0,
        0,
        4,
        3,
        0, //5
        0,
        0,
        0,
        5,
        6}; //10

    public static int[][] WaysEnemies = new int[][] {
        new int [] {0,0,0,0,0,0,0,0,0,0,0 }, //0
        new int [] {0,0,8,0,0,0,0,0,0,0,0 },
        new int [] {0,8,0,0,0,0,0,0,7,0,0 },
        new int [] {0,0,0,0,0,0,0,0,0,0,0 },
        new int [] {0,0,0,0,0,0,0,7,0,0,0 },
        new int [] {0,0,0,0,0,0,2,0,0,0,0 }, //5
        new int [] {0,0,0,0,0,2,0,0,0,0,0 },
        new int [] {0,0,0,0,7,0,0,0,0,0,0 },
        new int [] {0,0,7,0,0,0,0,0,0,0,10 },
        new int [] {0,0,0,0,0,0,0,0,0,0,0 },
        new int [] {0,0,0,0,0,0,0,0,10,0,0 }, //10
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
