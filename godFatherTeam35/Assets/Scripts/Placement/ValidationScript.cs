using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ValidationScript : MonoBehaviour
{
    //Si les GD veulent mettre un message si pas de cachette sélectionnée
    [Header("Events")]
    [SerializeField] UnityEvent ValidationImpossible;
    [SerializeField] UnityEvent ValidationComplete;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Validation();
        }
    }
    public void Validation()
    {
        if(GameManager.Instance.PlacementPositionIndex == -1)
        {
            ValidationImpossible.Invoke();
            return;
        }

        ValidationComplete.Invoke();
    }
}
