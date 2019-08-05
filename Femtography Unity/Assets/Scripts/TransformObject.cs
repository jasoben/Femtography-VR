using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformObject : MonoBehaviour
{
    public bool moveThisObject = true;
    protected GameObject masterControlObject;
    
    // Start is called before the first frame update
    protected void Start()
    {
        masterControlObject = GameObject.Find("MasterControlObject");
        masterControlObject.GetComponent<MasterControlScript>().pauseEvent.AddListener(StopMoving);
        masterControlObject.GetComponent<MasterControlScript>().playEvent.AddListener(StartMoving);
        
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayBackControl.isPlaying && moveThisObject)
        {
            transform.Translate(-1, 0, 0, Space.World);
        } else
        {
        }

    }

    public void StopMoving()
    {
        moveThisObject = false;
    }
    public void StartMoving()
    {
        moveThisObject = true;
    }
}
