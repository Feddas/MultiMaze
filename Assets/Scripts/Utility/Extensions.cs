﻿using System;
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

public static class FloatExtensions
{
    public static bool IsZero(this float original)
    {
        return Mathf.Approximately(original, 0f);
        //return Mathf.Abs(original) <= float.Epsilon;
    }
}

public static class Vector3Extensions
{
    public static Vector3 SetY(this Vector3 original, float yValue)
    {
        return new Vector3(original.x, yValue, original.z);
    }
}