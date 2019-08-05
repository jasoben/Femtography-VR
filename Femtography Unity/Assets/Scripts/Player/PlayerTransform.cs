using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransform : TransformObject
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        masterControlObject.GetComponent<MasterControlScript>().collideEvent.AddListener(StopMovingPlayer);
    }

    public void StopMovingPlayer()
    {
        moveThisObject = false;
        masterControlObject.GetComponent<MasterControlScript>().collideEvent.RemoveListener(StopMovingPlayer);
    }
}
