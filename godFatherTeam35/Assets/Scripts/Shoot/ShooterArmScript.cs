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

    [Header("Shaking")]
    [SerializeField] float _shakeDelay;
    [SerializeField] float _shakeAmplitude;
    [SerializeField] float _shakeMinDistance;

    Coroutine _followMouse;
    Coroutine _reloadCoroutine;
    Coroutine _shakingCoroutine;
    private bool _isReloading;
    private float _actualRandomAmplitude;
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

    IEnumerator FollowMouse()
    {
        while (true)
        {
            Vector3 mousePo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePo.z = 0;

            //Shaking condition
            if (Vector2.Distance(transform.position, mousePo) < _shakeMinDistance)
            {
                if (_shakingCoroutine == null)
                    _shakingCoroutine = StartCoroutine(ShakingDelay());
            }
            else
            {
                if(_shakingCoroutine != null)
                {
                    StopCoroutine(_shakingCoroutine);
                    _shakingCoroutine = null;
                }
                _actualRandomAmplitude = 0f;
            }
            //Add randomness to shaking
            mousePo += new Vector3(Random.Range(-_actualRandomAmplitude, _actualRandomAmplitude), Random.Range(-_actualRandomAmplitude, _actualRandomAmplitude), 0);

            _rb.MovePosition(mousePo);
            _rb.SetRotation(_armTransform.eulerAngles.z);

            Vector2 newEndPosition = new Vector2(Camera.main.transform.position.x, transform.position.y /2.0f - 8.7f);
            _rbEndPoint.MovePosition(newEndPosition);

            yield return null;
        }
    }

    IEnumerator ShakingDelay()
    {
        _actualRandomAmplitude = 0;
        yield return new WaitForSeconds(_shakeDelay);
        _actualRandomAmplitude = _shakeAmplitude;
    }

    IEnumerator ReloadAnimation(float duration)
    {
        _isReloading = true;
        StopCoroutine(_followMouse);

        //Rangement d'arme
        float timeElapsed = 0;
        Vector3 startingPosition = transform.position;
        Vector3 endingPosition = _reloadPoint.position;
        Vector3 startingEndPointPosition = _rbEndPoint.position;

        while (timeElapsed < (duration * .25f))
        {
            _rb.MovePosition(Vector3.Lerp(startingPosition, endingPosition, timeElapsed / (duration *.25f)));

            Vector2 newEndPosition = new Vector2(Camera.main.transform.position.x, -8.7f);
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

            Vector2 newEndPosition = new Vector2(Camera.main.transform.position.x, transform.position.y / 2.0f - 8.7f);
            _rbEndPoint.MovePosition(Vector3.Lerp(startingEndPointPosition, newEndPosition, timeElapsed / (duration * .25f)));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _isReloading = false;
        _followMouse = StartCoroutine(FollowMouse());
    }
}
