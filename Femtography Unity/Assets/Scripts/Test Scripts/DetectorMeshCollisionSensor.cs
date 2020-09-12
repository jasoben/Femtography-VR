using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorMeshCollisionSensor : MonoBehaviour
{
    // This class shows aesthetic signals that the particles have been detected.
    // Any events or methods for detection occur on the particles themselves, not on the detector.

    public Material undetected, detected;
    AudioSource detectionSound;
    public FloatReference playbackSpeed;
    // Start is called before the first frame update
    void Start()
    {
        detectionSound = gameObject.AddComponent<AudioSource>();
        detectionSound.clip = transform.parent.parent.GetComponent<AudioSource>().clip;
    }

    private void OnTriggerEnter(Collider other)
    {
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
        float elapsedTime = 0;
        while (true)
        {
            yield return new WaitForSeconds(.01f);
            elapsedTime += .01f * playbackSpeed.Value;
            if (elapsedTime > 1.5f)
            {
                GetComponent<Renderer>().material = undetected;
                yield break;
            }
        }
    }
}
