using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhotonDetector : MonoBehaviour
{
    public GameObject photonDetectedLight;
    public UnityEvent photonDetected; 

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
        photonDetected.Invoke();
        Destroy(collision.gameObject);
    }

    public void TurnOnCollider()
    {
        GetComponent<MeshCollider>().enabled = true;
    }
    
}
