using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] private bool isLeft;
    [SerializeField] private float force;

    private HingeJoint2D _hingeJoint2D;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _hingeJoint2D = GetComponent<HingeJoint2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isLeft && Input.GetKey(KeyCode.LeftArrow))
        {
            Bump();
        }
        else if (!isLeft && Input.GetKey(KeyCode.RightArrow))
        {
            Bump();
        }
    }

    private void Bump()
    {
        // var impulse = (90 * Mathf.Deg2Rad) * _rigidbody2D.inertia;
        // _rigidbody2D.AddTorque(impulse, ForceMode2D.Impulse);

        Vector2 objectPosition = new Vector2(transform.position.x, transform.position.y);
        _rigidbody2D.AddForceAtPosition(transform.up * force,  objectPosition - _hingeJoint2D.anchor, ForceMode2D.Impulse);
    }
}
