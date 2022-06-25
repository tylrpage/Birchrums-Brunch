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
    
    public TimeSpan TimeLeft { get; private set; }
    public bool Enabled { get; private set; }
    // Represents initial timer or stopwatch value depending on mode
    public TimeSpan TotalTime { get; private set; }

    private Mode _mode;

    public void StartTimer(TimeSpan timeLeft)
    {
        _mode = Mode.Timer;
        TotalTime = timeLeft;
        TimeLeft = timeLeft;
        Enabled = true;
    }

    public void StartStopwatch()
    {
        _mode = Mode.Stopwatch;
        TotalTime = TimeSpan.Zero;
    }

    public void Update()
    {
        if (Enabled)
        {
            switch (_mode)
            {
                case Mode.Timer:
                    if (TimeLeft.TotalMilliseconds <= 0)
                    {
                        Enabled = false;
                        TimerEnded?.Invoke();
                    }
                    else
                    {
                        TimeLeft -= TimeSpan.FromSeconds(Time.deltaTime);
                    }

                    break;
                case Mode.Stopwatch:
                    TotalTime += TimeSpan.FromSeconds(Time.deltaTime);
                    break;
            }
        }
    }
}
