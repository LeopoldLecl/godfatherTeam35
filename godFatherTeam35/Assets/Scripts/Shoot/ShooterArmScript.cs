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

    Coroutine _followMouse;
    Coroutine _reloadCoroutine;
    private bool _isReloading;
    public bool IsReloading { get => _isReloading;}

    private void Awake()
    {
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
            _rb.MovePosition(mousePo);
            _rb.SetRotation(_armTransform.eulerAngles.z);

            Vector2 newEndPosition = new Vector2(Camera.main.transform.position.x, transform.position.y /2.0f - 8.7f);
            _rbEndPoint.MovePosition(newEndPosition);

            yield return null;
        }
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
