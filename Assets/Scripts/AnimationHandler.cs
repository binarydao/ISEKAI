using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AnimationHandler : MonoBehaviour
{

    private int currentState = 1;

    private static int currentGirl = 0;
    private static float previousTime;
    private static readonly int MAX_GIRL = 20;
    private static readonly int TIME_BETWEEN_STATES = 2;

    // Start is called before the first frame update
    void Start()
    {
        currentGirl++;
        if (currentGirl > MAX_GIRL)
        {
            currentGirl = 1;
        }
        VideoUpdate();
        previousTime = Time.time;
}

    // Update is called once per frame
    void Update()
    {
        if(Time.time - previousTime > TIME_BETWEEN_STATES)
        {

            NextState();
            previousTime = Time.time;
        }
    }        

    private void NextState()
    {
        currentState++;
        if(currentState > 3)
        {
            currentState = 1;
        }
        VideoUpdate();
    }

    private void VideoUpdate()
    {
        VideoPlayer videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
        string videoString = "video/video" + currentGirl + "_" + currentState + ".mp4";
        Debug.Log("VideoUpdate, video: " + videoString);
        videoPlayer.url = videoString;
    }
}
