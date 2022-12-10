using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToModeMenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            BackToModeMenu();
        }
    }

    void OnMouseUp()
    {
        BackToModeMenu();
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    void BackToModeMenu()
    {
        GameLogic.SaveGame();
        SceneManager.LoadScene("ModeScene");
        GameLogic.wasExit = true;
        GameLogic.setCurrentlyPlayingClip("");
    }
}
