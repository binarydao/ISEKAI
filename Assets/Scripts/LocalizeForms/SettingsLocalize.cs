using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsLocalize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += MyFunction;
        StartLoc();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void StartLoc()
    {
        if (SceneManager.GetActiveScene().name != "SettingsScene")
        {
            return;
        }
        GameObject.Find("Caption").GetComponent<Text>().text = Localization.l("UI_SettingsCaption");
        GameObject.Find("LanguageCaption").GetComponent<Text>().text = Localization.l("UI_Language");
        GameObject.Find("MusicCaption").GetComponent<Text>().text = Localization.l("UI_Music");
        GameObject.Find("SoundCaption").GetComponent<Text>().text = Localization.l("UI_Sound");
        GameObject.Find("ClearProgressText").GetComponent<Text>().text = Localization.l("UI_ClearProgress");
        GameObject.Find("ReallyWantText").GetComponent<Text>().text = Localization.l("UI_ReallyWant");
        GameObject.Find("EraseButtonText").GetComponent<Text>().text = Localization.l("UI_Delete");
        GameObject.Find("CancelButtonText").GetComponent<Text>().text = Localization.l("UI_Cancel");
    }

    void MyFunction<Scene>(Scene scene, LoadSceneMode lsm)
    {
        StartLoc();
    }
}
