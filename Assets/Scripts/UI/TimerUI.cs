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
        text.text = timeManager.TimeLeft.ToString(@"mm\:ss");
        fill.fillAmount = (float)timeManager.TimeLeft.Ticks / (float)timeManager.TotalTime.Ticks;

        if (timeManager.TimeLeft.TotalSeconds <= almostDoneTime)
        {
            fill.color = almostDoneColor;
        }
        else
        {
            fill.color = _originalFillColor;
        }
    }
}
