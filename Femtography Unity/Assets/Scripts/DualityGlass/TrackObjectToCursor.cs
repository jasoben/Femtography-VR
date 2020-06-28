using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackObjectToCursor : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPositionBasedOnMousePosition = new Vector3();
        Vector3 mousePositionPlusZ = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
        newPositionBasedOnMousePosition = Camera.main.ScreenToWorldPoint(mousePositionPlusZ);

        transform.position = newPositionBasedOnMousePosition;

        transform.rotation = Quaternion.LookRotation(Camera.main.transform.up, -Camera.main.transform.forward);
    }
}
