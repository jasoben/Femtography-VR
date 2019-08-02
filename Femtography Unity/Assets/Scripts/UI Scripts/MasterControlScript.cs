using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MasterControlScript : MonoBehaviour
{
    public UnityEvent launchEvent, collideEvent, pauseEvent;

    // Start is called before the first frame update
    void Start()
    {
        if (launchEvent == null)
            launchEvent = new UnityEvent();
        if (collideEvent == null)
            collideEvent = new UnityEvent();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && launchEvent != null)
        {
            launchEvent.Invoke();
        }

        else if (Input.GetKeyDown(KeyCode.P))
        {
            pauseEvent.Invoke();
        }
    }

    public void Collided()
    {
        collideEvent.Invoke();
    }
}
