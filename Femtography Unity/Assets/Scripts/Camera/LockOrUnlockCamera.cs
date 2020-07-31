using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.Utility;

public class LockOrUnlockCamera : MonoBehaviour
{
    SimpleMouseRotator simpleMouseRotator;
    // Start is called before the first frame update
    void Start()
    {
        simpleMouseRotator = GetComponent<SimpleMouseRotator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LockOrUnlock(bool isLocked)
    {
        if (isLocked)
            LockCamera();
        else
            UnLockCamera();
    }

    void LockCamera()
    {
        simpleMouseRotator.enabled = false;
        transform.rotation = Quaternion.identity;
    }
    void UnLockCamera()
    {
        simpleMouseRotator.enabled = true;
    }
}
