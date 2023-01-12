﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsBehaviour : MonoBehaviour {
    Text buttonCaption;

    // Use this for initialization
    void Start ()
    {
        buttonCaption = gameObject.GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BaseClick()
    {
        SceneManager.LoadScene("BaseScene", LoadSceneMode.Single);
    }

    public void LoseClick()
    {
        SceneManager.UnloadSceneAsync("M3Scene");
        MapLogic.Holder.SetActive(true);
    }

    public void WinClick()
    {
        SceneManager.UnloadSceneAsync("M3Scene");
        MapLogic.Holder.SetActive(true);
    }

    public void M3Click()
    {
        SceneManager.LoadScene("M3Scene", LoadSceneMode.Single);
    }

    public void QuestsClick()
    {
        SceneManager.LoadScene("QuestsScene", LoadSceneMode.Single);
    }

    public void BarracksClick()
    {
        SceneManager.LoadScene("BarracksScene", LoadSceneMode.Single);
    }

    public void HireClick()
    {
        SceneManager.LoadScene("HireScene", LoadSceneMode.Single);
    }

    public void InventoryClick()
    {
        SceneManager.LoadScene("InventoryScene", LoadSceneMode.Single);
    }

    public void BankClick()
    {
        SceneManager.LoadScene("BankScene", LoadSceneMode.Single);
    }

    public void ArenaShopClick()
    {
        SceneManager.LoadScene("ArenaShopScene", LoadSceneMode.Single);
    }
}
