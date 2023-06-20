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
        SceneManager.LoadScene("BaseScene", LoadSceneMode.Single);
    }

    public void LoseClick()
    {
        if (GameBehaviour.GameOver)
        {
            return;
        }
        GameBehaviour.Lose();

    }

    public void WinClick()
    {
        if (GameBehaviour.GameOver)
        {
            return;
        }
        GameBehaviour.Win();
       
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

    public void DamageHero()
    {
        if (GameBehaviour.GameOver)
        {
            return;
        }
        GameBehaviour.instance.DelayedEnemyAttack(5);
    }

    public void DamageEnemy()
    {
        if (GameBehaviour.GameOver)
        {
            return;
        }
        GameBehaviour.instance.HeroAttack(5);
    }

    public void AutoBattleClick()
    {
        if (GameBehaviour.GameOver)
        {
            return;
        }
        GameBehaviour.instance.ToggleAutoBattle();
    }

    public void CloseReward()
    {
        RewardWindow.CloseWindow();
    }

    public void CloseQuest()
    {
        QuestWindow.CloseWindow();
    }

    public void ShowBoobies()
    {
        MapLogic.ShowBoobies();
    }

    public void CloseGirl500()
    {
        SceneManager.UnloadSceneAsync("Girl500Window");
        MapLogic.isPopupWindow = false;
        MapLogic.WinAndFinishHalfwayMove();
    }

    public void CloseGirl1920()
    {
        SceneManager.UnloadSceneAsync("Girl1920Window");
        MapLogic.isPopupWindow = false;
        MapLogic.WinAndFinishHalfwayMove();
    }


}
