using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementButtonScript : MonoBehaviour, IPointerDownHandler
{
    [Header("References")]
    [SerializeField] private SpriteRenderer _validationFillImage;
    [SerializeField] private Animator _animator;
    [Space(5)]
    [SerializeField] private int _placementValue;

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
            //Bool dans les 2 sens
            if(_animator != null)
                _animator.SetBool("hidden", GameManager.Instance.PlacementPositionIndex == _placementValue);
        }
    }

    public void SelectPosition()
    {
        GameManager.Instance.PlacementPositionIndex = _placementValue;
        GameManager.Instance.PlacementPosition = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectPosition();
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
