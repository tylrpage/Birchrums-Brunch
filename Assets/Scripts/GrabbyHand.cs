using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbyHand : MonoBehaviour
{
    [SerializeField] private bool isLeft;
    [SerializeField] private float moveDistance;
    [SerializeField] private float extraMoveDistance;
    [SerializeField] private float oneWayTime;
    [SerializeField] private AnimationCurve speedCurve;

    private Vector2 _startPosition;
    private Rigidbody2D _rb;
    private Rigidbody2D _grabbedObject;
    private Vector3 _grabbedObjectOffset;
    private bool _currentlyGrabbing;
    private bool _awardedPoints;

    private void Awake()
    {
        _startPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if ((isLeft && Input.GetKeyDown(KeyCode.A)) ||
            (!isLeft && Input.GetKeyDown(KeyCode.D)))
        {
            if (!_currentlyGrabbing)
            {
                _currentlyGrabbing = true;
                StartCoroutine(GrabRoutine(() => _currentlyGrabbing = false));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Don't grab if we have something already grabbed, or if it has immunity
        if (_grabbedObject == null && col.GetComponent<GrabImmunity>() == null)
        {
            _grabbedObject = col.attachedRigidbody;
            _grabbedObject.isKinematic = true;
            // Let it pass through walls
            //_grabbedObject.gameObject.layer = LayerMask.NameToLayer("Walls");
            _grabbedObject.angularVelocity = 0;
            _grabbedObjectOffset = col.transform.position - transform.position;
        }
    }

    private IEnumerator GrabRoutine(Action completed)
    {
        _awardedPoints = false;
        
        Vector2 targetPosition = isLeft
            ? _startPosition + (moveDistance * Vector2.right)
            : _startPosition + moveDistance * Vector2.left;

        // Moves out until reaching desired location, or grabbed something
        float duration = 0;
        while (duration < oneWayTime && _grabbedObject == null)
        {
            float t = duration / oneWayTime;
            float tCurved = speedCurve.Evaluate(t);

            Vector2 lerpedPos = Vector2.Lerp(_startPosition, targetPosition, tCurved);
            _rb.MovePosition(lerpedPos);

            duration += Time.deltaTime;
            yield return null;
        }

        // Move back until we are to start position
        float returnPosX = _startPosition.x;
        float speed = moveDistance / oneWayTime;
        while ((isLeft && _rb.position.x > returnPosX) ||
               (!isLeft && _rb.position.x < returnPosX))
        {
            if (_grabbedObject != null)
            {
                // Move even further back if we have a grabbed object, so that it goes off screen
                returnPosX = _startPosition.x + (isLeft ? -1 : 1) * extraMoveDistance;
                // Move the grabbed object along with us
                _grabbedObject.MovePosition(transform.position + _grabbedObjectOffset);

                // Only awards points once per routine
                if (!_awardedPoints)
                {
                    // add or subtract points depending on the object
                    _awardedPoints = true;
                    
                    var grabbedObjectController = _grabbedObject.GetComponent<ObjectController>();
                    if (!grabbedObjectController.Good)
                    {
                        GameManager.Instance.PointsManager.ChangePoints(500);
                    }
                    else
                    {
                        GameManager.Instance.PointsManager.ChangePoints(-500);
                    }
                }
            }

            Vector2 movement = (isLeft ? Vector2.left : Vector2.right) * speed;
            _rb.velocity = movement;

            yield return null;
        }

        if (_grabbedObject != null)
        {
            Destroy(_grabbedObject.gameObject);
            _grabbedObject = null;
            
            // Move back to our original starting position
            while ((isLeft && _rb.position.x < _startPosition.x) ||
                   (!isLeft && _rb.position.x > _startPosition.x))
            {
                Vector2 movement = (isLeft ? Vector2.right : Vector2.left) * speed;
                _rb.velocity = movement;

                yield return null;
            }
        }
        
        _rb.MovePosition(_startPosition);
        _rb.velocity = Vector2.zero;

        completed?.Invoke();
    }
}