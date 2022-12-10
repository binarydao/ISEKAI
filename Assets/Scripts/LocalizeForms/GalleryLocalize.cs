using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GalleryLocalize : MonoBehaviour
{
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
        if (SceneManager.GetActiveScene().name != "GalleryScene")
        {
            return;
        }
        GameObject.Find("Caption").GetComponent<Text>().text = Localization.l("UI_GalleryCaption");
    }

    void MyFunction<Scene>(Scene scene, LoadSceneMode lsm)
    {
        StartLoc();
    }
}
