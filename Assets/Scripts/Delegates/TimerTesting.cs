using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTesting : MonoBehaviour
{
    public ActionOnTimer actionOnTimer;

    private void Start()
    {
        actionOnTimer.SetTimer(4f, () => { Debug.Log("Timer Has Elapsed"); });
    }
}
