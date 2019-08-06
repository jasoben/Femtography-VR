using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhotonColliderLauncher : MonoBehaviour
{
    public Particle particle;
    public UnityEvent backupReset;

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

    public void BackupReset() //in case collision event of photon detection fails
    {
        backupReset.Invoke();
    }
}
