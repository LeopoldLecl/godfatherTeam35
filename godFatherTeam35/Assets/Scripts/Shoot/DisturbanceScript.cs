using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturbanceScript : MonoBehaviour
{
    Animator _animator;
    private bool _isAimed;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        ShootingScript.HasFired += CheckFire;
    }

    private void OnDisable()
    {
        ShootingScript.HasFired -= CheckFire;
    }
    private void CheckFire()
    {
        if (_isAimed)
        {
            ChangeAnimation();
        }
    }
    private void ChangeAnimation()
    {
        _animator.SetTrigger("destroyed");
    }

    private void OnMouseEnter() => _isAimed = true;
    private void OnMouseExit() => _isAimed = false;
}
