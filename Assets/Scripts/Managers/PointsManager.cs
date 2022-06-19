using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    // param is delta
    public delegate void PointsChangedHandler(int points, int delta);
    public event PointsChangedHandler PointsChanged;
    
    public int Points { get; private set; }

    private void Start()
    {
        ChangePoints(0);
    }

    public void ChangePoints(int delta)
    {
        // Limit points to never go below 0
        Points = Mathf.Max(Points + delta, 0);
        PointsChanged?.Invoke(Points, delta);
    }
}
