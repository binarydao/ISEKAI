using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class GalleryController : MonoBehaviour
{
    private static List<string> availableAnimations;
    public static string currentVideo = "Girl1_1";

    // Start is called before the first frame update
    void Start()
    {
        generateAvialableArray();

        VideoPlayer VP = GameObject.Find("VideoPlayer").GetComponent<VideoPlayer>();
        VideoClip clip = Resources.Load<VideoClip>(currentVideo) as VideoClip;
        VP.clip = clip;
        VP.Play();
    }

    private void generateAvialableArray()
    {
        availableAnimations = new List<string>();

        for(int girl = 1; girl <= 10; girl ++)
            for(int state = 1; state <= 3; state++)
            {
                if (GalleryPage.isUnlocked(girl, state, true))
                {
                    availableAnimations.Add("Girl"+girl+"_"+state);
                }
                else return;
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static string getNextVideoName()
    {
        int currentIndex = availableAnimations.IndexOf(currentVideo);
        if(currentIndex < availableAnimations.Count - 1)
        {
            currentVideo = availableAnimations[currentIndex + 1];
        }
        else
        {
            currentVideo = availableAnimations[0];
        }
        return currentVideo;
    }

    public static string getPreviousVideoName()
    {
        int currentIndex = availableAnimations.IndexOf(currentVideo);
        if (currentIndex > 0)
        {
            currentVideo = availableAnimations[currentIndex - 1];
        }
        else
        {
            currentVideo = availableAnimations[availableAnimations.Count - 1];
        }
        return currentVideo;
    }
}
