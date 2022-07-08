using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtensionClass
{
    public static Enum GetRandomEnumValue(this Type t)
    {
        return Enum.GetValues(t)          // get values from Type provided
            .OfType<Enum>()               // casts to Enum
            .OrderBy(e => Guid.NewGuid()) // mess with order of results
            .FirstOrDefault();            // take first item in result
    }

    public static WaitForFixedUpdate WaitForFixedUpdate =
        new WaitForFixedUpdate();

    public static WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();

    static Dictionary<float, WaitForSeconds> _cachedWaits
        = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWaitForSeconds(float seconds)
    {
        if (!_cachedWaits.ContainsKey(seconds))
            _cachedWaits.Add(seconds, new WaitForSeconds(seconds));
        return _cachedWaits[seconds];
    }

    static Dictionary<float, WaitForSecondsRealtime> _cachedWaitsRealtime
        = new Dictionary<float, WaitForSecondsRealtime>();

    public static WaitForSecondsRealtime GetWaitForSecondsRealtime(
        float seconds)
    {
        if (!_cachedWaitsRealtime.ContainsKey(seconds))
            _cachedWaitsRealtime.Add(seconds, new WaitForSecondsRealtime(seconds));
        return _cachedWaitsRealtime[seconds];
    }

    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        return source.PickRandom(1).Single();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }

    public static string TrimLower(this string str)
    {
        return str.ToLower().Trim();
    }
}