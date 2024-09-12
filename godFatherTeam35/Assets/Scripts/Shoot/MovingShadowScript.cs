using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingShadowScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Sprite> _imagesList;
    [SerializeField] private Transform _firstPoint;
    [SerializeField] private Transform _secondPoint;
    [SerializeField] private Transform _shadowTransform;
    [SerializeField] private float _ZPosition;
    [Space(5)]
    [SerializeField] private _animationMode _WalkingMode;
    [SerializeField] private float _duration;
    [SerializeField] private float _stepSpeed;
    [SerializeField] private float _stepAmplitude;
    [SerializeField] private float _TimeBetweenCycles;
    [SerializeField] private List<AudioClip> _movingSounds;

    SpriteRenderer _shadowSR;
    Coroutine _walkingCoroutine;

    private enum _animationMode
    {
        BackAndForth,
        BackAndForth_inverted,
        Looping,
        Looping_inverted
    }

    private void Awake()
    {
        _shadowSR = _shadowTransform.gameObject.GetComponent<SpriteRenderer>();
        _walkingCoroutine = StartCoroutine(WalkingCoroutine());
    }

    IEnumerator WalkingCoroutine()
    {
        float timeElapsed = 0;
        Vector3 startingPosition = Vector3.zero;
        Vector3 endingPosition = _secondPoint.localPosition;
        _shadowSR.sprite = _imagesList[Random.Range(0, _imagesList.Count)];

        //Initialization of walking mode
        switch (_WalkingMode)
        {
            case _animationMode.BackAndForth:
            case _animationMode.Looping:
                timeElapsed = 0;
                startingPosition = Vector3.zero;
                endingPosition = _secondPoint.localPosition;
                break;

            case _animationMode.BackAndForth_inverted:
            case _animationMode.Looping_inverted:
                startingPosition = _secondPoint.localPosition;
                endingPosition = Vector3.zero;
                _shadowSR.flipX = true;
                break;
        }



        while (true)
        {
            if (SoundManager.instance != null)
                SoundManager.instance.SpawnRandomSound(_movingSounds,transform.position);

            timeElapsed = 0;
            //Walk animation
            while (timeElapsed < _duration)
            {
                Vector3 lerpPosition;
                lerpPosition.x = Mathf.Lerp(startingPosition.x, endingPosition.x, timeElapsed / _duration);
                lerpPosition.y = Mathf.PingPong(timeElapsed * _stepSpeed, _stepAmplitude);
                lerpPosition.z = _ZPosition;
                _shadowTransform.localPosition = lerpPosition;

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            //Change depending on walk mode
            switch (_WalkingMode)
            {
                case _animationMode.BackAndForth_inverted:
                case _animationMode.BackAndForth:
                    Vector3 keepPosition = endingPosition;
                    endingPosition = startingPosition;
                    startingPosition = keepPosition;
                    _shadowSR.flipX = !_shadowSR.flipX;
                    break;

                case _animationMode.Looping:
                    timeElapsed = 0;
                    startingPosition = Vector3.zero;
                    endingPosition = _secondPoint.localPosition;
                    break;
                case _animationMode.Looping_inverted:
                    startingPosition = _secondPoint.localPosition;
                    endingPosition = Vector3.zero;
                    break;
            }

            //Wait if necessary
            _shadowSR.gameObject.SetActive(false);
            yield return new WaitForSeconds(_TimeBetweenCycles);
            _shadowSR.gameObject.SetActive(true);
            _shadowSR.sprite = _imagesList[Random.Range(0, _imagesList.Count)];
        }

    }
}
