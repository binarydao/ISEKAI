using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class ProgressBarClass : MonoBehaviour
{
    private static Slider slider;
    private ParticleSystem progressBarParticles;
    private static ParticleSystem winParticleSystem;
    private static ParticleSystem gameWinParticleSystem;

    private static float targetProgress = 0;
    private static float FillSpeed = 0.6f;

    public static long[] WinValue = { 600, 3000, 24000, 192000, 1536000, 12288000, 98304000, 198304000, 906432000, 6431456000};

    static string[] StageNames = { "Gamer Girl", "Spooky!", "Relax", "Spiritual", "Sprawled", "All four", "Teasing of Maid", "Back view", "Outdoors", "Webcam" };

    private static long minValue;
    private static long maxValue;

    private float moveSpeed = 1000f;
    private static Vector2 desiredPos;
    private static Image LevelCompleteImage;
    private static Image GameCompleteImage;
    public static bool winAnimationStarted = false;
    public static bool isGameWin = false;
    private static bool isFirstUpdateInProcess = true;
    private bool winAnimationReturnAwaiting = false;
    private float winAnimationReturnTimer;

    static AudioSource fanfareAudio;
    static AudioClip clip;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.interactable = false;
        progressBarParticles = GameObject.Find("ProgressBarParticles").GetComponent<ParticleSystem>();
        winParticleSystem = GameObject.Find("LevelComplete").GetComponent<ParticleSystem>();
        gameWinParticleSystem = GameObject.Find("GameComplete").GetComponent<ParticleSystem>();
        GameCompleteImage = GameObject.Find("GameComplete").GetComponent<Image>();

        fanfareAudio = gameObject.AddComponent<AudioSource>();
        clip = (AudioClip)Resources.Load("Sounds/UI/Fanfare");
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public static void setLevel(int level)
    {
        SteamWrapper.UnlockAchievement(level);
        
        if(level>10)
        {
            Debug.Log("You beat all game!");
            return;
        }

        if(level == 1)
        {
            slider.minValue = 0;
        }
        else
        {
            /*if(GameLogic.getMaxLevel() < level)
            {
                GameLogic.setMaxLevel(level);
            }*/
            slider.minValue = WinValue[level - 2];
            
        }
        GameLogic.resetHeartsForFreeLevel();

        slider.maxValue = WinValue[level - 1];

        targetProgress = GameLogic.getHearts(false);
        slider.value = GameLogic.getHearts(false);
        
        
        Text caption = GameObject.Find("CaptionText").GetComponent<Text>();
        Debug.Log("level " + level);
        caption.text = Localization.l("UI_Level") + " " + level + ": " + Localization.l("Level" + level);

        isGameWin = false;
        winAnimationStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value < targetProgress)
        {
            slider.value += FillSpeed * Time.deltaTime;
            ChangeMovieIfNeeded(true);
            if (!progressBarParticles.isPlaying)
                progressBarParticles.Play();
        }
        else
        {
            progressBarParticles.Stop();
        }

        if (winAnimationStarted)
        {
            if(isGameWin)
            {
                GameWinMoveWinAndCheck();
            }
            else
            {
                MoveAndCheckAnimationForFinish();
            }
        }
        else if(winAnimationReturnAwaiting)
        {
            winAnimationReturnTimer -= Time.deltaTime;
            if(winAnimationReturnTimer <=0 )
            {
                StartMovingWinAnimationBack();
            }
        }
    }

    private void GameWinMoveWinAndCheck()
    {
        if (!gameWinParticleSystem.isPlaying)
            gameWinParticleSystem.Play();
        GameCompleteImage.rectTransform.anchoredPosition = Vector2.MoveTowards(GameCompleteImage.rectTransform.anchoredPosition, desiredPos, moveSpeed * Time.deltaTime);
        if (GameCompleteImage.rectTransform.anchoredPosition == desiredPos)
        {
            winAnimationStarted = false;
        }
    }

    private void MoveAndCheckAnimationForFinish()
    {
        LevelCompleteImage.rectTransform.anchoredPosition = Vector2.MoveTowards(LevelCompleteImage.rectTransform.anchoredPosition, desiredPos, moveSpeed * Time.deltaTime);
        if (LevelCompleteImage.rectTransform.anchoredPosition == desiredPos)
        {
            winAnimationStarted = false;
            if (LevelCompleteImage.rectTransform.anchoredPosition.y == 0)
            {
                winAnimationReturnAwaiting = true;
                winAnimationReturnTimer = 3;
            }
        }
    }
    private void StartMovingWinAnimationBack()
    {
        winParticleSystem.Stop();
        winAnimationReturnAwaiting = false;
        winAnimationReturnTimer = 0;

        desiredPos = new Vector2(0, 1000);
        winAnimationStarted = true;
    }


    public static void ChangeMovieIfNeeded(bool isStartSetup)
    {
        string needMovieName = GetNeededMovieName(isStartSetup);
        //Debug.Log("needMovieName: " + needMovieName);

        if((GameLogic.getCurrentlyPlayingClip() != needMovieName) || isStartSetup)
        {
            MainVideoClick.PlayVideo(needMovieName);
        }
    }

    private static string GetNeededMovieName(bool isStartSetup)
    {
        if(GameLogic.level > 10)
        {
            return "Girl10_3";
        }

        if (GameLogic.level == 1)
        {
            minValue = 0;
        }
        else
        {
            if (GameLogic.getMaxLevel() < GameLogic.level)
            {
                GameLogic.setMaxLevel(GameLogic.level);
            }
            minValue = WinValue[GameLogic.level - 2];
        }
        maxValue = WinValue[GameLogic.level - 1];

        float third = (maxValue - minValue) / 3.0f + minValue;
        float twoThird = (maxValue - minValue) / 1.5f + minValue;

        int stage = getStage();

        if (stage == 1 || stage == 2)
        {
            return "Girl" + GameLogic.level + "_" + stage;
        }
        else if (GameLogic.getHearts(false) < slider.maxValue && GameLogic.isProgressMode)
        {
            return "Girl" + GameLogic.level + "_" + 3;
        }
        else if (GameLogic.isProgressMode) //if we need to start win sequence
        {
            InitiateWinStage(isStartSetup);
            return "Girl" + GameLogic.level + "_" + 1;
        }
        if(GameLogic.getHearts(false) >= slider.maxValue)
        {
            GameLogic.showNextLevelButton();
        }
        
        return "Girl" + GameLogic.level + "_" + 3;
    }

    public static int getStage()
    {
        float third = (maxValue - minValue) / 3.0f + minValue;
        float twoThird = (maxValue - minValue) / 1.5f + minValue;

        if (GameLogic.getHearts(false) < third)
        {
            return 1;
        }
        else if (GameLogic.getHearts(false) >= third && GameLogic.getHearts(false) < twoThird)
        {
            return  2;
        }
        return 3;
    }


    private static void InitiateWinStage(bool isStartSetup)
    {
        GameLogic.level++;
        if(GameLogic.level > 10)
        {
            isGameWin = true;
           winAnimationStarted = true;
        }
        else
        {
            setLevel(GameLogic.level);
        }
        
        GameLogic.SaveGame();
                
        if(isFirstUpdateInProcess)
        {
            return;
        }

        LevelCompleteImage = GameObject.Find("LevelComplete").GetComponent<Image>();
        desiredPos = new Vector2(0, 0);
        winAnimationStarted = true;
        fanfareAudio.PlayOneShot(clip, 0.5f*SoundManager.soundVolume);

        if (!winParticleSystem.isPlaying)
            winParticleSystem.Play();
    }

    public static void SetProgress(float target, bool isStartUpdate)
    {
        isFirstUpdateInProcess = isStartUpdate;
        FillSpeed = target - slider.value;
        targetProgress = target;
    }
}
