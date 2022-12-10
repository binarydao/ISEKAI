using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Debug.Log("BaseClick");
        SceneManager.LoadScene("BaseScene", LoadSceneMode.Single);
    }

public void CampaignClick()
    {
        Debug.Log("CampaignClick");
        SceneManager.LoadScene("CampaignScene", LoadSceneMode.Single);
    }

public void M3Click()
{
    Debug.Log("M3Click");
    SceneManager.LoadScene("M3Scene", LoadSceneMode.Single);
}

public void QuestsClick()
{
    Debug.Log("QuestsClick");
    SceneManager.LoadScene("QuestsScene", LoadSceneMode.Single);
}

public void ArenaClick()
{
    Debug.Log("ArenaClick");
    SceneManager.LoadScene("ArenaScene", LoadSceneMode.Single);
}

public void BarracksClick()
{
    Debug.Log("BarracksClick");
    SceneManager.LoadScene("BarracksScene", LoadSceneMode.Single);
}

public void HireClick()
{
    Debug.Log("HireClick");
    SceneManager.LoadScene("HireScene", LoadSceneMode.Single);
}

public void InventoryClick()
{
    Debug.Log("InventoryClick");
    SceneManager.LoadScene("InventoryScene", LoadSceneMode.Single);
}

public void BankClick()
{
    Debug.Log("BankClick");
    SceneManager.LoadScene("BankScene", LoadSceneMode.Single);
}

public void ArenaShopClick()
{
    Debug.Log("ArenaShopClick");
    SceneManager.LoadScene("ArenaShopScene", LoadSceneMode.Single);
}
}
