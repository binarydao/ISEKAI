using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BonusPurchaise : MonoBehaviour
{
    
    static Image overlay;
    static Text overlayText;

    private static Image[] redDot = new Image[12];
    private static Image[] doorLock = new Image[12];

    //private static bool linksPopulated = false;


    static int[,] BonusPrices = { 
        {10,        15,         23,         35,         53}, 
        {40,        60,         90,         135,        203}, 
        {160,       240,        360,        540,        810},  
        {640,       960,        1440,       2160,       3240},        
        {2560,      3840,       5760,       8640,       12960},       
        {10240,     15360,      23040,      34560,      51840},        
        {40960,     61440,      92160,      138240,     207360},        
        {163840,    245760,     368640,     552960,     829440},        
        {655360,    983040,     1474560,    2211840,    3317760},       
        {2621440,   3932160,    5898240,    8847360,    13271040},        
        {10485760,  15728640,   23592960,   35389440,   53084160},        
        {41943040,  62914560,   94371840,   141557760,  212336640}    };

    static int[] BonusPower = { 1, 1, 20, 10, 10, 200, 100, 100, 2000, 1000, 1000, 20000 };

    static public int[] BonusBought = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

   /* static String[] BonusCaption = {
        "Strong hands",
        "Dildo",
        "Lubricant",
        "Vibro cap",
        "Vibrator",
        "Anal lubricant",
        "Aphrodisiac",
        "Anal beads",
        "Clitoral lubricant",
        "Vacuum toy",
        "Double dildo",
        "Kegel exercises"};*/

    static int clickAdd = 1;
    static int heartEachSec = 2;
    static int heart10Sec = 3;

    static int[] BonusTypes = { 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3};

    static AudioSource audio;
    static AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        overlay = GameObject.Find("OverlayBlock").GetComponent<Image>();
        overlayText = GameObject.Find("OverlayText").GetComponent<Text>();

        int id = Convert.ToInt32(this.gameObject.name.Substring(11));

        if(!audio)
        {
            audio = gameObject.AddComponent<AudioSource>();
            clip = (AudioClip)Resources.Load("Sounds/UI/Upgrade");
        }
    }

    private static void populateLinks()
    {
        //linksPopulated = true;

        for(var id=0; id<=11; id++)
        {
            redDot[id] = GameObject.Find("BonusDot" + id).GetComponent<Image>();
            //redDot[id].enabled = false;

            doorLock[id] = GameObject.Find("Lock" + id).GetComponent<Image>();
            //doorLock[id].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnMouseUp()
    {
        int id = Convert.ToInt32(this.gameObject.name.Substring(11));

        if (BonusBought[id] >= 5)
        {
            Debug.Log("Enough is enough");
            return;
        }

        if(!isOpened(id))
        {
            Debug.Log("Locked");
            return;
        }

        int price = BonusPrices[id, BonusBought[id]];
        if (price > GameLogic.getHearts(true))
        {
            Debug.Log("Not enough hearts, need: " + price);
            return;
        }
        
        GameLogic.spendHearts(price);
        GameLogic.statHeartsSpent += price;

        

        if (BonusTypes[id] == clickAdd)
        {
            GameLogic.HeartsByClick += BonusPower[id];
        }
        else if (BonusTypes[id] == heart10Sec)
        {
            GameLogic.HeartsBy10Sec += BonusPower[id];
        }
        else if (BonusTypes[id] == heartEachSec)
        {
            GameLogic.HeartsBySec += BonusPower[id];
        }

        audio.PlayOneShot(clip, SoundManager.soundVolume);

        BonusBought[id]++;
        UpdateBonusCaption(id);
        showAndRefreshOverlay();
        if (BonusBought[id] == 1 && id<11) 
        {
            UpdateBonusCaption(id + 1);
        }

        GameLogic.CountersUpdate(false);
    }

    public static void UpdateBonusCaption(int id)
    {
        if (!doorLock[id])
        {
            populateLinks();
        }

        GameObject gos;
        gos = GameObject.Find("BonusText" + id);

        UpdateRedDot(id);

        String effectString = "";

        String priceString = "";

        if(isOpened(id))
        {
            if (BonusBought[id] < 5)
            {
                if (BonusTypes[id] == clickAdd)
                {
                    effectString = "+" + BonusPower[id] + Localization.l("UI_HearTlick");
                }
                else if (BonusTypes[id] == heart10Sec)
                {
                    effectString = "+" + BonusPower[id] + Localization.l("UI_Heart10Sec");
                }
                else if (BonusTypes[id] == heartEachSec)
                {
                    effectString = "+" + BonusPower[id] + Localization.l("UI_HeartSec");
                }

                priceString = GameLogic.Shortener(BonusPrices[id, BonusBought[id]]) + "♥";

            }
            else
            {
                priceString = Localization.l("UI_FULL");
            }

            if (BonusBought[id] < 5)
            {
                gos.GetComponent<UnityEngine.UI.Text>().text = Localization.l("Bonus" + (id+1)) + " X" + BonusBought[id] + " " + priceString;
            }
            else
            {
                gos.GetComponent<UnityEngine.UI.Text>().text = Localization.l("Bonus" + (id + 1))  + " " + Localization.l("UI_FULL");
            }
                
            //if(doorLock[id])
                doorLock[id].enabled = false;
        } else
        {
            gos.GetComponent<UnityEngine.UI.Text>().text = "";
            //if (doorLock[id])
                doorLock[id].enabled = true;
        }
    }

    static public void UpdateRedDot(int id)
    {
        Boolean bisOpened = isOpened(id);
        Boolean notFull = BonusBought[id] < 5;
        Boolean enoughHearts = notFull && (BonusPrices[id, BonusBought[id]] <= GameLogic.getHearts(true));

        if (redDot[id])
            redDot[id].enabled = bisOpened && enoughHearts && notFull;
    }

    void OnMouseEnter()
    {
        showAndRefreshOverlay();
    }

    void showAndRefreshOverlay()
    {
        overlay.enabled = true;
        overlayText.enabled = true;
        int id = Convert.ToInt32(this.gameObject.name.Substring(11));

        String effectString = "";

        //String priceString = "";
        
        if (BonusTypes[id] == clickAdd)
        {
            effectString = "+" + BonusPower[id] + Localization.l("UI_HearTlick");
        }
        else if (BonusTypes[id] == heart10Sec)
        {
            effectString = "+" + BonusPower[id] + Localization.l("UI_Heart10Sec");
        }
        else if (BonusTypes[id] == heartEachSec)
        {
            effectString = "+" + BonusPower[id] + Localization.l("UI_HeartSec");
        }

        /*if (BonusBought[id] < 5)
        {
            priceString = GameLogic.Shortener(BonusPrices[id, BonusBought[id]]) + " ♥";
        }
        else
        {
            priceString = "FULL";
        }*/

        Vector2 newPosition = GetComponent<Image>().rectTransform.anchoredPosition;
        newPosition.x = 530;
        newPosition.y -= 25;

        overlay.rectTransform.anchoredPosition = newPosition;

        if (isOpened(id))
        {
            //overlayText.text = effectString + "\r\n" + priceString;
            overlayText.text = effectString;
        }
        else
        {
            overlayText.text = Localization.l("UI_UnlockByBuying");
        }
    }

    void OnMouseExit()
    {
        overlay.enabled = false;
        overlayText.enabled = false;
    }

    static bool isOpened(int id)
    {
        if (id == 0)
        {
            return true;
        }

        return BonusBought[id - 1] > 0;
    }
}
