using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonController : MonoBehaviour
{

    public Particle particle;
    public GameObject photon;
    public GlobalBool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, new Vector3(0,0,1000)) > 200)
        {
            Destroy(gameObject);
        }
    }


    public void DestroyPhoton()
    {
        Destroy(gameObject);
    }


}
