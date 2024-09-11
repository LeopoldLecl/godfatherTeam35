using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class TargetScript : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] GameObject _endCanvas; //Oui le mieux cela aurait été de faire un Action mais .. game jam donc pas le temps
    [Space(5)]
    [SerializeField] int _correspondingPlacement;

    [Header("Images")]
    [SerializeField] Sprite _normalImage;
    [SerializeField] Sprite _hiddenImage;
    [SerializeField] Sprite _falselyHiddenImage;

    [Header("Events")]
    [SerializeField] UnityEvent RightHideoutDestroyed;
    [SerializeField] UnityEvent FalseHideoutDestroyed;

    SpriteRenderer _sr;
    
    private bool _isAimed;
    private bool _isTarget;
    private bool _isDestroyed = false;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();

        if (GameManager.Instance != null)
            _isTarget = GameManager.Instance.PlacementPositionIndex == _correspondingPlacement;

        if (_isTarget)
            _sr.sprite = _hiddenImage;
        else
            _sr.sprite = Random.Range(0, 2) == 0 ? _normalImage : _falselyHiddenImage;
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
        if (_isAimed && !_isDestroyed)
        {
            if (_isTarget)
            {
                GameManager.Instance.IsPlayerHit = true;
                GameManager.Instance.GameEnded = true;
                StartCoroutine(DeathAnimationRoutine());
            }
            else
            {
                _isDestroyed = true;
                FalseHideoutDestroyed?.Invoke();
            }
        }
    }

    IEnumerator DeathAnimationRoutine()
    {
        RightHideoutDestroyed?.Invoke();
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
