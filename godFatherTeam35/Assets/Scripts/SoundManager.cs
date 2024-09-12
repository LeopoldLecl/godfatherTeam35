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

    public void SpawnSound(AudioClip clip, Vector3 position)
    {
        GameObject newSound = Instantiate(m_noisePrefab, position, Quaternion.identity);

        AudioSource newSoundSource = newSound.GetComponent<AudioSource>();
        newSoundSource.clip = clip;
        newSoundSource.Play();

        Destroy(newSound, clip.length);
    }

    public void SpawnRandomSound(List<AudioClip> clipList, Vector3 position)
    {
        SpawnSound(clipList[Random.Range(0, clipList.Count)], position);
    }
}
