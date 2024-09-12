using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementButtonScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer _validationFillImage;
    [Space(5)]
    [SerializeField] private int _placementValue;

    [Header("Images")]
    [SerializeField] private GameObject _hiddenObject;

    private SpriteRenderer _sr;
    
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
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
        if (_validationFillImage != null || buttonSpriteRenderer != null)
        {
            _validationFillImage.sprite = buttonSpriteRenderer.sprite;
        }
    }

    private void Update() //Pas opti je sais mais merde...
    {
        if (_validationFillImage != null)
        {
            _validationFillImage.gameObject.SetActive(GameManager.Instance.PlacementPositionIndex == _placementValue || GameManager.Instance.PlacementPositionIndex == -1);
        }
        if(_hiddenObject != null)
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
}
