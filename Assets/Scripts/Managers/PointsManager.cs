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
    public event Action Reset;
    
    public int Points { get; private set; }
    public int Combo { get; private set; }
    public int HighestCombo { get; private set; }

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
            
            GameManager.Instance.SoundManager.Score();
        }
        else
        {
            ClearCombo();

            if (GameManager.Instance.LevelSelectManager.CurrentLevel == LevelSelectManager.Level.Endless)
            {
                GameManager.Instance.TimeManager.StopGame();
            }
            else
            {
                // Don't play mistake sound in endless, they will hear the failure sound instead
                GameManager.Instance.SoundManager.Mistake();
            }
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

        HighestCombo = Math.Max(Combo, HighestCombo);
        
        ComboChanged?.Invoke(Combo, 1);
    }

    public void ResetHighestComboAndPoints()
    {
        HighestCombo = 1;
        Points = 0;
        Combo = 1;
        Reset?.Invoke();
    }

    public void ClearCombo()
    {
        int oldCombo = Combo;
        Combo = 1;
        ComboChanged?.Invoke(Combo, 1 - oldCombo);
    }
}
