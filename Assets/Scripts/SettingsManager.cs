using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class SettingsManager : MonoBehaviour
{
    private static int language = 0; //0 - English, 1 - Russian

    public static PreferenceSave preferenceSave = new PreferenceSave();

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("MusicSlider").GetComponent<Slider>().value = SoundManager.GetMusicVolume();
        GameObject.Find("SoundsSlider").GetComponent<Slider>().value = SoundManager.soundVolume;
        //GameObject.Find("LanguageDropdown").GetComponent<Dropdown>().value = language;

        GameObject.Find("Language").GetComponent<Text>().text = GetSteamLanguage().ToUpper();
    }

    public static void loadSettings()
    {
        if (SaveLoadFile.Load(SaveLoadFile.CONFIGFILE) != null)
        {
            preferenceSave = SaveLoadFile.Load(SaveLoadFile.CONFIGFILE) as PreferenceSave;

            SoundManager.SetMusicVolume(preferenceSave.musicVolume);
            SoundManager.soundVolume = preferenceSave.soundsVolume;
            language = preferenceSave.language;
        }
    }

    private string GetSteamLanguage()
    {
        //return "lol";
        return SteamWrapper.GetCurrentGameLanguage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SavePreferences()
    {
        preferenceSave.musicVolume = SoundManager.GetMusicVolume();
        preferenceSave.soundsVolume = SoundManager.soundVolume;
        preferenceSave.language = language;

        SaveLoadFile.Save(preferenceSave, SaveLoadFile.CONFIGFILE);
    }

    public void LanguageChanged(Int32 value)
    {
        language = value;
        SavePreferences();
    }

    public void MusicVolumeChanged(Single value)
    {
        SoundManager.SetMusicVolume(value);
        SavePreferences();
    }

    public void SoundsVolumeChanged(Single value)
    {
        AudioSource TestAudioSource = GameObject.Find("TestAudioSource").GetComponent<AudioSource>();
        TestAudioSource.volume = value;
        TestAudioSource.Play();
        SoundManager.soundVolume = value;
        SavePreferences();
    }
}
