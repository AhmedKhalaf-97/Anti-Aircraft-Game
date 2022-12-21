using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour
{
    public float hitmarkerInterval = 0.2f;

    private Image image;
    private Transform myTransform;

    public static HitMarker hitMarker;

    private void Awake()
    {
        hitMarker = this;

        image = GetComponent<Image>();
        myTransform = transform;

        image.enabled = false;
    }

    public void TargetHit()
    {
        image.color = Color.white;

        StartCoroutine(HitMarkerBlink());
    }

    public void TargetDestroyed()
    {
        image.color = Color.red;

        StartCoroutine(HitMarkerBlink());
    }

    IEnumerator HitMarkerBlink()
    {
        image.enabled = true;

        myTransform.localScale = Vector3.one;
        yield return new WaitForSeconds(hitmarkerInterval * 0.25f);

        myTransform.localScale = new Vector3(1.25f, 1.25f, 1f);
        yield return new WaitForSeconds(hitmarkerInterval *  0.25f);

        myTransform.localScale = Vector3.one;
        yield return new WaitForSeconds(hitmarkerInterval * 0.25f);
        
        image.enabled = false;
    }
}
