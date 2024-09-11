using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShooterArmScript _armScript;
    [Space(5)]
    [SerializeField] private float _reloadingTime;

    private bool _isReloading { get => _armScript.IsReloading; }

    public static Action HasFired;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.GameEnded && !_isReloading)
        {
            HasFired.Invoke();
            _armScript.StartReload(_reloadingTime);
        } 
    }
}
