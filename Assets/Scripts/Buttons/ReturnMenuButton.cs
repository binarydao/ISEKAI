
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMenuButton : MonoBehaviour
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
            BackToMenu();
        }
    }

    void OnMouseUp()
    {
        BackToMenu();
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
