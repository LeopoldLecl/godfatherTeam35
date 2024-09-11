using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingCameraScript : MonoBehaviour
{
    [SerializeField] bool _RandomSpawnX;

    [Header("Camera Limit")]
    [SerializeField] float _leftLimit;
    [SerializeField] float _rightLimit;
    [SerializeField] float _topLimit;
    [SerializeField] float _bottomLimit;

    [Header("Camera Deadzone")]
    [Range(0f, 1f)]
    [SerializeField] float _leftDeadZone;
    [Range(0f, 1f)]
    [SerializeField] float _rightDeadZone;
    [Range(0f, 1f)]
    [SerializeField] float _topDeadZone;
    [Range(0f, 1f)]
    [SerializeField] float _bottomDeadZone;

    [Header("Camera Speed")]
    [SerializeField] float _horizontalSpeed;
    [SerializeField] float _verticalSpeed;

    private void Start()
    {
        if (_RandomSpawnX)
        {
            transform.position = new Vector2(Random.Range(_leftLimit, _rightLimit),0);
        }
    }

    private void Update()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);

        Vector2 mouseDirection = new Vector2();
        if(mousePosition.x < _leftDeadZone && transform.position.x > _leftLimit)
        {
            mouseDirection -= new Vector2(_horizontalSpeed, 0);
        }
        else if (mousePosition.x > _rightDeadZone && transform.position.x < _rightLimit)
        {
            mouseDirection += new Vector2(_horizontalSpeed, 0);
        }

        if (mousePosition.y < _topDeadZone && transform.position.y > _topLimit)
        {
            mouseDirection -= new Vector2(0, _verticalSpeed);
        }
        else if (mousePosition.y > _bottomDeadZone && transform.position.y < _bottomLimit)
        {
            mouseDirection += new Vector2(0, _verticalSpeed);
        }

        if (mouseDirection != Vector2.zero)
        {
            transform.position += new Vector3(mouseDirection.x, mouseDirection.y, 0);
        }
    }
}
