using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    private const float maxTimer = 8f;
    private float timer = maxTimer;

    private CanvasGroup canvasGroup;
    private RectTransform rect;

    private Transform target;
    private Transform player;

    private IEnumerator iE_Countdown;
    private Action unRegister;

    private Quaternion tRot = Quaternion.identity;
    private Vector3 tPos = Vector3.zero;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
    }

    public void Register(Transform target, Transform player, Action unRegister)
    {
        this.target = target;
        this.player = player;
        this.unRegister = unRegister;

        StartCoroutine(RotateToTheTarget());
        StartTimer();
    }

    public void Restart()
    {
        timer = maxTimer;
        StartTimer();
    }

    void StartTimer()
    {
        if (iE_Countdown != null)
            StopCoroutine(iE_Countdown);

        iE_Countdown = Countdown();
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while(canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += 4 * Time.deltaTime;
            yield return null;
        }

        while(timer > 0)
        {
            timer--;
            yield return new WaitForSeconds(1f);
        }

        while(canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= 2 * Time.deltaTime;
            yield return null;
        }

        unRegister();
        Destroy(gameObject);
    }

    IEnumerator RotateToTheTarget()
    {
        while (enabled)
        {
            if (target)
            {
                tPos = target.position;
                tRot = target.rotation;
            }

            Vector3 direction = player.position - tPos;

            tRot = Quaternion.LookRotation(direction);
            tRot.z = -tRot.y;
            tRot.x = 0f;
            tRot.y = 0f;

            Vector3 northDirection = new Vector3(0, 0, player.eulerAngles.y);
            rect.localRotation = tRot * Quaternion.Euler(northDirection);

            yield return null;
        }
    }
}
