using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainVideoClick : MonoBehaviour
{
    private Text clickMeText;

    // Start is called before the first frame update
    void Start()
    {
        clickMeText = GameObject.Find("ClickMe").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameLogic.getHearts(false) > 0)
        {
            clickMeText.enabled = false;
        }
    }

    private void OnMouseUp()
    {
        clickMeText.enabled = false;

        GameLogic.statClicks++;

        GameLogic.statHeartsClicks += GameLogic.HeartsByClick;
        GameLogic.addHearts(GameLogic.HeartsByClick);
        GameLogic.CountersUpdate(false);

        AudioSource AC = GameObject.Find("SighAudioSource").GetComponent<AudioSource>();
        if(!AC.isPlaying)
        {
            AudioClip musicClip = SoundManager.getNextSigh(GameLogic.level, ProgressBarClass.getStage());
            AC.PlayOneShot(musicClip, SoundManager.soundVolume);
        }
    }

    public static void PlayVideo(string videoName)
    {
        //Debug.Log("PlayVideo: " + videoName);
        VideoPlayer VP = GameObject.Find("VideoPlayer").GetComponent<VideoPlayer>();
        GameLogic.setCurrentlyPlayingClip(videoName);
        VideoClip clip = Resources.Load<VideoClip>(videoName) as VideoClip;
        VP.clip = clip;
        VP.Play();
    }
}
