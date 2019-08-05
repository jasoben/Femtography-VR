using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronLauncher : TransformObject
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start(); 
        masterControlObject.GetComponent<MasterControlScript>().launchEvent.AddListener(StartMoving);
    }

}
