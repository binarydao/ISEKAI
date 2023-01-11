using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStructure : MonoBehaviour
{
    public static int[][] Ways = new int[][] { 
        new int [] { }, //0
        new int [] { 4, 7, 2, 6},
        new int [] { 1, 8, 3},
        new int [] { 2},
        new int [] { 7, 1},
        new int [] { 9, 6}, //5
        new int [] { 1, 5},
        new int [] { 4, 1},
        new int [] { 2, 10},
        new int [] { 5},
        new int [] { 8}, //10
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
