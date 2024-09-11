using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] GameObject _endCanvas; //Oui le mieux cela aurait été de faire un Action mais .. game jam donc pas le temps
    [SerializeField] Animator _animator;
    [Space(5)]
    [SerializeField] int _correspondingPlacement;

    bool _isAimed;

    private void Awake()
    {
        if (GameManager.Instance != null)
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
        if (_isAimed)
        {
            GameManager.Instance.IsPlayerHit = true;
            GameManager.Instance.GameEnded = true;
            StartCoroutine(DeathAnimationRoutine());
        }
    }

    IEnumerator DeathAnimationRoutine()
    {
        if (_animator != null) 
        {
            _animator.SetTrigger("dead");
        }
        yield return new WaitForSeconds(0);
        _endCanvas.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    { 
        if (collision.GetComponent<ShooterArmScript>() != null) 
            _isAimed = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<ShooterArmScript>() != null)
            _isAimed = false;
    }

}
