using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonColliderLauncher : MonoBehaviour
{
    public Particle particle;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchPhotonCollider()
    {
        GetComponent<Rigidbody>().AddRelativeForce(0, 0, 26.6f, ForceMode.VelocityChange);
        Destroy(gameObject, 7f);
    }
}
