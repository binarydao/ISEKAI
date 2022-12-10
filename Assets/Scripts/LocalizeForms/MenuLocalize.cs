using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLocalize : MonoBehaviour
{
    static GameObject ClickImage;
    static GameObject MoveImage;
    static GameObject ClickCaption;
    static GameObject MoveCaption;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += MyFunction;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void StartLoc()
    {
        if(SceneManager.GetActiveScene().name != "MenuScene")
        {
            return;
        }
        GameObject.Find("GameNameCaption").GetComponent<Text>().text = Localization.l("UI_ArtBy");
        GameObject.Find("PlayButtonText").GetComponent<Text>().text = Localization.l("UI_Play");
        GameObject.Find("SettingsButtonText").GetComponent<Text>().text = Localization.l("UI_SettingsButton");
        GameObject.Find("DLCButtonText").GetComponent<Text>().text = Localization.l("UI_DLC");
        GameObject.Find("GalleryButtonText").GetComponent<Text>().text = Localization.l("UI_GalleryButton");
        GameObject.Find("StatsButtonText").GetComponent<Text>().text = Localization.l("UI_StatsButton");
        GameObject.Find("ClickCaption").GetComponent<Text>().text = Localization.l("UI_Click");
        GameObject.Find("MoveCaption").GetComponent<Text>().text = Localization.l("UI_Move");

        /*
        ClickImage = GameObject.Find("ClickImage");
        MoveImage = GameObject.Find("MoveImage");
        ClickCaption = GameObject.Find("ClickCaption");
        MoveCaption = GameObject.Find("MoveCaption");
        Color invisibleColor = Color.clear;
        ClickImage.GetComponent<Image>().color = invisibleColor;
        MoveImage.GetComponent<Image>().color = invisibleColor;
        ClickCaption.GetComponent<Text>().text = "";
        MoveCaption.GetComponent<Text>().text = "";*/
    }

    void MyFunction<Scene>(Scene scene, LoadSceneMode lsm)
    {
        StartLoc();
    }
}
