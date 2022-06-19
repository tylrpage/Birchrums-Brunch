using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public event Action<ObjectController> Destroyed;
    
    [SerializeField] private int groupLevel;
    [SerializeField] private bool good;

    public int GroupLevel => groupLevel;
    public bool Good => good;

    private void OnDestroy()
    {
        Destroyed?.Invoke(this);
    }
}
