using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopColliderScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Shop Collider Script start");
        //GameLogic.HeartsUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUp()
    {
        Debug.Log("Shop Mouse Up");
       // GameLogic.heartAdder++;
    }
}
