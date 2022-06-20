using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private float textAdjustSpeed;

    [SerializeField] private Vector3 punchVector;
    [SerializeField] private float punchDuration;
    [SerializeField] private int punchVibrato;
    [SerializeField] [Range(0, 1)] private float punchElasticity;

    private int _displayedPoints;
    private int _targetPoints;
    private float _textSpeed;
    private Tweener _currentTween;

    private void Awake()
    {
        GameManager.Instance.PointsManager.PointsChanged += PointsManagerOnPointsChanged;
        pointsText.text = _displayedPoints.ToString("N0");
    }

    private void Update()
    {
        if (_displayedPoints != _targetPoints)
        {
            // Limit the lerped points to the target depending on the direction we are moving
            float lerpedPoints = _displayedPoints < _targetPoints
                ? Mathf.Min(_displayedPoints + Time.deltaTime * _textSpeed, _targetPoints)
                : Mathf.Max(_displayedPoints - Time.deltaTime * _textSpeed, _targetPoints);

            _displayedPoints = (int)Mathf.Ceil(lerpedPoints);
            pointsText.text = _displayedPoints.ToString("N0");
        }
    }

    private void PointsManagerOnPointsChanged(int points, int delta)
    {
        _targetPoints = points;
        
        // Cancel any current tween
        if (_currentTween?.IsPlaying() ?? false)
        {
            _currentTween.Rewind();
        }
        _currentTween = pointsText.transform.DOPunchScale(punchVector, punchDuration, punchVibrato, punchElasticity);

        // Re-calculate text speed, so that display text reaches the target in textAdjustSpeed seconds
        float diff = Mathf.Abs(_targetPoints - _displayedPoints);
        _textSpeed = diff / textAdjustSpeed;

        // todo: make a cool effect of + or - points using delta
    }
}