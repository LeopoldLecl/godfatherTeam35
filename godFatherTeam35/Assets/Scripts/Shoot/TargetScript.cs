using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [SerializeField] int _correspondingPlacement;
    [SerializeField] float _radius;
    bool _isAimed;

    private void Awake()
    {
        transform.position = GameManager.Instance.PlacementPosition;
    }

    private void OnEnable()
    {
        ShootingScript.HasFired += CheckFire;
    }

    private void CheckFire()
    {
        GameManager.Instance.IsPlayerHit = _isAimed;
    }

    private void OnMouseEnter() => _isAimed = true;
    private void OnMouseExit() => _isAimed = false;

}
