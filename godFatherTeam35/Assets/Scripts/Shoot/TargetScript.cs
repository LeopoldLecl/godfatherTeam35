using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [SerializeField] int _correspondingPlacement;
    [SerializeField] float _radius;

    private void OnEnable()
    {
        ShootingScript.HasFired += CheckFire;
    }

    private void CheckFire()
    {
        throw new NotImplementedException();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
