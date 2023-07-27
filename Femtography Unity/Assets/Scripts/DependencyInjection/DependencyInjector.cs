
using System;
using UnityEngine;

public static class DependencyInjector  
{
    public static event EventHandler<MonoBehaviour> InjectorEvent;

    public static void InvokeInjectorEvent(System.Object obj, MonoBehaviour monoBehaviour)
    {
        InjectorEvent?.Invoke(obj, monoBehaviour);
    }

}
