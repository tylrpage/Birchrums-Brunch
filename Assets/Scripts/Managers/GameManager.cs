using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public PointsManager PointsManager { get; private set; }
    public LevelSelectManager LevelSelectManager { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        PointsManager = GetComponent<PointsManager>();
        LevelSelectManager = GetComponent<LevelSelectManager>();
    }
}
