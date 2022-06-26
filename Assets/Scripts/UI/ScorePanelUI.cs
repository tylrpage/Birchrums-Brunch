using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScorePanelUI : MonoBehaviour
{
    [SerializeField] private GameObject success;
    [SerializeField] private GameObject failed;
    [SerializeField] private GameObject pointsPanel;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private GameObject requiredPointsPanel;
    [SerializeField] private TMP_Text requiredPointsText;
    [SerializeField] private GameObject highestComboPanel;
    [SerializeField] private TMP_Text highestComboText;
    [SerializeField] private GameObject timeLastedPanel;
    [SerializeField] private TMP_Text timeLastedText;
    [SerializeField] private float hidePositionY;
    [SerializeField] private float showPositionY;
    [SerializeField] private float showTime;

    private void Awake()
    {
        Hide(true);
        GameManager.Instance.TimeManager.TimerEnded += Show;
    }

    private void Show()
    {
        Vector3 showPosition = new Vector3(transform.position.x, showPositionY, transform.position.z);
        transform.DOLocalMove(showPosition, showTime).SetEase(Ease.OutBack);

        LevelSelectManager levelSelectManager = GameManager.Instance.LevelSelectManager;
        
        bool didCompleteLevel = levelSelectManager.DidCompleteLevel();
        success.SetActive(didCompleteLevel);
        failed.SetActive(!didCompleteLevel);
        GameManager.Instance.SoundManager.LevelComplete(didCompleteLevel);
        
        pointsText.text = GameManager.Instance.PointsManager.Points.ToString("N0");
        highestComboText.text = "x" + GameManager.Instance.PointsManager.HighestCombo.ToString("N0");

        if (levelSelectManager.CurrentLevel == LevelSelectManager.Level.Endless)
        {
            requiredPointsPanel.SetActive(false);

            timeLastedPanel.SetActive(true);
            timeLastedText.text = GameManager.Instance.TimeManager.Time.ToString(@"mm\:ss");
        }
        else
        {
            requiredPointsPanel.SetActive(true);
            requiredPointsText.text = GameManager.Instance.LevelSelectManager.GetRequiredPointsForCurrentLevel()
                .ToString("N0");
            
            timeLastedPanel.SetActive(false);
        }
    }

    private void Hide(bool instant)
    {
        Vector3 hidePosition = new Vector3(transform.position.x, hidePositionY, transform.position.z);
        if (instant)
        {
            transform.localPosition = new Vector3(transform.position.x, hidePositionY, transform.position.z);
        }
        else
        {
            transform.DOLocalMove(hidePosition, showTime).SetEase(Ease.OutBack);
        }
    }
    
    public void BackClicked()
    {
        Hide(false);
        
        GameManager.Instance.CameraManager.MoveToCanvas(CameraManager.CanvasOption.Level);
        GameManager.Instance.SoundManager.ButtonPressed();
    }
    
    public void ReplayClicked()
    {
        Hide(false);
        
        GameManager.Instance.LevelSelectManager.RestartLevel();
        GameManager.Instance.SoundManager.ButtonPressed();
    }
}
