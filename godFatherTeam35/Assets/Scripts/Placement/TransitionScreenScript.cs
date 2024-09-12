using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionScreenScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _fadeInImg;
    [SerializeField] private string _ShootScene;
    [Space(5)]
    [SerializeField] private float _duration;
    private Coroutine _fadeInCoroutine;
    private bool _transitionAnimationFinished;

    void Awake()
    {
        _transitionAnimationFinished = false;
    }

    private void Reset()
    {
        _fadeInImg = GetComponent<Image>();
    }

    public void OnEnable()
    {
        _fadeInCoroutine = StartCoroutine(FadeInEnumerator(_duration));
    }

    private void Update()
    {
        if (_transitionAnimationFinished && Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene(_ShootScene);
        }
    }

    IEnumerator FadeInEnumerator(float duration)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            _fadeInImg.color = new Color(0f, 0f, 0f, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _fadeInImg.color = new Color(0f, 0f, 0f, 1f);
        _transitionAnimationFinished = true;
    }
}
