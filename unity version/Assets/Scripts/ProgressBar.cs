using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private float currentValue = 1f;
    private float curTime;
    private Slider progressBar;
    private bool start;

    public float maxTime;
    public Text timeRemainig;

    public float CurrentValue
    {
        get { return currentValue; }
        set
        {
            currentValue = value;
            progressBar.value = currentValue;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        progressBar = gameObject.GetComponent<Slider>();
        CurrentValue = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            curTime -= Time.deltaTime;
            timeRemainig.text = curTime.ToString("F2");
            CurrentValue = curTime / maxTime;
        }
        
    }

    public void SetValues()
    {
        curTime = maxTime;
        timeRemainig.text = curTime.ToString("F2");
    }

    public float GetCurTime()
    {
        return curTime;
    }

    public void StartTimer()
    {
        start = true;
    }

    public void StopTimer()
    {
        start = false;
    }
}
