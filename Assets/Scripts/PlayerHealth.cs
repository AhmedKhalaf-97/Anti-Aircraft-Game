using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;

    public float bleedingTimeLength = 0.15f;

    public AudioSource bulletsImpactAudioSource;
    public AudioClip[] bulletsImpactAudioClips;

    public PostProcessingEffects postProcessingEffects;

    public static PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = this;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        BleedingEffect();

        PlayBulletsImpactSFX();

        if (health <= 0)
            print("Player Killed");
    }

    void BleedingEffect()
    {
        postProcessingEffects.VignetteBleedingEffect();
    }

    void PlayBulletsImpactSFX()
    {
        bulletsImpactAudioSource.PlayOneShot(bulletsImpactAudioClips[Random.Range(0, bulletsImpactAudioClips.Length)]);
    }
}
