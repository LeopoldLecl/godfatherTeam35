using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlacementUIScript : MonoBehaviour
{

    private GameObject _character;
    private bool _canCancel;

    private void Awake()
    {
        _canCancel = false;
    }

    private void OnEnable()
    {
        Drag.HasChosePlacement += SpawnCancelButton;
    }

    private void OnDisable()
    {
        Drag.HasChosePlacement -= SpawnCancelButton;
    }


    private void SpawnCancelButton(GameObject character)
    {
        _character = character;
        _canCancel = true;
    }

    private void Update()
    {
        if (_canCancel && Input.GetMouseButtonDown(1))
        {
            CancelPosition();
        }
    }

    public void CancelPosition()
    {
        GameManager.Instance.PlacementPositionIndex = -1;
        _character.SetActive(true);
        _canCancel = false;
    }
}
