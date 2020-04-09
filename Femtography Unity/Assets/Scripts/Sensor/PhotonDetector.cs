using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhotonDetector : MonoBehaviour
{
    public GameObject photonDetectedLight;
    public UnityEvent photonDetected, pauseEverything;
    public GlobalBool firstPlay;

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
        GameObject particleDetected = Instantiate(photonDetectedLight, other.gameObject.transform.position, Quaternion.identity);
        Vector3 towardsCenter = gameObject.transform.position - GetComponent<MeshCollider>().ClosestPoint(other.transform.position);
        Vector3 upDirection = new Vector3(0, 0, 1100) - GetComponent<MeshCollider>().ClosestPoint(other.transform.position);
        Quaternion thisRotation = Quaternion.LookRotation(towardsCenter, upDirection);
        particleDetected.transform.rotation = thisRotation;
        

        if  (other.tag == "photonCollider")
        {
            photonDetected.Invoke();
            //if (firstPlay.boolValue)
            //    pauseEverything.Invoke();
            particleDetected.GetComponent<Renderer>().material.color = Color.white;
            particleDetected.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
        }
        else if (other.tag == "Proton")
        {
            particleDetected.transform.localScale = new Vector3(20, 20, 1);
            particleDetected.transform.Translate(new Vector3(0,0,10), Space.Self);
            particleDetected.GetComponent<Renderer>().material.color = Color.red;
            particleDetected.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);
            other.gameObject.GetComponent<Renderer>().enabled = false;
            
            Renderer[] theseRenderers = other.gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer thisRenderer in theseRenderers)
            {
                thisRenderer.enabled = false;
            }
        }   
        else if (other.tag == "electron")
        {
            particleDetected.GetComponent<Renderer>().material.color = Color.yellow;
            particleDetected.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.yellow);
            
            Renderer[] theseRenderers = other.gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer thisRenderer in theseRenderers)
            {
                thisRenderer.enabled = false;
            }
        }

    }

    public void TurnOnCollider()
    {
        GetComponent<MeshCollider>().enabled = true;
    }
    
    public void TurnOffCollider()
    {
        GetComponent<MeshCollider>().enabled = false;
    }
}
