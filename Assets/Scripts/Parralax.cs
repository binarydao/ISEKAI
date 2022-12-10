using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parralax : MonoBehaviour
{
    private Image background;

    // Start is called before the first frame update
    void Start()
    {
        background = GameObject.Find("BackgroundImage").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = -Input.mousePosition.x * 0.1f;
        float y = -Input.mousePosition.y * 0.1f;
        //Debug.Log("Input.mousePosition: " + Input.mousePosition);
        Vector2 newPos = new Vector2(x, y);
        
        background.rectTransform.anchoredPosition = newPos;
    }
}
