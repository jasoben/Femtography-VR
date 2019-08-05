using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonColliderLauncher : MonoBehaviour
{
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
        GetComponent<Rigidbody>().AddRelativeForce(-1400, 0, 0);
        Destroy(gameObject, 7f);
    }
}
