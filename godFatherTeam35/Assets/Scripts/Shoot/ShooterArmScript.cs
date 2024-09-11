using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterArmScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _armTransform;
    [SerializeField] Transform _rotationPoint;

    Coroutine _followMouse;

    private void Awake()
    {
        _followMouse = StartCoroutine(FollowMouse());
    }
    IEnumerator FollowMouse()
    {
        while (!GameManager.Instance.GameEnded)
        {
            /* Version avec transform position (obselete)
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = mousePosition;
            */

            Vector3 mousePo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePo.z = 0;
            _rb.MovePosition(mousePo);
            _rb.SetRotation(_armTransform.eulerAngles.z); 

            yield return null;
        }
    }
}
