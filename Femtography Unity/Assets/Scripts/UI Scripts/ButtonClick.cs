using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public GameObject startPosition, endPosition, buttonPosition, masterControlObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Vector3.Distance(buttonPosition.transform.position, endPosition.transform.position) < Vector3.Distance(startPosition.transform.position, endPosition.transform.position))
        {
            masterControlObject.GetComponent<MasterControlScript>().launchEvent.Invoke();

        }
    }
}
