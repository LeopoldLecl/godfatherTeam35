using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private GameObject m_noisePrefab;
    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
    }

    public void SpawnSound(Transform position, AudioClip clip)
    {
        GameObject newSound = Instantiate(m_noisePrefab, position.position, position.rotation);

        AudioSource newSoundSource = newSound.GetComponent<AudioSource>();
        newSoundSource.clip = clip;
        newSoundSource.Play();

        Destroy(newSound, clip.length);
    }
}
