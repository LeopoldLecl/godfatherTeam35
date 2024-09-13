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
    [SerializeField] GameObject _hiddenObject;
    [SerializeField] GameObject _falselyHiddenObject;

    [Header("Events")]
    [SerializeField] UnityEvent RightHideoutDestroyed;
    [SerializeField] UnityEvent FalseHideoutDestroyed;

    [Header("Sounds")]
    [SerializeField] List<AudioClip> _shotCharacterSound;
    [SerializeField] List<AudioClip> _shotNearMissedSound;
    [SerializeField] List<AudioClip> _shotMissedSound;
    [SerializeField] List<AudioClip> _FalseHideOutShot;
    [SerializeField] AudioClip _shotCharS;
    [SerializeField] float _nearMissedShotDistance;

    [Header("FalseTarget")]
    [SerializeField] List<AudioClip> _falseTargetSounds;
    [SerializeField] float _minWaitingFalseTargetAnimation;
    [SerializeField] float _maxWaitingFalseTargetAnimation;
    [SerializeField] float _falseTargetNumberRotation;
    [SerializeField] float _falseTargetRotatingDegree;
    [SerializeField] float _falseTargetRotatingDuration;
    [SerializeField] bool _growInsteadOfShake;

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

        _hiddenObject.SetActive(_isTarget);
        _falselyHiddenObject.SetActive(!_isTarget);

        _falseTargetCoroutine = StartCoroutine(FalseTargetAnimation());
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
                return;
            }
            else
            {
                _isDestroyed = true;
                _falselyHiddenObject.SetActive(false);
                if (_falseTargetCoroutine != null)
                    StopCoroutine(_falseTargetCoroutine);

                SoundManager.instance.SpawnRandomSound(_FalseHideOutShot, transform.position);
                FalseHideoutDestroyed?.Invoke();
                return;
            }
        }

        if (_isTarget)
        {
            Vector3 mousePo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePo.z = 0;

            float distance = Vector2.Distance(mousePo, transform.position);
            if(distance < _nearMissedShotDistance)
            {
                if (SoundManager.instance != null)
                    SoundManager.instance.SpawnRandomSound(_shotNearMissedSound, transform.position);
            }
            else
            {
                if (SoundManager.instance != null)
                    SoundManager.instance.SpawnRandomSound(_shotMissedSound, transform.position);
            }
        }
    }

    IEnumerator DeathAnimationRoutine()
    {
        RightHideoutDestroyed?.Invoke();

        //Choisit un son random
        AudioClip deathSound = _shotCharacterSound[Random.Range(0, _shotCharacterSound.Count)];        

        if (SoundManager.instance != null)
        {
            SoundManager.instance.SpawnSound(deathSound,transform.position);
            SoundManager.instance.SpawnSound(_shotCharS,transform.position);

        }
        
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


            Vector3 startingSize = transform.localScale;
            Vector3 endSize = transform.localScale + new Vector3(_falseTargetRotatingDegree, _falseTargetRotatingDegree);
            for (int i=0; i < _falseTargetNumberRotation; i++)
            {
                float timeElapsed = 0;
                float startingPosition = transform.eulerAngles.z;
                float endPosition = transform.eulerAngles.z + _falseTargetRotatingDegree;

                while (timeElapsed < _falseTargetRotatingDuration)
                {
                    if (_growInsteadOfShake)
                    {
                        //Growing shaking
                        transform.localScale = Vector3.Lerp(startingSize, endSize, timeElapsed / _falseTargetRotatingDuration);
                        timeElapsed += Time.deltaTime;
                        yield return null;
                    }
                    else
                    {
                        //Rotation shaking
                        Vector3 newEulerAngles = transform.eulerAngles;
                        newEulerAngles.z = Mathf.Lerp(startingPosition, endPosition, timeElapsed / _falseTargetRotatingDuration);
                        transform.eulerAngles = newEulerAngles;
                        timeElapsed += Time.deltaTime;
                        yield return null;
                    }
                }
                _falseTargetRotatingDegree *= -1;
            }

            transform.eulerAngles = Vector3.zero;
            transform.localScale = startingSize;
        }
    }
}
