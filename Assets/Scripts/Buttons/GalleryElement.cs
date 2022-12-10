using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class GalleryElement : MonoBehaviour
{
    public bool isClickable = true;
    RenderTexture rt;
    RawImage rawImage;
    RawImage dynamicImage;
    string substring;
    VideoPlayer VP;

    // Start is called before the first frame update
    void Start()
    {
        substring = this.gameObject.name.Substring(8);

        rawImage = GameObject.Find("RawImage" + substring).GetComponent<RawImage>();

        dynamicImage = GameObject.Find("DynamicImage").GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseUp()
    {
        if (!isClickable)
            return;
        GalleryController.currentVideo = "Girl" + substring;
        Debug.Log("GalleryController.currentVideo " + GalleryController.currentVideo);
        SceneManager.LoadScene("GalleryFullscreen");
    }

    public void OnMouseEnter()
    {
        if(!isClickable)
        {
            return;
        }

        if (!VP)
        {
            rt = new RenderTexture(1920, 1080, 16, RenderTextureFormat.ARGB32);
            rt.Create();

            VP = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
            VideoClip clip = Resources.Load<VideoClip>("Girl" + substring) as VideoClip;
            VP.waitForFirstFrame = true;
            VP.playOnAwake = true;
            VP.clip = clip;
            VP.isLooping = true;
            VP.targetTexture = rt;
            VP.Stop();
        }

        //rawImage.texture = rt;

        dynamicImage.texture = rt;
        dynamicImage.rectTransform.anchoredPosition = rawImage.rectTransform.anchoredPosition;

        VP.Play();
    }

    public void OnMouseExit()
    {
        if(!isClickable)
        {
            return;
        }
        if(VP)
        {
            VP.Stop();
        }
        
        //rawImage.texture = Resources.Load<Texture2D>("Preview" + substring);
    }
}
