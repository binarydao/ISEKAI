using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Video;
using UnityEngine.UI;

public class ProgressElement : MonoBehaviour
{
    RenderTexture rt;
    RawImage rawImage;
    RawImage dynamicImage;
    VideoPlayer VP;

    // Start is called before the first frame update
    void Start()
    {
        rawImage = GameObject.Find("Progress").GetComponent<RawImage>();
        dynamicImage = GameObject.Find("DynamicProgress").GetComponent<RawImage>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseUp()
    {
        GameLogic.isProgressMode = true;
        SceneManager.LoadScene("GamePlayScene");
    }

    public void OnMouseEnter()
    {
        if (!VP)
        {
            rt = new RenderTexture(1920, 1080, 16, RenderTextureFormat.ARGB32);
            rt.Create();

            VP = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
            int tempLevel = GameLogic.getMaxLevel();
            if (tempLevel > 10)
            {
                tempLevel = 10;
            }
            VideoClip clip = Resources.Load<VideoClip>("Girl" + tempLevel + "_2") as VideoClip;
            VP.waitForFirstFrame = true;
            VP.playOnAwake = true;
            VP.clip = clip;
            VP.isLooping = true;
            VP.targetTexture = rt;
            VP.Stop();
        }

        dynamicImage.texture = rt;
        dynamicImage.rectTransform.anchoredPosition = rawImage.rectTransform.anchoredPosition;

        VP.Play();
    }

    public void OnMouseExit()
    {
        if (VP)
        {
            VP.Stop();
        }

        rawImage.texture = Resources.Load<Texture2D>("Preview" + GameLogic.getMaxLevel() + "_2");
    }
}
