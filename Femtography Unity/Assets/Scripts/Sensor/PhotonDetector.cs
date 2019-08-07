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

    void OnTriggerEnter(Collider other)
    {
        GameObject thisPhotonDetected = Instantiate(photonDetectedLight, other.gameObject.transform.position, Quaternion.identity);
        thisPhotonDetected.transform.rotation = other.transform.rotation;
        photonDetected.Invoke();
        Destroy(other.gameObject);
    }

    public void TurnOnCollider()
    {
        GetComponent<MeshCollider>().enabled = true;
    }
    
}
