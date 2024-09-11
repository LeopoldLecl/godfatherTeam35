using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoHideoutPanelScript : MonoBehaviour
{
    private Coroutine _FadeOutCoroutine;
    [SerializeField] private float _duration;
    [SerializeField] private Image _image;

    private void Reset()
    {
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _FadeOutCoroutine = StartCoroutine(FadeInEnumerator(_duration));
    }

    IEnumerator FadeInEnumerator(float duration)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            _image.color = new Color(1f,1f,1f, 1 - timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
