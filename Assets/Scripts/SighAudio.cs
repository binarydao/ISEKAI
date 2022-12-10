using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SighAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            AudioSource SighAudioSource = GameObject.Find("SighAudioSource").GetComponent<AudioSource>();
            if (SighAudioSource)
            {
                SighAudioSource.volume = SoundManager.soundVolume;
            }
        }
        catch (Exception e)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
