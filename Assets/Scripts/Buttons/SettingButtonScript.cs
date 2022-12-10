using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SettingButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUp()
    {
        SceneManager.LoadScene("SettingsScene");
    }
}
