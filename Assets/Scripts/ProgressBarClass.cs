using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class ProgressBarClass : MonoBehaviour
{
    private Slider slider;

    private float targetProgress = 0;
    private float FillSpeed = 0.6f;
    
    private long minValue;
    private long maxValue;

    private Vector2 desiredPos;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.interactable = false;
    }

    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(slider.value - targetProgress)>0.1)
        {
            slider.value += FillSpeed * Time.deltaTime;
        }

    }

    public void SetProgress(float target)
    {
        FillSpeed = target - slider.value;
        targetProgress = target;
    }
}
