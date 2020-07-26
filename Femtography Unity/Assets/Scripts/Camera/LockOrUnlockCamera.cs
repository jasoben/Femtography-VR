using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.Utility;

public class LockOrUnlockCamera : MonoBehaviour
{
    bool isLocked;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LockOrUnlock()
    {
        if (isLocked)
            UnLockCamera();
        else
            LockCamera();
        isLocked = !isLocked;
    }

    void LockCamera()
    {
        GetComponent<SimpleMouseRotator>().enabled = false;
        transform.rotation = Quaternion.identity;
        GetComponent<PhysicsRaycaster>().enabled = false;
    }
    void UnLockCamera()
    {
        GetComponent<SimpleMouseRotator>().enabled = true;
        GetComponent<PhysicsRaycaster>().enabled = true;
    }
}
