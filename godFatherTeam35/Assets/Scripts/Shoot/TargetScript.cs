using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Sounds")]
    [SerializeField] List<AudioClip> _shotCharacterSound;
    [SerializeField] List<AudioClip> _shotNearMissed;
    [SerializeField] List<AudioClip> _shotMissed;

    [Header("FalseTarget")]
    [SerializeField] List<AudioClip> _falseTargetSounds;
    [SerializeField] float _minWaitingFalseTargetAnimation;
    [SerializeField] float _maxWaitingFalseTargetAnimation;
    [SerializeField] float _falseTargetNumberRotation;
    [SerializeField] float _falseTargetRotatingDegree;
    [SerializeField] float _falseTargetRotatingDuration;

    Coroutine _falseTargetCoroutine;
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
        {
            if (Random.Range(0, 2) == 0)
            {
                _sr.sprite = _falselyHiddenImage;
                _falseTargetCoroutine = StartCoroutine(FalseTargetAnimation());
            }
            else
                _sr.sprite = _normalImage;
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
                if(_falseTargetCoroutine != null)
                    StopCoroutine(_falseTargetCoroutine);
                FalseHideoutDestroyed?.Invoke();
            }
        }
    }

    IEnumerator DeathAnimationRoutine()
    {
        RightHideoutDestroyed?.Invoke();

        //Choisit un son random
        AudioClip deathSound = _shotCharacterSound[Random.Range(0, _shotCharacterSound.Count)];

        if (SoundManager.instance != null)
            SoundManager.instance.SpawnSound(deathSound,transform.position);
        
        yield return new WaitForSeconds(deathSound.length); //Attend pour la durée du son
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


    IEnumerator FalseTargetAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minWaitingFalseTargetAnimation,_maxWaitingFalseTargetAnimation));

            if (SoundManager.instance != null)
                SoundManager.instance.SpawnRandomSound(_falseTargetSounds, transform.position);

            for(int i=0; i < _falseTargetNumberRotation; i++)
            {
                float timeElapsed = 0;
                float startingPosition = transform.eulerAngles.z;
                float endPosition = transform.eulerAngles.z + _falseTargetRotatingDegree;
                while (timeElapsed < _falseTargetRotatingDuration)
                {
                    Vector3 newEulerAngles = transform.eulerAngles;
                    newEulerAngles.z = Mathf.Lerp(startingPosition, endPosition, timeElapsed / _falseTargetRotatingDuration);
                    transform.eulerAngles = newEulerAngles;
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                _falseTargetRotatingDegree *= -1;
            }

            transform.eulerAngles = Vector3.zero;
        }
    }
}
