using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private PlacementButtonScript _overlapedButton;

    private Vector2 _startingPosition;
    private Vector3 _offset;

    public static Action<GameObject> HasChosePlacement;

    private void Awake()
    {
        _startingPosition = transform.position;
    }

    private void OnEnable()
    {
        //Reset position when (re)activated
        transform.position = _startingPosition;
    }

    private void OnMouseDown()
    {
        _offset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + _offset;
    }

    private void OnMouseUp()
    {
        //Si pas de placement
        if (_overlapedButton == null)
        {
            transform.position = _startingPosition;
            return;
        }

        HasChosePlacement.Invoke(gameObject);
        _overlapedButton.SelectPosition();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlacementButtonScript overlapedPlacementScript = collision.GetComponent<PlacementButtonScript>();
        if (overlapedPlacementScript != null)
        {
            _overlapedButton = overlapedPlacementScript;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _overlapedButton = null;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = transform.position.z;
        var result = Camera.main.ScreenToWorldPoint(mousePoint);
        return result;
    }
}
