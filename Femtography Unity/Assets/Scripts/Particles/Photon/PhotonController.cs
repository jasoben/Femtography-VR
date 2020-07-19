using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhotonController : MonoBehaviour
{

    public Particle particle;
    public GameObject photon;
    public GlobalBool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TransformObject>().StartMoving();
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
