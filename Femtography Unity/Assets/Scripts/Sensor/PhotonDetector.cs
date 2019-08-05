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
        GameObject thisPhotonDetected = Instantiate(photonDetectedLight, collision.gameObject.transform.position, Quaternion.identity);
        thisPhotonDetected.transform.rotation = collision.transform.rotation;
        Destroy(collision.gameObject);
    }

    public void TurnOnCollider()
    {
        GetComponent<MeshCollider>().enabled = true;
    }
    
}
