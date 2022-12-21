using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameobject : MonoBehaviour
{
    readonly float destroyTime = 5f;

    public AudioClip[] explosionAudioClips;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayExplosionSFX();

        DestroyThisGameobject();
    }

    void PlayExplosionSFX()
    {
        if (audioSource == null)
            return;

        audioSource.clip = explosionAudioClips[Random.Range(0, explosionAudioClips.Length)];

        audioSource.Play();
    }

    void DestroyThisGameobject()
    {
        Destroy(gameObject, destroyTime);
    }
}
