using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementButtonScript : MonoBehaviour
{
    [Header("References")]
    [Space(5)]
    [SerializeField] private int _placementValue;

    [Header("Images")]
    [SerializeField] private GameObject _hiddenObject;
    [Space(5)]
    [SerializeField] private float _blinkingSpeed;
    [SerializeField] private Color _blinkingHigh;
    [SerializeField] private Color _blinkingLow;

    private SpriteRenderer _sr;
    private Coroutine _blinkingCoroutine;
    private bool _isBlinking;
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _blinkingCoroutine = StartCoroutine(BlinkingLoop());
        _isBlinking = true;
    }

    private void OnValidate()
    {
        if (_placementValue < 0)
        {
            Debug.LogWarning("placement value est sous 0, etes-vous sure de l'avoir définit ?", gameObject);
        }
    }

    private void Reset()
    {
        SpriteRenderer buttonSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() //Pas opti je sais mais merde...
    {
        if (_sr != null && GameManager.Instance != null)
        {
            _isBlinking = (GameManager.Instance.PlacementPositionIndex == _placementValue || GameManager.Instance.PlacementPositionIndex == -1);
        }
        if(_hiddenObject != null && GameManager.Instance != null)
            _hiddenObject.SetActive(GameManager.Instance.PlacementPositionIndex == _placementValue);
    }

    public void SelectPosition()
    {
        GameManager.Instance.PlacementPositionIndex = _placementValue;
        GameManager.Instance.PlacementPosition = transform.position;
    }
    private void AddPhysics2DRaycaster()
    {
        Physics2DRaycaster physicsRaycaster = FindObjectOfType<Physics2DRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }

    IEnumerator BlinkingLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(_blinkingSpeed);
            if(_isBlinking)
                _sr.color = _sr.color == _blinkingHigh ? _blinkingLow : _blinkingHigh;
        }
    }
}