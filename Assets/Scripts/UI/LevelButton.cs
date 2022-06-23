using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private LevelSelectManager.Level level;
    [SerializeField] private Image logImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private Image textImage;

    private LevelSelectManager.LevelState _levelState;

    private void Start()
    {
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        LevelSelectManager levelSelectManager = GameManager.Instance.LevelSelectManager;
        _levelState = levelSelectManager.GetLevelState(level);

        logImage.sprite = levelSelectManager.LogSprites[_levelState];
        lockImage.gameObject.SetActive(_levelState != LevelSelectManager.LevelState.Unlocked);

        textImage.sprite = _levelState == LevelSelectManager.LevelState.Hidden
            ? levelSelectManager.MysterySprite
            : levelSelectManager.LevelSprites[level];
        textImage.gameObject.SetActive(_levelState != LevelSelectManager.LevelState.Hidden);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_levelState == LevelSelectManager.LevelState.Hidden)
        {
            textImage.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_levelState == LevelSelectManager.LevelState.Hidden)
        {
            textImage.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }
}