using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public enum PlayerPrefEnum
{
    /// <summary> an int representing ControlModeEnum.cs </summary>
    ControlMode,
}

/// <summary>
/// based off of http://codereview.stackexchange.com/questions/90212/helper-class-to-interact-with-unitys-playerprefs-system
/// </summary>
public static class PlayerPref
{
    /// <summary>
    /// Used when PlayerPrefs are not available, AKA in WebGL
    /// </summary>
    private static Dictionary<string, object> PrefsFallback
    {
        get
        {
            if (prefsFallback == null)
            {
                prefsFallback = new Dictionary<string, object>();
            }
            return prefsFallback;
        }
        set { prefsFallback = value; }
    }
    private static Dictionary<string, object> prefsFallback;

    private static void AddFallback<T>(string key, T value) where T : struct, IConvertible
    {
        PrefsFallback.Add(key, value);
    }

    private static T GetFallback<T>(string key) where T : struct, IConvertible
    {
        return (T)PrefsFallback[key];
    }

    public static T Get<T>(PlayerPrefEnum PrefProperty, T defaultValue = default(T)) where T : struct, IConvertible
    {
        string key = PrefProperty.ToString();

        if (PlayerPrefs.HasKey(key) == false)
            return PrefsFallback.ContainsKey(key) ? GetFallback<T>(key) : defaultValue;

        var type = typeof(T);
        if (type.IsEnum && Enum.GetUnderlyingType(type) == typeof(int))
        {
            return (T)Enum.ToObject(typeof(T), PlayerPrefs.GetInt(key)); //http://stackoverflow.com/questions/29482/cast-int-to-enum-in-c-sharp
            //return EnumConverter<T>.Convert(PlayerPrefs.GetInt(key));
            //return (T)(object)PlayerPrefs.GetInt(key);
        }
        throw new Exception("PlayerPref type " + type + " handling isn't implemented in Get<T>()");
    }

    public static void SetInt(string key, int value)
    {
        try
        {
            PlayerPrefs.SetInt(key, value);
        }
        catch (Exception)
        {
            AddFallback<int>(key, value);
            throw;
        }
    }
}


/// <summary>
/// This doesn't work with WebGL because the dynamic code is Unsupported internal call for IL2CPP:DynamicMethod::create_dynamic_method - System.Reflection.Emit is not supported.
/// </summary>
/// <typeparam name="TEnum"></typeparam>
//static class EnumConverter<TEnum> where TEnum : struct, IConvertible
//{
//    public static readonly Func<long, TEnum> Convert = GenerateConverter();

//    static Func<long, TEnum> GenerateConverter()
//    {
//        var parameter = Expression.Parameter(typeof(long), "param");
//        var dynamicMethod = Expression.Lambda<Func<long, TEnum>>(
//            Expression.Convert(parameter, typeof(TEnum)),
//            parameter);
//        return dynamicMethod.Compile();
//    }
//}
