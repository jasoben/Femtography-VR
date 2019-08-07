using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhotonColliderLauncher : MonoBehaviour
{
    public UnityEvent backupReset;

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

    public void LaunchPhotonCollider()
    {
        GetComponent<Rigidbody>().AddRelativeForce(0, 0, 26.6f, ForceMode.VelocityChange);
    }
    public void PausePhotonCollider()
    {
        GetComponent<Rigidbody>().AddRelativeForce(0, 0, -26.6f, ForceMode.VelocityChange);
    }

    public void BackupReset() //in case collision event of photon detection fails
    {
        backupReset.Invoke();
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "DestructionTrigger")
        {
            Destroy(gameObject);
        }
    }
}
