using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ClearProgressButton : MonoBehaviour
{
    Canvas mainCanvas;
    Canvas modalCanvas;

    // Start is called before the first frame update
    void Start()
    {
        mainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
        modalCanvas = GameObject.Find("ClearProgressModalCanvas").GetComponent<Canvas>();

        //if (!File.Exists(Application.persistentDataPath + SaveLoadFile.CONFIGFILE))
        //{
        //    GameObject.Find("ClearProgress").SetActive(false);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClearClick()
    {
        mainCanvas.enabled = false;
        modalCanvas.enabled = true;
    }

    public void onCancelClick()
    {
        mainCanvas.enabled = true;
        modalCanvas.enabled = false;
    }
    
    public void deleteSaves()
    {
        File.Delete(Application.persistentDataPath + SaveLoadFile.SAVEFILE);

        GameLogic.clearProgress();

        onCancelClick();
    }



}
