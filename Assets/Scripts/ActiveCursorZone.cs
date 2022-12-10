using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCursorZone : MonoBehaviour
{
    static Texture2D cursor;
    static Vector2 hotspot = new Vector2(0,0);

    static AudioSource audio;
    static AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        if(!cursor)
        {
            cursor = (Texture2D)Resources.Load("active_cursor");
        }

        if (!audio)
        {
            audio = gameObject.AddComponent<AudioSource>();
            clip = (AudioClip)Resources.Load("Sounds/UI/Click");
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

    private void OnMouseUp()
    {
        audio.PlayOneShot(clip, SoundManager.soundVolume);
    }
}
