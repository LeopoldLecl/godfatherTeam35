using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterArmScript : MonoBehaviour
{
    Coroutine _followMouse;
    private void Awake()
    {
        _followMouse = StartCoroutine(FollowMouse());
    }
    IEnumerator FollowMouse()
    {
        while (!GameManager.Instance.GameEnded)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = mousePosition;

            yield return null;
        }
    }
}
