using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        if (clip != null)
        {
            GameObject newSound = Instantiate(m_noisePrefab, position, Quaternion.identity);

            AudioSource newSoundSource = newSound.GetComponent<AudioSource>();
            newSoundSource.clip = clip;
            newSoundSource.Play();

            Destroy(newSound, clip.length);
        }
    }

    public void SpawnRandomSound(List<AudioClip> clipList, Vector3 position)
    {
        SpawnSound(clipList[Random.Range(0, clipList.Count)], position);
    }

    public GameObject SpawnLoopingSound(AudioClip clip)
    {
        if (clip != null)
        {
            GameObject newSound = Instantiate(m_noisePrefab, Vector2.zero, Quaternion.identity);

            AudioSource newSoundSource = newSound.GetComponent<AudioSource>();
            newSoundSource.clip = clip;
            newSoundSource.loop = true;
            newSoundSource.Play();

            return newSound;
        }
        return null;
    }
}
