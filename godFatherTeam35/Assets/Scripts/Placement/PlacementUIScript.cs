using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlacementUIScript : MonoBehaviour
{
    [SerializeField] private GameObject _CancelButton;

    private GameObject _character;

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
    }

    public void CancelPosition()
    {
        GameManager.Instance.PlacementPositionIndex = -1;
        _character.SetActive(true);
    }
}
