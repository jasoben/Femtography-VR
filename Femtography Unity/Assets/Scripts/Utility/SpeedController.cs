using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Codice.CM.SEIDInfo;

public class SpeedController : MonoBehaviour
{
    List<MonoBehaviour> speedControllers = new List<MonoBehaviour>();

    public FloatReference playbackSpeed;

    // Start is called before the first frame update
    void Start()
    {
        var allObjects = FindObjectsOfType<MonoBehaviour>();
        foreach (var obj in allObjects)
        {
            if (obj.GetType().GetInterface("ISpeedController") != null)
            {
                speedControllers.Add(obj);
            }
        }

        Debug.Log(speedControllers.Count);

        foreach (var obj in speedControllers)
        {
            ISpeedController speedController = obj as ISpeedController;
            speedController.SetSpeedReference(playbackSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
