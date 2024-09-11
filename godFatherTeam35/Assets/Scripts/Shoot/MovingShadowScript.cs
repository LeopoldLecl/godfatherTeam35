using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingShadowScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _firstPoint;
    [SerializeField] private Transform _secondPoint;
    [SerializeField] private Transform _shadowTransform;
    [Space(5)]
    [SerializeField] private float _duration;
    [SerializeField] private float _stepSpeed;
    [SerializeField] private float _stepAmplitude;

    SpriteRenderer _shadowSR;
    Coroutine _walkingCoroutine;

    private void Awake()
    {
        _walkingCoroutine = StartCoroutine(WalkingCoroutine());
        _shadowSR = _shadowTransform.gameObject.GetComponent<SpriteRenderer>();
    }

    IEnumerator WalkingCoroutine()
    {
        float timeElapsed = 0;
        Vector3 startingPosition = Vector3.zero;
        Vector3 endingPosition = _secondPoint.localPosition;
        while (true)
        {
            timeElapsed = 0;

            while (timeElapsed < _duration)
            {
                Vector2 lerpPosition;
                lerpPosition.x = Mathf.Lerp(startingPosition.x, endingPosition.x, timeElapsed / _duration);
                lerpPosition.y = Mathf.PingPong(timeElapsed * _stepSpeed, _stepAmplitude);
                _shadowTransform.localPosition = lerpPosition;

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            _shadowSR.flipX = !_shadowSR.flipX;

            Vector3 keepPosition = endingPosition;
            endingPosition = startingPosition;
            startingPosition = keepPosition;
        }

    }
}
