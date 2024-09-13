using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceCrowdSound : MonoBehaviour
{
    [SerializeField] private AudioClip _crowdSound;
    private void Start()
    {
        if(SoundManager.instance != null)
        {
            GameObject ambiance = SoundManager.instance.SpawnLoopingSound(_crowdSound);
            ambiance.transform.SetParent(transform);
        }
    }
}
