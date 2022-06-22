using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabImmunity : MonoBehaviour
{
    [SerializeField] private float immunity;
    [SerializeField] private float flashInterval;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor;

    private float _immunityLeft;
    private float _timeSinceVisibilityToggle;

    private void Awake()
    {
        _immunityLeft = immunity;
    }

    private void Update()
    {
        _immunityLeft -= Time.deltaTime;
        _timeSinceVisibilityToggle += Time.deltaTime;
        
        // Flash sprite
        if (_timeSinceVisibilityToggle > flashInterval)
        {
            _timeSinceVisibilityToggle = 0;
            spriteRenderer.color = spriteRenderer.color == Color.white ? flashColor : Color.white;
        }

        if (_immunityLeft <= 0)
        {
            spriteRenderer.color = Color.white;
            Destroy(this);
        }
    }
}
