using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingEffects : MonoBehaviour
{
    public float vignetteFadingSpeed = 2f;
    public float maxVignetteIntensity = 0.6f;

    public float vignetteMaxTimer = 3f;
    private float vignetteTimer;

    bool isVignetteFading;

    private Volume postProcessingVolume;
    private Vignette _vignette;

    void Start()
    {
        postProcessingVolume = GetComponent<Volume>();

        postProcessingVolume.sharedProfile.TryGet<Vignette>(out _vignette);
    }

    public void VignetteBleedingEffect()
    {
        vignetteTimer = vignetteMaxTimer;

        if(!isVignetteFading)
            StartCoroutine(FadeVignetteEffectCoroutine());
    }

    IEnumerator FadeVignetteEffectCoroutine()
    {
        isVignetteFading = true;

        while(vignetteTimer > 0f)
        {
            vignetteTimer -= Time.deltaTime;

            FadeInVignetteEffect();

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        while (vignetteTimer <= 0f && _vignette.intensity.value > 0.05f)
        {
            FadeOutVignetteEffect();

            yield return null;
        }

        isVignetteFading = false;
    }

    void FadeInVignetteEffect()
    {
        _vignette.active = true;
        _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, maxVignetteIntensity, vignetteFadingSpeed * Time.deltaTime);
    }

    void FadeOutVignetteEffect()
    {
        _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, 0f, vignetteFadingSpeed * Time.deltaTime);

        if(_vignette.intensity.value <= 0.05f)
            _vignette.active = false;
    }
}
