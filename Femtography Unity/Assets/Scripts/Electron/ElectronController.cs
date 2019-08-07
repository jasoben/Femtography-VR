using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElectronController : MonoBehaviour
{

    public Light electronLight;
    private GameObject proton, electronStartPosition, masterControlObject;
    public GameObject Proton
    {
        get { return proton; }
        set { proton = value; }
    }
    public GameObject photon, photonCollider;
    private float distanceToProton;
    private bool photonLaunched, protonFound;
    public Particle particle, playerParticle;
    public UnityEvent photonBullet;

    // Start is called before the first frame update
    void Start()
    {
        photonLaunched = false;
        distanceToProton = 24;
    }

    // Update is called once per frame
    void Update()
    {
    
        if (proton != null && (Vector3.Distance(transform.position, proton.transform.position) < distanceToProton && !photonLaunched) || (transform.position.z > 1000f - distanceToProton && !photonLaunched))
        {
            LaunchPhoton();
            DeflectElectron();
            photonBullet.Invoke();
            playerParticle.normalSpeed = 0;
        }
                    
    }

    public void LaunchPhoton()
    {
        photonLaunched = true;
        GameObject photonBullet = Instantiate(photon, transform.position, Quaternion.Euler(0,90,0));
        photonBullet.GetComponent<PhotonController>().StartPhotonAnimation();
    }

    public void DeflectElectron()
    {
        float deflectionAngle = Random.Range(45f, 90f);
        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, deflectionAngle, transform.rotation.eulerAngles.z);
        transform.rotation = newRotation;
    }

    public void SetToFastLaunch()
    {
        particle.normalSpeed = 10;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "DestructionTrigger")
        {
            SetToFastLaunch();
            Destroy(gameObject);
        }
    }

}
