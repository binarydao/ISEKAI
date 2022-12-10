using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextFreeLevelClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseUp()
    {
        GameLogic.freeLevel++;
        if (GameLogic.freeLevel > 10)
        {
            GameLogic.freeLevel = 10;
            ProgressBarClass.isGameWin = true;
            ProgressBarClass.winAnimationStarted = true;
        }
        else
        {
            SceneManager.LoadScene("GamePlayScene");
        }
    }
}
