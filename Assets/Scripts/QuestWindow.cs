using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestWindow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void CloseWindow()
    {
        SceneManager.UnloadSceneAsync("QuestWindow");
        MapLogic.isQuestWindow = false;
    }
}
