using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicatorSystem : MonoBehaviour
{
    public DamageIndicator indicatorPrefab;
    public RectTransform holder;
    public Camera mainCamera;
    public Transform player;

    private Dictionary<Transform, DamageIndicator> indicators = new Dictionary<Transform, DamageIndicator>();

    public static Action<Transform> createIndicator;
    public static Func<Transform, bool> checkIfObjectInSight;

    private void OnEnable()
    {
        createIndicator += Create;
        checkIfObjectInSight += IsInSight;
    }

    private void OnDisable()
    {
        createIndicator -= Create;
        checkIfObjectInSight -= IsInSight;
    }

    void Create(Transform target)
    {
        if (indicators.ContainsKey(target))
        {
            indicators[target].Restart();
            return;
        }

        DamageIndicator newIndicator = Instantiate(indicatorPrefab, holder);
        newIndicator.Register(target, player, new Action(() => {indicators.Remove(target); }));

        indicators.Add(target, newIndicator);
    }

    bool IsInSight(Transform t)
    {
        if (t == null)
            return false;

        Vector3 screenPoint = mainCamera.WorldToViewportPoint(t.position);

        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}
