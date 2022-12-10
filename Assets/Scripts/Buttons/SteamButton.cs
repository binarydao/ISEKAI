using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUp()
    {
        Application.OpenURL("https://store.steampowered.com/app/1630130/Lustful_Tribe");
    }
}
