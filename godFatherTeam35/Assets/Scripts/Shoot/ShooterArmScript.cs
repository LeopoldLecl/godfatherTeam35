using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterArmScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Rigidbody2D _rbEndPoint;
    [SerializeField] Transform _armTransform;
    [SerializeField] Transform _rotationPoint;
    [SerializeField] Transform _reloadPoint;

    [Header("EndPointParameters")]
    [SerializeField] float _endPointDefaultYPosition;
    [SerializeField] float _endPointYFollow;
    [SerializeField] float _endPointXFollow;

    [Header("Shaking")]
    [SerializeField] float _shakeDelay;
    [SerializeField] float _shakeMinDistance;
    [Space(2)]
    [SerializeField] float _shakeRate;
    [SerializeField] float _shakeSpeed;
    [SerializeField] float _shakeLimitDistance;

    private bool _isReloading;

    private Vector3 _shakePosition;
    private Vector3 _shakeDirection;
    private bool _isShaking;

    Coroutine _followMouse;
    Coroutine _reloadCoroutine;
    Coroutine _shakingCoroutine;

    public bool IsReloading { get => _isReloading;}

    private void Awake()
    {
        Cursor.visible = false;

        _followMouse = StartCoroutine(FollowMouse());
        _isReloading = false;
    }

    public void StartReload(float duration)
    {
        _reloadCoroutine = StartCoroutine(ReloadAnimation(duration));
    }

    private void Update()
    {
        print(_shakePosition);
    }

    IEnumerator FollowMouse()
    {
        while (true)
        {
            Vector3 mousePo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePo.z = 0;

            //Shaking trigger condition
            if (Vector2.Distance(transform.position, mousePo) < _shakeMinDistance)
            {
                if (_shakingCoroutine == null)
                    _shakingCoroutine = StartCoroutine(ShakingDelay());
                _isShaking = true;
            }
            else
            {
                if(_shakingCoroutine != null)
                {
                    StopCoroutine(_shakingCoroutine);
                    _shakingCoroutine = null;
                    _isShaking = false;
                }
            }
            //Random smooth shaking
            if(_isShaking)
            {
                //Apply shaking
                _shakePosition += _shakeDirection * _shakeSpeed * Time.deltaTime;
                mousePo += _shakePosition;
                
                //Change direction & clamp if going too far
                if(_shakePosition.x > _shakeLimitDistance || _shakePosition.y < -_shakeLimitDistance || _shakePosition.x < -_shakeLimitDistance || _shakePosition.y > _shakeLimitDistance)
                {
                    _shakeDirection *= -1;
                    _shakePosition = new Vector2( Mathf.Clamp(_shakePosition.x, -_shakeLimitDistance, _shakeLimitDistance), 
                                                  Mathf.Clamp(_shakePosition.y, - _shakeLimitDistance, _shakeLimitDistance));
                }
            }

            _rb.MovePosition(mousePo);
            _rb.SetRotation(_armTransform.eulerAngles.z);

            Vector2 newEndPosition = new Vector2(transform.position.x / _endPointXFollow, transform.position.y / _endPointYFollow + _endPointDefaultYPosition);
            _rbEndPoint.MovePosition(newEndPosition);

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ShakingDelay()
    {
        _shakePosition = Vector3.zero;
        yield return new WaitForSeconds(_shakeDelay);

        //Change direction randomly in function of Rate
        while (true)
        {
            _shakeDirection = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
            yield return new WaitForSeconds(_shakeRate);
        }
    }

    IEnumerator ReloadAnimation(float duration)
    {
        _isReloading = true;
        StopCoroutine(_followMouse);
        if (_shakingCoroutine != null)
        {
            StopCoroutine(_shakingCoroutine);
            _shakingCoroutine = null;
            _isShaking = false;
        }

        //Rangement d'arme
        float timeElapsed = 0;
        Vector3 startingPosition = transform.position;
        Vector3 endingPosition = _reloadPoint.position;
        Vector3 startingEndPointPosition = _rbEndPoint.position;

        while (timeElapsed < (duration * .25f))
        {
            _rb.MovePosition(Vector3.Lerp(startingPosition, endingPosition, timeElapsed / (duration *.25f)));

            Vector2 newEndPosition = new Vector2(Camera.main.transform.position.x, _endPointDefaultYPosition);
            _rbEndPoint.MovePosition(Vector3.Lerp(startingEndPointPosition, newEndPosition, timeElapsed / (duration * .25f)));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //Attente de rechargement
        yield return new WaitForSeconds(duration / 2f);

        //Degaine l'arme
        timeElapsed = 0;
        startingPosition = _reloadPoint.position;
        startingEndPointPosition = _rbEndPoint.position;
        while (timeElapsed < (duration * .25f))
        {
            Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MousePosition.z = 0;
            _rb.MovePosition(Vector3.Lerp(startingPosition, MousePosition, timeElapsed / (duration * .25f)));

            Vector2 newEndPosition = new Vector2(transform.position.x / _endPointXFollow, transform.position.y / _endPointYFollow + _endPointDefaultYPosition);
            _rbEndPoint.MovePosition(Vector3.Lerp(startingEndPointPosition, newEndPosition, timeElapsed / (duration * .25f)));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _isReloading = false;
        _followMouse = StartCoroutine(FollowMouse());
    }
}
