using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Drawing;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class SteamWrapper : MonoBehaviour
{
    private static bool initialisationStarted;

    static CSteamID steamIDUser;
    static bool isIdReturned;

    static int achievementsCount = 42;

    private static InputHandle_t[] InputHandles = new InputHandle_t[Constants.STEAM_INPUT_MAX_COUNT];
    private static InputDigitalActionHandle_t ClickHandle;
    private static InputAnalogActionHandle_t MoveHandle;
    private static bool steamApiResult = false;

    private static StreamWriter fileWriter;

    public const int MOUSEEVENTF_LEFTDOWN = 0x02;
    public const int MOUSEEVENTF_LEFTUP = 0x04;

    private static InputActionSetHandle_t InGameControlsHandle;

    GameObject ClickImage;
    GameObject MoveImage;
    GameObject ClickCaption;
    GameObject MoveCaption;

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out Point point);

    [DllImport("user32.dll")]
    public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("SteamWrapper start");
        DontDestroyOnLoad(gameObject);

        File.WriteAllBytes("game.log", new byte[0]);
        fileWriter = new StreamWriter("game.log", true);
        fileWriter.AutoFlush = true;
        fileWriter.WriteLine("SteamWrapper Start");

        if (!initialisationStarted)
        {
            initialisationStarted = true;
            initialize();
        }

        UnlockAchievement(11);
    }

    private void initialize()
    {
        fileWriter.WriteLine("SteamWrapper initialize");
        if (!Packsize.Test())
        {
            Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
        }

        if (!DllCheck.Test())
        {
            Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
        }

        

        try
        {
            steamApiResult = SteamAPI.Init();
        }
        catch (Exception e)
        {
        }
        

        Debug.Log("SteamApi: " + steamApiResult);
        fileWriter.WriteLine("SteamApi: " + steamApiResult);


        if (steamApiResult)
        {
            //SteamAPI.RunCallbacks();
            System.Diagnostics.Trace.WriteLine("steamApiResult true");
            if (SteamManager.Initialized)
            {
                string name = SteamFriends.GetPersonaName();
                Debug.Log(name);
            }

            Debug.Log("Build id: " + SteamApps.GetAppBuildId());
            Debug.Log("SteamUtils.GetSteamUILanguage: " + SteamUtils.GetSteamUILanguage());

            MenuLocalize.StartLoc();

            SteamInput.Init();

            InGameControlsHandle = SteamInput.GetActionSetHandle("InGameControls");
            fileWriter.WriteLine("InGameControlsHandle: " + InGameControlsHandle.ToString());

            ClickHandle = SteamInput.GetDigitalActionHandle("Click");

            MoveHandle = SteamInput.GetAnalogActionHandle("Move");

            for (int i = 0; i < Constants.STEAM_INPUT_MAX_COUNT; i++)
            {
                SteamInput.ActivateActionSet(InputHandles[i], InGameControlsHandle);
            }
                
        }
        else
        {
            initialisationStarted = false;
            System.Diagnostics.Trace.WriteLine("steamApiResult false");
        }

        /*ClickImage = GameObject.Find("ClickImage");
        MoveImage = GameObject.Find("MoveImage");
        ClickCaption = GameObject.Find("ClickCaption");
        MoveCaption = GameObject.Find("MoveCaption");*/
    }

    internal static string GetCurrentGameLanguage()
    {
        //Debug.Log("initialisationStarted: " + initialisationStarted);
        if(!initialisationStarted)
        {
            return "no steam";
        }

        return SteamUtils.GetSteamUILanguage();
        //return SteamApps.GetCurrentGameLanguage();
    }

    // Update is called once per frame
    void Update()
    {
        bool firstControllerFound = false;
        bool isController = false;
        if (steamApiResult)
        {
            SteamAPI.RunCallbacks();
            SteamInput.GetConnectedControllers(InputHandles);

            for (int i=0; i < Constants.STEAM_INPUT_MAX_COUNT; i++)
            {
                if (SteamInput.GetDigitalActionData(InputHandles[i], ClickHandle).bState > 0)
                {
                    LeftMouseClick();
                }
                InputAnalogActionData_t AAD = SteamInput.GetAnalogActionData(InputHandles[i], MoveHandle);
                int deltaX = (int)(AAD.x*0.5f);
                int deltaY = (int)(AAD.y*0.5f);
                
                Point currentPoint;
                GetCursorPos(out currentPoint);
                SetCursorPos(currentPoint.X + deltaX, currentPoint.Y + deltaY);
                
                if(!firstControllerFound)
                {
                    firstControllerFound = true;

                    EInputActionOrigin[] origins1 = new EInputActionOrigin[Constants.STEAM_INPUT_MAX_ORIGINS];
                    SteamInput.GetDigitalActionOrigins(InputHandles[i], InGameControlsHandle, ClickHandle, origins1);
                    EInputActionOrigin firstOrigin = origins1[0];
                    if(firstOrigin.ToString() == "k_EInputActionOrigin_None")
                    {
                        Debug.Log("no device");
                        break;
                    }
                    isController = true;
                    String clickGlyphPath = SteamInput.GetGlyphForActionOrigin(firstOrigin);
                    if (clickGlyphPath.Length < 2)
                    {
                        break;
                    }
                    
                    Sprite clickSprite = LoadSprite(clickGlyphPath);
                    GameObject.Find("ClickImage").GetComponent<Image>().sprite = clickSprite;

                    EInputActionOrigin[] origins2 = new EInputActionOrigin[Constants.STEAM_INPUT_MAX_ORIGINS];
                    SteamInput.GetAnalogActionOrigins(InputHandles[i], InGameControlsHandle, MoveHandle, origins2);
                    EInputActionOrigin secondOrigin = origins2[0];
                    String moveGlyphPath = SteamInput.GetGlyphForActionOrigin(secondOrigin);
                    Sprite moveSprite = LoadSprite(moveGlyphPath);
                    GameObject.Find("MoveImage").GetComponent<Image>().sprite = moveSprite;
                }

            }

            ClickImage = GameObject.Find("ClickImage");
            MoveImage = GameObject.Find("MoveImage");
            ClickCaption = GameObject.Find("ClickCaption");
            MoveCaption = GameObject.Find("MoveCaption");
            if(isController)
            {
                Debug.Log("Controller found");
                Color visibleColor = Color.white;
                ClickImage.GetComponent<Image>().color = visibleColor;
                MoveImage.GetComponent<Image>().color = visibleColor;
                ClickCaption.GetComponent<Text>().text = Localization.l("UI_Click");
                MoveCaption.GetComponent<Text>().text = Localization.l("UI_Move");
            }
            else
            {
                Debug.Log("No controllers found");
                Color invisibleColor = Color.clear;
                ClickImage.GetComponent<Image>().color = invisibleColor;
                MoveImage.GetComponent<Image>().color = invisibleColor;
                ClickCaption.GetComponent<Text>().text = "";
                MoveCaption.GetComponent<Text>().text = "";
            }

        }

    }

    private Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }

    public static void LeftMouseClick()
    {
        Point currentPoint;
        GetCursorPos(out currentPoint);
        mouse_event(MOUSEEVENTF_LEFTDOWN, currentPoint.X, currentPoint.Y, 0, 0);
        mouse_event(MOUSEEVENTF_LEFTUP, currentPoint.X, currentPoint.Y, 0, 0);
    }

    private void OnApplicationCancelQuit()
    {
        if (!initialisationStarted)
        {
            return;
        }

        initialisationStarted = false;

        Debug.Log("SteamAPI.Shutdown();");
        SteamAPI.Shutdown();
    }

    private void OnApplicationQuit()
    {
        if (!initialisationStarted)
        {
            return;
        }

        initialisationStarted = false;

        Debug.Log("SteamAPI.Shutdown();");
        SteamAPI.Shutdown();
    }

    public static void UnlockAchievement(int id)
    {
        if (!initialisationStarted)
        {
            return;
        }

        string pchName = "ACHIEVEMENT" + id;

        SteamUserStats.SetAchievement(pchName);
        SteamUserStats.StoreStats();
    }

    public static void ClearAchievements()
    {
        for(int i=1; i<= achievementsCount; i++)
        {
            string pchName = "ACHIEVEMENT" + i;
            SteamUserStats.ClearAchievement(pchName);
        }
        SteamUserStats.StoreStats();
    }


}
