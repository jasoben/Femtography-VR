using UnityEngine;
public interface IVariableInjector
{
    void RequestInjection(System.Object obj, MonoBehaviour monoBehaviour);
}