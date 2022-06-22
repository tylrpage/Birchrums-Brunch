using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    // param is delta
    public delegate void ValueChangedHandler(int value, int delta);
    public event ValueChangedHandler PointsChanged;
    public event ValueChangedHandler ComboChanged;
    
    public int Points { get; private set; }
    public int Combo { get; private set; }

    [SerializeField] private int basePoints;

    private void Start()
    {
        ChangePoints(0);
        ClearCombo();
    }

    public void AteObject(bool good)
    {
        if (good)
        {
            ChangePoints(basePoints);
            IncreaseCombo();
        }
        else
        {
            ClearCombo();
        }
    }

    public void ChangePoints(int delta)
    {
        int earnedPoints = delta * Combo;
        
        // Limit points to never go below 0
        Points = Mathf.Max(Points + earnedPoints, 0);
        PointsChanged?.Invoke(Points, earnedPoints);
    }

    public void IncreaseCombo()
    {
        Combo++;
        ComboChanged?.Invoke(Combo, 1);
    }

    public void ClearCombo()
    {
        int oldCombo = Combo;
        Combo = 1;
        ComboChanged?.Invoke(Combo, 1 - oldCombo);
    }
}
