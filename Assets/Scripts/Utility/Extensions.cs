using System;
using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static void Delay(this MonoBehaviour original, float secondsToDelay, Action act)
    {
        original.StartCoroutine(delayAction(secondsToDelay, act));
    }

    private static IEnumerator delayAction(float secondsToDelay, Action act)
    {
        yield return new WaitForSeconds(secondsToDelay);
        act();
    }
}