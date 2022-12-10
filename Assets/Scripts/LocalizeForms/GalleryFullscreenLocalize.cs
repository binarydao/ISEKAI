using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GalleryFullscreenLocalize : MonoBehaviour
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
        if (SceneManager.GetActiveScene().name != "GalleryFullscreen")
        {
            return;
        }
        GameObject.Find("LoadingText").GetComponent<Text>().text = Localization.l("UI_LoadingGallery");
    }

    void MyFunction<Scene>(Scene scene, LoadSceneMode lsm)
    {
        StartLoc();
    }
}
