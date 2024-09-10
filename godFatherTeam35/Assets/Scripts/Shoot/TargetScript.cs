using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] GameObject _endCanvas; //Oui le mieux cela aurait été de faire un Action mais .. game jam donc pas le temps
    [Space(5)]
    [SerializeField] int _correspondingPlacement;
    [SerializeField] float _radius;
    bool _isAimed;

    private void Awake()
    {
        if(GameManager.Instance != null)
        {
            transform.position = GameManager.Instance.PlacementPosition;
        }
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
        GameManager.Instance.IsPlayerHit = _isAimed;
        GameManager.Instance.GameEnded = true;
        _endCanvas.SetActive(true);
    }

    private void OnMouseEnter() => _isAimed = true;
    private void OnMouseExit() => _isAimed = false;

}
