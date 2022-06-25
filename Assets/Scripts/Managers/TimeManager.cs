using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public enum Mode
    {
        Stopwatch, Timer
    }
    
    public event Action TimerEnded;
    
    public Mode TimerMode { get; private set; }
    
    // Represents time left or stopwatch value depending on mode
    public TimeSpan Time { get; private set; }
    public bool Enabled { get; private set; }
    public TimeSpan InitialTime { get; private set; }

    [SerializeField] private int timerSeconds;

    public void StartTimer()
    {
        TimeSpan timeLeft = TimeSpan.FromSeconds(timerSeconds);
        
        Enabled = true;
        TimerMode = Mode.Timer;
        InitialTime = timeLeft;
        Time = timeLeft;
    }

    public void StartStopwatch()
    {
        Enabled = true;
        TimerMode = Mode.Stopwatch;
        Time = TimeSpan.Zero;
        InitialTime = TimeSpan.Zero;
    }

    public void Update()
    {
        if (Enabled)
        {
            switch (TimerMode)
            {
                case Mode.Timer:
                    if (Time.TotalMilliseconds <= 0)
                    {
                        Enabled = false;
                        TimerEnded?.Invoke();
                    }
                    else
                    {
                        Time -= TimeSpan.FromSeconds(UnityEngine.Time.deltaTime);
                    }

                    break;
                case Mode.Stopwatch:
                    Time += TimeSpan.FromSeconds(UnityEngine.Time.deltaTime);
                    break;
            }
        }
    }
}
