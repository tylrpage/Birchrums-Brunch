using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulldown : MonoBehaviour
{
    [SerializeField] private float startY;

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (transform.position.y > startY && _rigidbody2D.velocity.y > 0)
        {
            //_rigidbody2D.gravityScale = 1 + _rigidbody2D.position.y - startY;
            float diff = transform.position.y - startY;
            _rigidbody2D.AddForce(new Vector2(0, -diff), ForceMode2D.Force);
        }
        else
        {
            //_rigidbody2D.gravityScale = 1;
        }
    }
}
