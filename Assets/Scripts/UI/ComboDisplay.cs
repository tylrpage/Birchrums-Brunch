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
    [SerializeField] private Animator animator;
    
    [SerializeField] private Vector3 punchVector;
    [SerializeField] private float punchDuration;
    [SerializeField] private int punchVibrato;
    [SerializeField] [Range(0, 1)] private float punchElasticity;

    private string _currentAnimation;
    
    private void Awake()
    {
        GameManager.Instance.PointsManager.ComboChanged += PointsManagerOnComboChanged;
        GameManager.Instance.PointsManager.Reset += Reset;
        SetComboText(GameManager.Instance.PointsManager.Combo, 0);
    }

    private void PointsManagerOnComboChanged(int value, int delta)
    {
        SetComboText(value, delta);
        comboPanelTransform.DOPunchScale(punchVector, punchDuration, punchVibrato, punchElasticity);
    }

    private void Reset()
    {
        comboPanelTransform.gameObject.SetActive(false);
    }

    private void SetComboText(int combo, int delta)
    {
        if (combo <= 1 && delta >= 0)
        {
            comboPanelTransform.gameObject.SetActive(false);
        }
        else
        {
            comboPanelTransform.gameObject.SetActive(true);
        }

        if (combo == 1 && delta < 0)
        {
            SetAnimation("ClearCombo");
        }
        else
        {
            comboText.text = $"x{combo:N0}";
        }
        
        if (combo == 2)
        {
            SetAnimation("Start");
        }
    }

    private void SetAnimation(string animation)
    {
        if (animation == _currentAnimation)
            return;
        
        animator.Play(animation);
    }
}
