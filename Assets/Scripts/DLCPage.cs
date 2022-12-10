using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DLCPage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FillImages();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FillImages()
    {
        for (int i = 1; i <= 10; i++)
            for (int j = 1; j <= 3; j++)
            {
                RawImage rawImage = GameObject.Find("RawImage" + i + "_" + j).GetComponent<RawImage>();
                rawImage.texture = Resources.Load<Texture2D>("plate");
            }
    }
}
