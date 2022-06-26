using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    public enum LevelState
    {
        Unlocked,
        Locked,
        Hidden
    }

    public enum Level
    {
        One,
        Two,
        Three,
        Endless
    }

    public event Action UnlockedChanged;
    
    public Level CurrentLevel { get; private set; }

    public SerializableDictionary<LevelState, Sprite> LogSprites;
    public SerializableDictionary<Level, Sprite> LevelSprites;
    public SerializableDictionary<Level, int> PointsNeededPerLevel;
    public Sprite MysterySprite;

    [SerializeField] private List<Sprite> level1Dialogs;
    [SerializeField] private List<Sprite> level2Dialogs;
    [SerializeField] private List<Sprite> level3Dialogs;
    [SerializeField] private List<Sprite> endlessDialogs;
    [SerializeField] private Image dialogImage;

    private Dictionary<Level, LevelState> _levelStates = new Dictionary<Level, LevelState>();
    // negative if not showing dialog
    private int _dialogIndex;

    private void Awake()
    {
        LoadLevelUnlocksFromPrefs();
        GameManager.Instance.TimeManager.TimerEnded += OnTimerEnded;
    }

    private void Update()
    {
        if (_dialogIndex >= 0 && Input.GetKeyDown(KeyCode.Space))
        {
            List<Sprite> dialogs = GetDialogForLevel(CurrentLevel);
            if (_dialogIndex == dialogs.Count - 1)
            {
                dialogImage.gameObject.SetActive(false);
                // It was the last dialog, start game
                StartLevel(CurrentLevel, false);
            }
            else
            {
                _dialogIndex++;
                dialogImage.sprite = dialogs[_dialogIndex];
            }
        }
    }

    private List<Sprite> GetDialogForLevel(Level level)
    {
        switch (level)
        {
            case Level.One:
                return level1Dialogs;
            case Level.Two:
                return level2Dialogs;
            case Level.Three:
                return level3Dialogs;
            case Level.Endless:
                return endlessDialogs;
        }

        return null;
    }

    public void StartLevel(Level level, bool showDialog)
    {
        CurrentLevel = level;
        GameManager.Instance.CameraManager.MoveToCanvas(CameraManager.CanvasOption.Game);

        if (showDialog)
        {
            _dialogIndex = 0;
            dialogImage.gameObject.SetActive(true);
            List<Sprite> dialogs = GetDialogForLevel(CurrentLevel);
            dialogImage.sprite = dialogs[_dialogIndex];
        }
        else
        {
            GameManager.Instance.ObjectSpawnManager.StartLevel(level);
            GameManager.Instance.PointsManager.ResetHighestComboAndPoints();
        
            if (level == Level.Endless)
                GameManager.Instance.TimeManager.StartStopwatch();
            else
                GameManager.Instance.TimeManager.StartTimer();
        }
    }

    public void RestartLevel()
    {
        StartLevel(CurrentLevel, false);
    }

    public bool DidCompleteLevel()
    {
        int requiredPoints = GetRequiredPointsForCurrentLevel();
        return GameManager.Instance.PointsManager.Points >= requiredPoints;
    }

    public int GetRequiredPointsForCurrentLevel()
    {
        if (PointsNeededPerLevel.TryGetValue(CurrentLevel, out int requiredPoints))
        {
            return requiredPoints;
        }

        return 0;
    }

    private void OnTimerEnded()
    {
        GameManager.Instance.ObjectSpawnManager.DeleteAllActiveObjects();
        
        if (DidCompleteLevel())
        {
            // Unlock the next level in prefs and ui
            CompleteLevel(CurrentLevel);
        }
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

    private void LoadLevelUnlocksFromPrefs()
    {
        foreach (var levelString in Enum.GetNames(typeof(Level)))
        {
            Level level = (Level)Enum.Parse(typeof(Level), levelString);

            LevelState levelState = PlayerPrefs.GetInt(levelString) == 1 ? LevelState.Unlocked : LevelState.Locked;
            
            // Special cases, level one is always unlocked
            if (level == Level.One)
            {
                levelState = LevelState.Unlocked;
            }
            // If endless is locked, make it hidden
            else if (level == Level.Endless && levelState == LevelState.Locked)
            {
                levelState = LevelState.Hidden;
            }

            _levelStates[level] = levelState;
        }
    }

    public void CompleteLevel(Level level)
    {
        // Get the next level, in an ugly way
        Level? nextLevelNullable = null;
        switch (level)
        {
            case Level.One:
                nextLevelNullable = Level.Two;
                break;
            case Level.Two:
                nextLevelNullable = Level.Three;
                break;
            case Level.Three:
                nextLevelNullable = Level.Endless;
                break;
        }

        // Unlock next level
        if (nextLevelNullable != null)
        {
            Level nextLevel = (Level)nextLevelNullable;
            _levelStates[nextLevel] = LevelState.Unlocked;
            PlayerPrefs.SetInt(nextLevel.ToString(), 1);
        }
        
        UnlockedChanged?.Invoke();
    }
}