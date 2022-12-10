using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static bool isExist = false;
    private static AudioSource MusicAudioSource;
    static AudioClip musicClip;

    private static int musicCount = 7;
    private static int lastId = 0;
    private static int lastSighId = 0;

    public static float soundVolume = 1.0f;

    private static float tempVolume = 1.0f;

    //girl/stage
    static int[,] SighCount = {
        {10,     10,     10},
        {15,    29,     11},       
        {10,     20,     23}    };

    //1 - LOW
    //2 - MIDDLE
    //3 - HIGH
    public static int[] GirlPitchType = { 3, 2, 1, 2, 3, 1, 3, 2, 3, 2};
    

    // Start is called before the first frame update
    void Start()
    {
        if (!isExist)
        {
            
            DontDestroyOnLoad(this);
            PlayNextTrack();
            isExist = true;
        }
    }

    void PlayNextTrack()
    {
        SettingsManager.loadSettings();

        Debug.Log("PlayNextTrack");
        MusicAudioSource = GameObject.Find("MusicAudioSource").GetComponent<AudioSource>();
        MusicAudioSource.volume = tempVolume;
        musicClip = (AudioClip)Resources.Load("Music/Music" + getNextMusicId());
        MusicAudioSource.PlayOneShot(musicClip);
        Invoke("PlayNextTrack", musicClip.length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetMusicVolume(float value)
    {
        if(MusicAudioSource)
        {
            MusicAudioSource.volume = value;
        }
        else
        {
            tempVolume = value;
        }
    }

    public static float GetMusicVolume()
    {
        if(MusicAudioSource)
            return MusicAudioSource.volume;
        return tempVolume;
    }

    internal static AudioClip getNextSigh(int girl, int stage)
    {
        string intenseString;
        if(stage == 1)
        {
            intenseString = "1_small_intense";
        }
        else if(stage == 2)
        {
            intenseString = "2_middle_intense";
        }
        else
        {
            intenseString = "3_high_intense";
        }
        int randomSoundId = getRandomSoundId(girl, stage);
        string playingClip = "Sounds/Girl/" + GirlPitchType[girl-1]+"/"+intenseString+"/Sound" + randomSoundId;
        Debug.Log("playingClip: " + playingClip);
        AudioClip answer = (AudioClip)Resources.Load(playingClip);
        return answer;
    }

    private static int getRandomSoundId(int girl, int stage)
    {
        Debug.Log("girl: " + girl);
        Debug.Log("stage: " + stage);
        int possibleId = UnityEngine.Random.Range(0, SighCount[GirlPitchType[girl-1]-1, stage-1]) + 1;
        if (lastSighId == 0 || possibleId != lastSighId)
        {
            lastSighId = possibleId;
            Debug.Log("getRandomSoundId: " + lastSighId);
            return possibleId;
        }
        return getRandomSoundId(girl, stage);
    }

    private int getNextMusicId()
    {
        int possibleId = UnityEngine.Random.Range(0, musicCount) + 1;
        if (lastId == 0 || possibleId!= lastId)
        {
            lastId = possibleId;
            Debug.Log("getNextMusicId: " + possibleId);
            return possibleId;
        }
        return getNextMusicId();
    }
}
