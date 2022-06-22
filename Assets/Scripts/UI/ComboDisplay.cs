using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ComboDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private Transform comboPanelTransform;
    [SerializeField] private Transform starTransform;
    
    [SerializeField] private Vector3 punchVector;
    [SerializeField] private float punchDuration;
    [SerializeField] private int punchVibrato;
    [SerializeField] [Range(0, 1)] private float punchElasticity;
    
    private void Awake()
    {
        GameManager.Instance.PointsManager.ComboChanged += PointsManagerOnComboChanged;
        SetComboText(GameManager.Instance.PointsManager.Combo);
    }

    private void PointsManagerOnComboChanged(int value, int delta)
    {
        SetComboText(value);
        comboPanelTransform.DOPunchScale(punchVector, punchDuration, punchVibrato, punchElasticity);
    }

    private void SetComboText(int combo)
    {
        comboText.text = $"x{combo:N0}";
    }
}
