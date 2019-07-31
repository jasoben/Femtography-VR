using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtonTransform : MonoBehaviour
{
    public GameObject electron;
    public float distanceToElectron;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(gameObject.transform.position, electron.transform.position) > distanceToElectron)
        {
            gameObject.transform.Translate(1, 0, 0);
        }

        
    }
}
