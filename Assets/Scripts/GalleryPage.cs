using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System;

public class GalleryPage : MonoBehaviour

{
    private VideoPlayer[,] VP = new VideoPlayer[10, 3];
    private static bool isSecondList = false;



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
       /* for (int i = 1; i <= 10; i++)
            for (int j = 1; j <= 3; j++)
            {
                if (VP[i - 1, j - 1].frame > 0)
                {
                    VP[i - 1, j - 1].Stop();
                }
            }*/
    }

    static void FillImages()
    {
        for (int i=1; i<=10; i++)
            for(int j=1; j<=3; j++)
            {
                RawImage rawImage = GameObject.Find("RawImage" + i + "_" + j).GetComponent<RawImage>();

                if (isUnlocked(i, j, true))
                {

                    rawImage.texture = Resources.Load<Texture2D>("Preview" + i + "_" + j);
                    rawImage.GetComponent<GalleryElement>().isClickable = true;
                }
                else
                {
                    rawImage.texture = Resources.Load<Texture2D>("plate");
                    rawImage.GetComponent<GalleryElement>().isClickable = false;
                }
            }
        if(isUnlocked(10,3, false))
        {
            Text captionText = GameObject.Find("Caption").GetComponent<Text>();
            captionText.text = "Gallery: new girls incoming in free DLC!";
        }
    }

    public static void ChangeList(bool isNext)
    {
        isSecondList = !isSecondList;
        FillImages();

        if(isSecondList)
        {
            RawImage dynamicImage = GameObject.Find("DynamicImage").GetComponent<RawImage>();
            dynamicImage.rectTransform.anchoredPosition = new Vector2(-5000, -5000);
        }
    }


    public static bool isUnlocked(int girl, int state, bool isSecondListImportant)
    {
        Debug.Log("GameLogic.getMaxLevel(): " + GameLogic.getMaxLevel());
        if(isSecondListImportant && isSecondList)
        {
            return false;
        }

        if (girl > GameLogic.getMaxLevel())
        {
            return false;
        }
            

        if (girl < GameLogic.getMaxLevel())
        {
            return true;
        }
            
        if (state == 1)
        {
            return true;
        }

        float minValue;
        float maxValue;

        if (girl == 1)
        {
            minValue = 0;
        }
        else
        {
            minValue = ProgressBarClass.WinValue[girl - 2];
        }
        maxValue = ProgressBarClass.WinValue[girl - 1];

        float third = (maxValue - minValue) / 3.0f + minValue;
        float twoThird = (maxValue - minValue) / 1.5f + minValue;

        if (state == 2)
        {
            return (GameLogic.getHeartsStat() >= third);
        }
        return (GameLogic.getHeartsStat() >= twoThird);
    }
}
