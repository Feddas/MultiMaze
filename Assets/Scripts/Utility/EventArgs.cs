using UnityEngine;
using System.Collections;
using System;

public class EventArgs<T> : EventArgs
{
    public T Data { get; set; }

    public EventArgs(T input)
    {
        Data = input;
    }
}

public static class EventHandlerExtensions
{
    public static EventArgs<T> Arg<T>(
        this EventHandler<EventArgs<T>> _,
        T argument)
    {
        return new EventArgs<T>(argument);
    }
}