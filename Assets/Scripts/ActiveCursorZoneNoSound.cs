using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCursorZoneNoSound : MonoBehaviour
{
    static Texture2D cursor;
    static Vector2 hotspot = new Vector2(0, 0);


    void Start()
    {
        if (!cursor)
        {
            cursor = (Texture2D)Resources.Load("active_cursor");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseEnter()
    {
        Cursor.SetCursor(cursor, hotspot, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
