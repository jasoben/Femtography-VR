using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorMeshCollisionSensor : MonoBehaviour
{

    public Material undetected, detected;
    AudioSource detectionSound;
    // Start is called before the first frame update
    void Start()
    {
        detectionSound = gameObject.AddComponent<AudioSource>();
        detectionSound.clip = transform.parent.parent.GetComponent<AudioSource>().clip;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        GetComponent<Renderer>().material = detected;
        detectionSound.Play();
        StartCoroutine("ResetColor");
    }

    private void OnCollisionEnter(Collision collision)
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(1.5f);
        GetComponent<Renderer>().material = undetected;
        yield break;
    }
}
