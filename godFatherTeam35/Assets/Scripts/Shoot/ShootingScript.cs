using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public static Action HasFired;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HasFired.Invoke();
        } 
    }
}
