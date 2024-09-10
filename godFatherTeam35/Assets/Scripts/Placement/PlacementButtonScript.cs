using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementButtonScript : MonoBehaviour
{
    [SerializeField] private int _placementValue;
    [SerializeField] private Image _ValidationFillImage;

    private void Reset()
    {
        if (_placementValue <= 0) 
        {
            Debug.LogWarning("placement value est egale ou sous 0, etes-vous sure de l'avoir définit ?", gameObject);
        }


        Image buttonImg = GetComponent<Image>();
        if (_ValidationFillImage != null || buttonImg != null)
        {
            _ValidationFillImage.sprite = buttonImg.sprite;
        }
    }

    private void Update() //Pas opti je sais mais merde...
    {
        if (_ValidationFillImage != null)
        {
            _ValidationFillImage.gameObject.SetActive(GameManager.Instance.PlacementPosition == _placementValue);
        }
    }

    public void SelectPosition()
    {
        GameManager.Instance.PlacementPosition = _placementValue;
    }
}
