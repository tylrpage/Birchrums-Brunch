using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public enum LevelState
    {
        Unlocked, Locked, Hidden
    }
    public enum Level
    {
        One, Two, Three, Endless
    }
    
    public SerializableDictionary<LevelState, Sprite> LogSprites;
    public SerializableDictionary<Level, Sprite> LevelSprites;
    public Sprite MysterySprite;

    private Dictionary<Level, LevelState> _levelStates = new Dictionary<Level, LevelState>();

    private void Awake()
    {
        // todo: dummy data, load this from prefs
        _levelStates.Add(Level.One, LevelState.Unlocked);
        _levelStates.Add(Level.Two, LevelState.Unlocked);
        _levelStates.Add(Level.Three, LevelState.Unlocked);
        _levelStates.Add(Level.Endless, LevelState.Unlocked);
    }

    public LevelState GetLevelState(Level level)
    {
        if (_levelStates.TryGetValue(level, out LevelState levelState))
        {
            return levelState;
        }
        else
        {
            return LevelState.Hidden;
        }
    }
}
