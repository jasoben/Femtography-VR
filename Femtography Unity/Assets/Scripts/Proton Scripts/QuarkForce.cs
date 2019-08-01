using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuarkForce : MonoBehaviour
{
    public Vector3 forceOnQuark;

    // Start is called before the first frame update
    void Start()
    {

        GetComponent<Rigidbody>().AddForce(forceOnQuark);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
