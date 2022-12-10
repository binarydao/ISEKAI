using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GalleryLeftButton : MonoBehaviour
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
        VideoPlayer VP = GameObject.Find("VideoPlayer").GetComponent<VideoPlayer>();
        VideoClip clip = Resources.Load<VideoClip>(GalleryController.getPreviousVideoName()) as VideoClip;
        VP.clip = clip;
        VP.Play();
    }
}
