using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhotonController : MonoBehaviour
{

    public Particle particle;
    public GameObject photon;
    public GlobalBool isPlaying;

    public UnityEvent photonCollisionWithDetector;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TransformObject>().StartMoving();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "detector")
        {
            photonCollisionWithDetector.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void DestroyPhoton()
    {
        Destroy(gameObject);
    }


}
