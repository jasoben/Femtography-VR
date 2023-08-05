using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Codice.CM.SEIDInfo;
using System.Runtime.CompilerServices;

public class GlobalVariableManager : MonoBehaviour
{
    public static GlobalVariableManager Instance;
    public FloatReference playbackSpeed;
    public float PlayerHeight { get; private set; } = 1.75f;

    public void InjectDependency(object sender, MonoBehaviour monoBehaviour)
    {
        if (monoBehaviour.GetType().GetInterface(nameof(ISpeedController)) != null)
        {
            ISpeedController speedController = monoBehaviour as ISpeedController;
            speedController.SetSpeedReference(playbackSpeed);
        }
        else
        {
            throw new System.Exception("Interface not found. Did you forget to add it to the conditional list?"); 
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        DependencyInjector.InjectorEvent += InjectDependency;
        Instance = this;
    }
    private void OnDestroy()
    {
        DependencyInjector.InjectorEvent -= InjectDependency;
    }

}
