using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text text;
    [SerializeField] private float almostDoneTime;
    [SerializeField] private Color almostDoneColor;
    
    private Color _originalFillColor;

    private void Awake()
    {
        _originalFillColor = fill.color;
    }

    private void Update()
    {
        TimeManager timeManager = GameManager.Instance.TimeManager;
        text.text = timeManager.Time.ToString(@"mm\:ss");

        if (timeManager.TimerMode == TimeManager.Mode.Timer)
        {
            fill.fillAmount = (float)timeManager.Time.Ticks / (float)timeManager.InitialTime.Ticks;
            
            if (timeManager.Time.TotalSeconds <= almostDoneTime)
            {
                fill.color = almostDoneColor;
            }
            else
            {
                fill.color = _originalFillColor;
            }
        }
        else
        {
            fill.fillAmount = 1;
            fill.color = _originalFillColor;
        }
    }
}
