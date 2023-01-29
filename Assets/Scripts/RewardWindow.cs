using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardWindow : MonoBehaviour
{
    private static bool IsWin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Win()
    {
        IsWin = true;
        
    }


    public static void Lose()
    {
        IsWin = false;

    }

    public static void CloseWindow()
    {
        SceneManager.UnloadSceneAsync("M3Scene");
        MapLogic.Holder.SetActive(true);
        if (IsWin)
        {
            MapLogic.WinAndFinishHalfwayMove();
        }
        else
        {
            MapLogic.ReturnHalfwayMove();
        }
    }
}
