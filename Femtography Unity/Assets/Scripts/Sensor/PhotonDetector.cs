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
        Debug.Log("particle detected");
        GameObject particleDetected = Instantiate(photonDetectedLight, other.gameObject.transform.position, Quaternion.identity);
        particleDetected.transform.rotation = other.transform.rotation;

        if (other.tag == "photons")
        {
            photonDetected.Invoke();
            particleDetected.GetComponent<Renderer>().material.color = Color.white;
            particleDetected.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
        }
        if (other.tag == "Proton")
        {
            particleDetected.GetComponent<Renderer>().material.color = Color.red;
            particleDetected.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);
        }   
        if (other.tag == "electron")
        {
            particleDetected.GetComponent<Renderer>().material.color = Color.yellow;
            particleDetected.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.yellow);
        }

    }

    public void TurnOnCollider()
    {
        GetComponent<MeshCollider>().enabled = true;
    }
    
}
