using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardWindow : MonoBehaviour
{
    private static bool IsWin;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        var RewardCaption = GameObject.Find("RewardCaption");
        if (IsWin)
        {
            Debug.Log("IsWin");
            RewardCaption.GetComponent<Text>().text = "YOU WIN";
        }
        else
        {
            Debug.Log("not IsWin");
            RewardCaption.GetComponent<Text>().text = "YOU LOSE";
        }

        var MoneyCount = GameObject.Find("MoneyCount");
        MoneyCount.GetComponent<Text>().text = "x " + GameBehaviour.LocalLoot.money;

        var ExpCount = GameObject.Find("ExpCount");
        ExpCount.GetComponent<Text>().text = "x " + GameBehaviour.LocalLoot.experience;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Win()
    {
        Debug.Log("Win function");
        IsWin = true;
    }

    public static void Lose()
    {
        Debug.Log("Lose function");
        IsWin = false;
    }

    public static void CloseWindow()
    {
        if (SceneManager.GetSceneByName("M3Scene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("M3Scene");
        }

        if (SceneManager.GetSceneByName("MapScene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("RewardWindow");
            MapLogic.Holder.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("MapScene");
        }
        
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
