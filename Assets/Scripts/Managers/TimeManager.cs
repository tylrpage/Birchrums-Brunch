using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public event Action TimerEnded;
    
    public TimeSpan TimeLeft { get; private set; }
    public bool Enabled { get; private set; }
    public TimeSpan TotalTimeLeft { get; private set; }

    public void StartTimer(TimeSpan timeLeft)
    {
        TotalTimeLeft = timeLeft;
        TimeLeft = timeLeft;
        Enabled = true;
    }

    public void Update()
    {
        if (Enabled)
        {
            if (TimeLeft.TotalMilliseconds <= 0)
            {
                Enabled = false;
                TimerEnded?.Invoke();
            }
            else
            {
                TimeLeft -= TimeSpan.FromSeconds(Time.deltaTime);
            }
        }
    }
}
