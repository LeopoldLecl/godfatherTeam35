using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementButtonScript : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private int _placementValue;
    [SerializeField] private SpriteRenderer _ValidationFillImage;

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
        if (_ValidationFillImage != null || buttonSpriteRenderer != null)
        {
            _ValidationFillImage.sprite = buttonSpriteRenderer.sprite;
        }
    }

    private void Update() //Pas opti je sais mais merde...
    {
        if (_ValidationFillImage != null)
        {
            _ValidationFillImage.gameObject.SetActive(GameManager.Instance.PlacementPositionIndex == _placementValue);
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
