using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeLocalize : MonoBehaviour
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
        if (SceneManager.GetActiveScene().name != "ModeScene")
        {
            return;
        }
        GameObject.Find("ProgressModeCaption").GetComponent<Text>().text = Localization.l("UI_ProgressRecommended");
    }

    void MyFunction<Scene>(Scene scene, LoadSceneMode lsm)
    {
        StartLoc();
    }
}
