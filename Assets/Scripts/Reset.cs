using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    private Vector3 _resetPosition;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _resetPosition = transform.position;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.position = _resetPosition;
        }
    }
}
