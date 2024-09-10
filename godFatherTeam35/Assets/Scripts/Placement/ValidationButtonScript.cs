using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ValidationButtonScript : MonoBehaviour
{
    //Si les GD veulent mettre un message si pas de cachette sélectionnée
    [Header("Events")]
    [SerializeField] UnityEvent ValidationImpossible;
    [SerializeField] UnityEvent ValidationComplete;


    public void Validation()
    {
        if(GameManager.Instance.PlacementPosition == -1)
        {
            ValidationImpossible.Invoke();
            return;
        }

        ValidationComplete.Invoke();
    }
}
