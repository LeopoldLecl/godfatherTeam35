using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private string _placementScene;

    [Header("Logo animation")]
    [SerializeField] Transform _logoTransform;
    [SerializeField] float _logoSpeed;
    [SerializeField] float _logoAmplitude;

    [Header("Character animation")]
    [SerializeField] Transform _charTransform;
    [SerializeField] float _charSpeed;
    [SerializeField] float _charAmplitude;

    private Vector3 _defaultLogoPosition;


    public void PlayGame() => SceneManager.LoadScene(_placementScene);
    public void QuitGame() => Application.Quit();


    private void Awake()
    {
        _defaultLogoPosition = _logoTransform.position;
    }

    private void Update()
    {
        //Logo movement
        Vector3 logoPosition = new Vector3(_defaultLogoPosition.x, _defaultLogoPosition.y + Mathf.Sin(Time.time * _logoSpeed) * _logoAmplitude, _defaultLogoPosition.z);
        _logoTransform.position = logoPosition;
    
        //Character movement
        Vector3 charRotation = Vector3.zero;
        charRotation.z = Mathf.Sin(Time.time * _charSpeed) * _charAmplitude;
        _charTransform.eulerAngles = charRotation;
    }
}
