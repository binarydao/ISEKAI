using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AnimationHandler : MonoBehaviour
{

    private int currentState;

    private static int currentGirl = 0;
    private static float previousTime;
    private static readonly int MAX_GIRL = 20;
    private static readonly int TIME_BETWEEN_STATES = 2;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("AnimationHandler start");
        currentGirl++;
        if (currentGirl > MAX_GIRL)
        {
            currentGirl = 1;
        }
        VideoPlayer videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
        videoPlayer.clip = (VideoClip)Resources.Load("video/video" + currentGirl + "_1");
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

    public void NextState()
    {
        currentState++;
        if(currentState > 3)
        {
            currentState = 1;
        }
        VideoPlayer videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
        videoPlayer.clip = (VideoClip)Resources.Load("video/video" + currentGirl + "_" + currentState);
    }
}
