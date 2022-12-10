using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ModePage : MonoBehaviour
{
    private static bool isSecondList = false;

    private VideoPlayer[] VP = new VideoPlayer[10];

    // Start is called before the first frame update
    void Start()
    {
        if (!GameLogic.isSaveGame)
        {
            GameLogic.LoadGame();
        }
        FillImages();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static void FillImages()
    {
        //RenderTexture rt;
        //VideoClip clip;
        RawImage rawImage;
        for (int i = 1; i <= 10; i++)
            {
                rawImage = GameObject.Find("Free_" + i).GetComponent<RawImage>();
                if (isUnlocked(i, true))
                {

                    rawImage.texture = Resources.Load<Texture2D>("Preview" + i + "_" + 2);
                    rawImage.GetComponent<FreeModeElement>().isClickable = true;
                }
                else
                {
                    rawImage.texture = Resources.Load<Texture2D>("plate");
                    rawImage.GetComponent<FreeModeElement>().isClickable = false;
                }
            }

        rawImage = GameObject.Find("Progress").GetComponent<RawImage>();
        int tempLevel = GameLogic.getMaxLevel();
        if(tempLevel > 10)
        {
            tempLevel = 10;
        }
        rawImage.texture = Resources.Load<Texture2D>("Preview" + tempLevel + "_" + 2);

        Text captionText = GameObject.Find("FreeModeCaption").GetComponent<Text>();
        if (isUnlocked(10, false))
        {

            captionText.text = Localization.l("UI_FreeModeDLC");
        }
        else
        {
            captionText.text = Localization.l("UI_FreeMode");
        }
    }

    public static bool isUnlocked(int girl, bool isSecondListImportant)
    {
        if(isSecondList)
        {
            return false;
        }
        return (girl <= GameLogic.getMaxLevel());
    }

    public static void ChangeList(bool isNext)
    {
        isSecondList = !isSecondList;
        FillImages();

        if (isSecondList)
        {
            RawImage dynamicImage = GameObject.Find("DynamicFree").GetComponent<RawImage>();
            dynamicImage.rectTransform.anchoredPosition = new Vector2(-5000, -5000);
        }
    }
}
