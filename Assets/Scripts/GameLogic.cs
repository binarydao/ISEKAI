using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    //private static long innerHearts = 0;
    public static long HeartsByClick = 1;
    public static long HeartsBySec = 0;
    public static long HeartsBy10Sec = 0;

    float timeLeft = 1f;
    int tenSecondsCounter = 0;

    public static int level = 1;
    public static int l_maxLevel = 1;
    //public static long maxHearts = 0;
    public static int stage = 1;

    private static string l_currentlyPlayingClip = "";
    public static bool isProgressMode = true;

    private static SaveFile saveFile = new SaveFile();

    public static bool isSaveGame = false;

    //hearts
    private static long heartsProgressAll;
    private static long heartsFreeAll;
    private static long heartsProgressCurrency;
    private static long heartsFreeCurrency;

    public static long statClicks = 0;
    
    public static long statHeartsClicks = 0;
    public static long statHeartsSec = 0;
    public static long statHearts10Sec = 0;
    public static long statHeartsSpent = 0;
    public static int freeLevel;

    public static bool wasExit = false;

    private static Button freeNextButton;

    // Start is called before the first frame update
    void Start()
    {
        if(!isSaveGame)
        {
            LoadGame();
        }

        freeNextButton = GameObject.Find("FreeNextButton").GetComponent<Button>();

        Restart();
    }

    void Restart()
    {
        freeNextButton.gameObject.SetActive(false);
        if (isProgressMode)
        {
            level = getMaxLevel();
            if(level > 10)
            {
                ResetLastLevel();
            }
        }
        else
        {
            //setMaxLevel(level);
            level = freeLevel;
            if(level == 1)
            {
                heartsFreeAll = 0;
            }
            else if(heartsFreeAll < ProgressBarClass.WinValue[level - 2])
            {
                heartsFreeAll = ProgressBarClass.WinValue[level - 2];
            }
            /*if (level == 1)
            {
                heartsProgressAll = 0;
                heartsProgressCurrency = 0;
            }
            else
            {
                heartsFreeAll = ProgressBarClass.WinValue[level - 2];
                //heartsProgressCurrency = ProgressBarClass.WinValue[level - 2];
            }*/
        }

        CountersUpdate(true);
        PricesUpdate();
        
        ProgressBarClass.setLevel(level);
        ProgressBarClass.ChangeMovieIfNeeded(true);
    }

    internal static void resetHeartsForFreeLevel()
    {
        if (level == 1)
        {
            heartsFreeAll = 0;
        }
        else
        {
            heartsFreeAll = ProgressBarClass.WinValue[level - 2];
        }
    }

    private void PricesUpdate()
    {
        for(int i=0; i<=11; i++)
            BonusPurchaise.UpdateBonusCaption(i);
    }


    /*public static void setHearts(long value, bool isCurrency)
    {
        if(innerHearts < value)
        {
            statHeartsAll += (value - innerHearts);
        }
        innerHearts = value;


        for (int i = 0; i <= 11; i++)
            BonusPurchaise.UpdateRedDot(i);
    }*/

    public static void addHearts(long change)
    {
        if(isProgressMode)
        {
            heartsProgressAll += change;
            heartsProgressCurrency += change;
        }
        else
        {
            heartsFreeAll += change;
            heartsFreeCurrency += change;
        }

        for (int i = 0; i <= 11; i++)
            BonusPurchaise.UpdateRedDot(i);

        CheckHeartsAchievements();
    }

    private static void CheckHeartsAchievements()
    {
        Debug.Log("getHeartsStat(): " + getHeartsStat());
        if(getHeartsStat() >= 10)
        {
            SteamWrapper.UnlockAchievement(12);
        }
        if (getHeartsStat() >= 20)
        {
            SteamWrapper.UnlockAchievement(13);
        }
        if (getHeartsStat() >= 40)
        {
            SteamWrapper.UnlockAchievement(14);
        }
        if (getHeartsStat() >= 80)
        {
            SteamWrapper.UnlockAchievement(15);
        }
        if (getHeartsStat() >= 160)
        {
            SteamWrapper.UnlockAchievement(16);
        }
        if (getHeartsStat() >= 320)
        {
            SteamWrapper.UnlockAchievement(17);
        }
        if (getHeartsStat() >= 640)
        {
            SteamWrapper.UnlockAchievement(18);
        }
        if (getHeartsStat() >= 1280)
        {
            SteamWrapper.UnlockAchievement(19);
        }
        if (getHeartsStat() >= 2500)
        {
            SteamWrapper.UnlockAchievement(20);
        }
        if (getHeartsStat() >= 5000)
        {
            SteamWrapper.UnlockAchievement(21);
        }
        if (getHeartsStat() >= 10000)
        {
            SteamWrapper.UnlockAchievement(22);
        }
        if (getHeartsStat() >= 20000)
        {
            SteamWrapper.UnlockAchievement(23);
        }
        if (getHeartsStat() >= 40000)
        {
            SteamWrapper.UnlockAchievement(24);
        }
        if (getHeartsStat() >= 80000)
        {
            SteamWrapper.UnlockAchievement(25);
        }
        if (getHeartsStat() >= 160000)
        {
            SteamWrapper.UnlockAchievement(26);
        }
        if (getHeartsStat() >= 320000)
        {
            SteamWrapper.UnlockAchievement(27);
        }
        if (getHeartsStat() >= 640000)
        {
            SteamWrapper.UnlockAchievement(28);
        }
        if (getHeartsStat() >= 1280000)
        {
            SteamWrapper.UnlockAchievement(29);
        }
        if (getHeartsStat() >= 2500000)
        {
            SteamWrapper.UnlockAchievement(30);
        }
        if (getHeartsStat() >= 5000000)
        {
            SteamWrapper.UnlockAchievement(31);
        }
        if (getHeartsStat() >= 10000000)
        {
            SteamWrapper.UnlockAchievement(32);
        }
        if (getHeartsStat() >= 20000000)
        {
            SteamWrapper.UnlockAchievement(33);
        }
        if (getHeartsStat() >= 40000000)
        {
            SteamWrapper.UnlockAchievement(34);
        }
        if (getHeartsStat() >= 80000000)
        {
            SteamWrapper.UnlockAchievement(35);
        }
        if (getHeartsStat() >= 160000000)
        {
            SteamWrapper.UnlockAchievement(36);
        }
        if (getHeartsStat() >= 320000000)
        {
            SteamWrapper.UnlockAchievement(37);
        }
        if (getHeartsStat() >= 640000000)
        {
            SteamWrapper.UnlockAchievement(38);
        }
        if (getHeartsStat() >= 1280000000)
        {
            SteamWrapper.UnlockAchievement(39);
        }
        if (getHeartsStat() >= 2500000000)
        {
            SteamWrapper.UnlockAchievement(40);
        }
        if (getHeartsStat() >= 5000000000)
        {
            SteamWrapper.UnlockAchievement(41);
        }
        if (getHeartsStat() >= 10000000000)
        {
            SteamWrapper.UnlockAchievement(42);
        }
    }

    public static void spendHearts(long change)
    {
        if (isProgressMode)
        {
            heartsProgressCurrency -= change;
        }
        else
        {
            heartsFreeCurrency -= change;
        }

        for (int i = 0; i <= 11; i++)
            BonusPurchaise.UpdateRedDot(i);
    }

    public static string getCurrentlyPlayingClip()
    {
        return l_currentlyPlayingClip;
    }

    public static void setCurrentlyPlayingClip(string value)
    {
        //Debug.Log("setCurrentlyPlayingClip value: " + value);
        l_currentlyPlayingClip = value;
    }

    public static long getHearts(bool isCurrency)
    {
        //return innerHearts;
        if (isCurrency)
        {
            if(isProgressMode)
            {
                return heartsProgressCurrency;
            }
            else
            {
                return heartsFreeCurrency;
            }
        }
        else
        {
            if (isProgressMode)
            {
                return heartsProgressAll;
            }
            else
            {
                return heartsFreeAll;
            }
        }
    }

    public static long getHeartsStat()
    {
        if(getHearts(true)> heartsProgressAll && getHearts(true) > heartsFreeAll)
        {
            return getHearts(true);
        }
        if(heartsProgressAll > heartsFreeAll)
        {
            return heartsProgressAll;
        }
        return heartsFreeAll;
    }

    internal static void DoubleHearts()
    {
        if (getHearts(true) == 0)
        {
            addHearts(1);
        }
        else
        {
            addHearts(getHearts(true));
        }
        CountersUpdate(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(wasExit) //same as start
        {
            wasExit = false;
            Restart();
        }

        timeLeft -= Time.deltaTime;
        if(timeLeft<=0)
        {
            timeLeft++;
            tenSecondsCounter++;
            addHearts(HeartsBySec);
            statHeartsSec += HeartsBySec;
            if (tenSecondsCounter%10==0)
            {
                tenSecondsCounter = 0;
                statHearts10Sec += HeartsBy10Sec;
                addHearts(HeartsBy10Sec);
                SaveGame();
            }
            CountersUpdate(false);
        }
    }
    
    internal static void showNextLevelButton()
    {
        //Debug.Log("showNextLevelButton");
        freeNextButton.gameObject.SetActive(true);
    }

    static public int getMaxLevel()
    {
        return l_maxLevel;
    }

    static public void setMaxLevel(int value)
    {
        if(value > l_maxLevel)
        {
            l_maxLevel = value;
        }
    }


    static public void CountersUpdate(bool isStartUpdate)
    {
        GameObject.Find("HeartsText").GetComponent<Text>().text = "    :\r\n" + Shortener7(getHearts(true));
        GameObject.Find("HeartsToClickText").GetComponent<Text>().text = "    " + Localization.l("UI_ClicksSlash") + "\r\n" + Shortener(HeartsByClick);
        GameObject.Find("HeartsToSecondText").GetComponent<Text>().text = "    " + Localization.l("UI_Sec") + "\r\n" + Shortener(HeartsBySec);
        GameObject.Find("HeartsTo10SecondsText").GetComponent<Text>().text = "    " + Localization.l("UI_10Sec") + "\r\n" + Shortener(HeartsBy10Sec);
        if (isProgressMode)
        {
            GameObject.Find("ModeText").GetComponent<Text>().text = Localization.l("UI_ProgressMode");
        }
        else
        {
            GameObject.Find("ModeText").GetComponent<Text>().text = Localization.l("UI_FreeMode");
        }
        ProgressBarClass.SetProgress(getHearts(false), isStartUpdate);
    }

    static public string Shortener(long num)
    {
        if (num < 9999)
        {
            return num.ToString();
        } else if (num < 999999)
        {
            return (num / 1000).ToString() + "k";
        } else if (num < 999999999)
        {
            return (num / 1000000).ToString() + "M";
        }
        else return (num / 1000000000).ToString() + "B";
    }

    static public string Shortener7(long num)
    {
        if (num < 9999999)
        {
            return num.ToString();
        }
        else if (num < 999999999)
        {
            return (num / 1000000).ToString() + "M";
        }
        else if (num < 999999999999)
        {
            return (num / 1000000000).ToString() + "B";
        }
        else return (num / 1000000000000).ToString() + "T";
    }


    public static void SaveGame()
    {
        isSaveGame = true;
        saveFile.BonusBought = BonusPurchaise.BonusBought;

        if (getMaxLevel() > level)
        {
            saveFile.maxLevel = getMaxLevel();
        }
        else
        {
            saveFile.maxLevel = level;
        }

        saveFile.heartsProgressAll = heartsProgressAll;
        saveFile.heartsFreeAll = heartsFreeAll;
        saveFile.heartsProgressCurrency = heartsProgressCurrency;
        saveFile.heartsFreeCurrency = heartsFreeCurrency;

        saveFile.heartsPerClick = HeartsByClick;
        saveFile.heartsPerSec = HeartsBySec;
        saveFile.heartsPer10Sec = HeartsBy10Sec;
        saveFile.statClicks = statClicks;
        saveFile.statHeartsClicks = statHeartsClicks;
        saveFile.statHeartsSec = statHeartsSec;
        saveFile.statHearts10Sec = statHearts10Sec;
        saveFile.statHeartsSpent = statHeartsSpent;

        SaveLoadFile.Save(saveFile, SaveLoadFile.SAVEFILE);
    }

    public static void LoadGame()
    {
        isSaveGame = true;
               
        if (SaveLoadFile.Load(SaveLoadFile.SAVEFILE) != null)
        {
            saveFile = SaveLoadFile.Load(SaveLoadFile.SAVEFILE) as SaveFile;
            BonusPurchaise.BonusBought = saveFile.BonusBought;
            setMaxLevel(saveFile.maxLevel);

            Debug.Log("Loaded maxlevel: " + saveFile.maxLevel);

            HeartsByClick = saveFile.heartsPerClick;
            HeartsBySec = saveFile.heartsPerSec;
            HeartsBy10Sec= saveFile.heartsPer10Sec;
            statClicks = saveFile.statClicks;
            statHeartsClicks = saveFile.statHeartsClicks;
            statHeartsSec = saveFile.statHeartsSec;
            statHearts10Sec = saveFile.statHearts10Sec;
            statHeartsSpent = saveFile.statHeartsSpent;

            heartsProgressAll = saveFile.heartsProgressAll;
            heartsFreeAll = saveFile.heartsFreeAll;
            heartsProgressCurrency = saveFile.heartsProgressCurrency;
            heartsFreeCurrency = saveFile.heartsFreeCurrency;

            Debug.Log("heartsProgressAll: " + heartsProgressAll);
            Debug.Log("heartsFreeAll: " + heartsFreeAll);
            Debug.Log("heartsProgressCurrency: " + heartsProgressCurrency);
            Debug.Log("heartsFreeCurrency: " + heartsFreeCurrency);

            if (getMaxLevel() > 10 || level > 10 || getHearts(false)>= ProgressBarClass.WinValue[9])
            {
                Debug.Log("reset last level?");
                ResetLastLevel();
            }
        }
    }

    public static void ResetLastLevel()
    {
        setMaxLevel(11);
        level = 10;
        heartsProgressAll = ProgressBarClass.WinValue[8];
        heartsFreeAll = ProgressBarClass.WinValue[8];
    }

    internal static void clearProgress()
    {
        for(var i=0; i<12; i++)
        {
            BonusPurchaise.BonusBought[i] = 0;
        }
        l_maxLevel = 1;
        heartsProgressAll = 0;
        heartsFreeAll = 0;
        heartsProgressCurrency = 0;
        heartsFreeCurrency = 0;
        HeartsByClick = 1;
        HeartsBySec = 0;
        HeartsBy10Sec = 0;
        statClicks = 0;
        statHeartsClicks = 0;
        statHeartsSec = 0;
        statHearts10Sec = 0;
        statHeartsSpent = 0;
    }
}
