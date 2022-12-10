using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayLocalize : MonoBehaviour
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
        if (SceneManager.GetActiveScene().name != "GameplayScene")
        {
            return;
        }
        GameObject.Find("BonusCaption").GetComponent<Text>().text = Localization.l("UI_Bonuses");
        GameObject.Find("Loading").GetComponent<Text>().text = Localization.l("UI_Loading");
        GameObject.Find("ClickMe").GetComponent<Text>().text = Localization.l("UI_ClickMe");
        GameObject.Find("LevelCompleteText").GetComponent<Text>().text = Localization.l("UI_LevelComplete");

        GameObject.Find("GameCompleteDetail").GetComponent<Text>().text = Localization.l("UI_Thanks");
        GameObject.Find("GameCompleteOkText").GetComponent<Text>().text = Localization.l("UI_Ok");
        GameObject.Find("GameCompleteText").GetComponent<Text>().text = Localization.l("UI_YouWin");
    }

    void MyFunction<Scene>(Scene scene, LoadSceneMode lsm)
    {
        StartLoc();
    }
}
