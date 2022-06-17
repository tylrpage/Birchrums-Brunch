using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulldown : MonoBehaviour
{
    [SerializeField] private float startY;
    [SerializeField] private float maxY;
    [SerializeField] private AnimationCurve limitCurve;

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (transform.position.y > startY && _rigidbody2D.velocity.y > 0)
        {
            float mult = 1 - limitCurve.Evaluate((transform.position.y - startY) / maxY);
            Debug.Log(mult);
            var multVector = new Vector2(1, mult);
            _rigidbody2D.velocity *= multVector;
        }
    }
}
