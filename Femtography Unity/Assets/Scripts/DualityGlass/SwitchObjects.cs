using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObjects : MonoBehaviour
{
    public List<GameObject> firstSetOfObjects, secondSetOfObjects;
    bool whichActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.T))
        {
            whichActive = !whichActive;
            foreach(GameObject thisGameObject in firstSetOfObjects)
            {
                thisGameObject.SetActive(whichActive);
            }
            foreach(GameObject thisGameObject in secondSetOfObjects)
            {
                thisGameObject.SetActive(!whichActive);
            }
        }
        
    }
}
