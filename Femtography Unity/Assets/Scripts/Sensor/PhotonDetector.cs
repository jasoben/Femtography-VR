using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonDetector : MonoBehaviour
{
    public GameObject photonDetectedLight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(photonDetectedLight, collision.gameObject.transform.position, Quaternion.identity);
        Destroy(collision.gameObject);
    }
    
}
