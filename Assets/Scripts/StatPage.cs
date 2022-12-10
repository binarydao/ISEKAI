using UnityEngine;
using UnityEngine.UI;

public class StatPage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!GameLogic.isSaveGame)
        {
            GameLogic.LoadGame();
        }
        FillText();
    }

    private void FillText()
    {
        GameObject.Find("StatClicks").GetComponent<Text>().text = Localization.l("UI_Clicks") + ": " + GameLogic.statClicks.ToString();
        GameObject.Find("StatHeartsAll").GetComponent<Text>().text = Localization.l("UI_HeartsAcquired") + ": " + GameLogic.getHeartsStat();
        GameObject.Find("StatHeartsClicks").GetComponent<Text>().text = Localization.l("UI_HeartsAcquiredClicks") + ": " + GameLogic.statHeartsClicks;
        GameObject.Find("StatHeartsSec").GetComponent<Text>().text = Localization.l("UI_HeartsAcquiredSec") + ": " + GameLogic.statHeartsSec;
        GameObject.Find("StatHearts10Sec").GetComponent<Text>().text = Localization.l("UI_HeartsAcquired10Sec") + ": " + GameLogic.statHearts10Sec;
        GameObject.Find("StatHeartsSpent").GetComponent<Text>().text = Localization.l("UI_HeartsSpent") + ": " + GameLogic.statHeartsSpent;
        GameObject.Find("StatLevel").GetComponent<Text>().text = Localization.l("UI_MaxLevel") + ": " + GameLogic.getMaxLevel();
        GameObject.Find("StatBonuses").GetComponent<Text>().text = Localization.l("UI_BonusesBought") + ": " + getBonusBought();
    }

    private int getBonusBought()
    {
        int answer = 0;
        for(int i=0; i<BonusPurchaise.BonusBought.Length; i++)
        {
            answer += BonusPurchaise.BonusBought[i];
        }
        return answer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
