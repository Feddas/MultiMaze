using System;
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
    public static T Get<T>(PlayerPrefEnum PrefProperty, T defaultValue = default(T)) where T : struct, IConvertible
    {
        string key = PrefProperty.ToString();
        if (PlayerPrefs.HasKey(key) == false)
            return defaultValue;

        var type = typeof(T);
        if (type.IsEnum && Enum.GetUnderlyingType(type) == typeof(int))
        {
            return EnumConverter<T>.Convert(PlayerPrefs.GetInt(key));
            //return (T)(object)PlayerPrefs.GetInt(key);
        }
        throw new Exception("PlayerPref type " + type + " handling isn't implemented in Get<T>()");
    }
}

static class EnumConverter<TEnum> where TEnum : struct, IConvertible
{
    public static readonly Func<long, TEnum> Convert = GenerateConverter();

    static Func<long, TEnum> GenerateConverter()
    {
        var parameter = Expression.Parameter(typeof(long), "param");
        var dynamicMethod = Expression.Lambda<Func<long, TEnum>>(
            Expression.Convert(parameter, typeof(TEnum)),
            parameter);
        return dynamicMethod.Compile();
    }
}
