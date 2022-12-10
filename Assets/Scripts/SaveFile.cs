[System.Serializable]
public class SaveFile
{
    public int[] BonusBought = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int maxLevel = 1;

    public long heartsProgressAll;
    public long heartsFreeAll;
    public long heartsProgressCurrency;
    public long heartsFreeCurrency;

    public long heartsPerClick = 0;
    public long heartsPerSec = 0;
    public long heartsPer10Sec = 0;

    public long statClicks = 0;
    public long statHeartsClicks = 0;
    public long statHeartsSec = 0;
    public long statHearts10Sec = 0;
    public long statHeartsSpent = 0;
}
