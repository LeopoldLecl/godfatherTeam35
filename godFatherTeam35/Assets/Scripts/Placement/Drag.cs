using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] List<AudioClip> placedSounds;
    [SerializeField] Sprite _normalSprite;
    [SerializeField] Sprite _draggedImage;

    public PlacementButtonScript _overlapedButton;
    private SpriteRenderer _sr;
    private Vector2 _startingPosition;
    private Vector3 _offset;
    private bool _lockOnCamera;

    public static Action<GameObject> HasChosePlacement;


    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();

        _startingPosition = transform.position;
        _lockOnCamera = true;
    }

    private void OnEnable()
    {
        //Reset position when (re)activated
        transform.position = _startingPosition;
        _sr.sprite = _normalSprite;

        //Si pas lock sur Camera -> alors repositionnement
        if (!_lockOnCamera)
        {
            _lockOnCamera = true;

            if (SoundManager.instance != null)
                SoundManager.instance.SpawnRandomSound(placedSounds, transform.position);
        }
    }

    private void Update()
    {
        if (_lockOnCamera)
        {
            transform.position = _startingPosition + new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        }
    }

    private void OnMouseDown()
    {
        _offset = transform.position - GetMouseWorldPos();
        _lockOnCamera = false;
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + _offset;
        _sr.sprite = _draggedImage;
    }

    private void OnMouseUp()
    {
        //Si pas de placement
        if (_overlapedButton == null)
        {
            transform.position = _startingPosition + new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
            _lockOnCamera = true;
            _sr.sprite = _normalSprite;
            return;
        }
        
        HasChosePlacement.Invoke(gameObject);
        _overlapedButton.SelectPosition();
        gameObject.SetActive(false);

        if (SoundManager.instance != null)
            SoundManager.instance.SpawnRandomSound(placedSounds, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlacementButtonScript overlapedPlacementScript = collision.GetComponent<PlacementButtonScript>();
        if (overlapedPlacementScript != null)
        {
            _overlapedButton = overlapedPlacementScript;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlacementButtonScript overlapedPlacementScript = collision.GetComponent<PlacementButtonScript>();
        if (overlapedPlacementScript == _overlapedButton)
            _overlapedButton = null;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = transform.position.z;
        var result = Camera.main.ScreenToWorldPoint(mousePoint);
        return result;
    }
}
