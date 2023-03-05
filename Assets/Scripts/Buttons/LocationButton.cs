using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationButton : MonoBehaviour
{
    Texture2D cursor;
    Vector2 hotspot = new Vector2(0, 6);

    // Start is called before the first frame update
    void Start()
    {
        cursor = (Texture2D)Resources.Load("hand");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUp()
    {
        int DestinationId;
        DestinationId = int.Parse(name.Substring(3));

        MapLogic.TryMove(DestinationId, false);
    }

    void OnMouseEnter()
    {
        if (MapLogic.isQuestWindow)
        {
            return;
        }
        Cursor.SetCursor(cursor, hotspot, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
