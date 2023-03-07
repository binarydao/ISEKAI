using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageHandler : MonoBehaviour
{
    private static int currentGirl = 0;
    private static readonly int MAX_GIRL = 20;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ImageHandler start");
        currentGirl++;
        if (currentGirl > MAX_GIRL)
        {
            currentGirl = 1;
        }

        SpriteRenderer spriteRenderer = GameObject.Find("RewardImage").GetComponent<SpriteRenderer>();
        string text = "Girls/girl" + currentGirl;
        spriteRenderer.sprite = Resources.Load<Sprite>(text);
    }
}
