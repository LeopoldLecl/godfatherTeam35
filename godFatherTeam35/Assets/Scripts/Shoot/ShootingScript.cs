using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShooterArmScript _armScript;
    [SerializeField] private placeholderShotScript _shaderShotScript;
    [Space(5)]
    [SerializeField] private float _reloadingTime;
    [Header("Sounds")]
    [SerializeField] private List<AudioClip> _fireSounds;
    [SerializeField] private List<AudioClip> _shootSound;

    private bool _isReloading { get => _armScript.IsReloading; }

    public static Action HasFired;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.GameEnded && !_isReloading)
        {
            HasFired.Invoke();


            if (SoundManager.instance != null)
            {
                SoundManager.instance.SpawnRandomSound(_fireSounds, transform.position);
                SoundManager.instance.SpawnRandomSound(_shootSound, transform.position);

            }
            
            _shaderShotScript.ActivateShoot();
            _armScript.StartReload(_reloadingTime);
        }

        
    }
}
