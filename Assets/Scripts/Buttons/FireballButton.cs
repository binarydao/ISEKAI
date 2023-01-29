using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballButton : MonoBehaviour
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
        if (GameBehaviour.GameOver)
        {
            return;
        }
        Debug.Log("FireBall click");
        GameBehaviour.instance.TryFireball();
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
