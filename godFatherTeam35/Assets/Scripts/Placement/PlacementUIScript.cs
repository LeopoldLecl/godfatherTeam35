using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlacementUIScript : MonoBehaviour
{
    [SerializeField] private GameObject _CancelButton;

    private GameObject _character;
    private bool _canCancel;

    private void Awake()
    {
        _CancelButton.SetActive(false);
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
        _CancelButton.SetActive(true);
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
        _CancelButton.SetActive(false);
        _canCancel = false;
    }
}
