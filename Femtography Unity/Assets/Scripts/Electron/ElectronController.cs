using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElectronController : MonoBehaviour
{

    public Light electronLight;
    private GameObject proton, electronStartPosition, masterControlObject;
    public GameObject photon, photonCollider;
    private float distanceToProton;
    private bool photonLaunched, protonFound;
    public Particle particle, playerParticle;
    public UnityEvent photonBullet;

    // Start is called before the first frame update
    void Start()
    {
        particle.speed = 0; 
        photonLaunched = false;
        protonFound = false;
        distanceToProton = 24;
    }
    public void FindProton()
    {
        proton = GameObject.FindWithTag("Proton");
        protonFound = true;
    }

    // Update is called once per frame
    void Update()
    {
    
        if (protonFound && (Vector3.Distance(transform.position, proton.transform.position) < distanceToProton && !photonLaunched) || (transform.position.z > 1000f - distanceToProton && !photonLaunched))
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
        GameObject photonBulletCollider = Instantiate(photonCollider, transform.position, transform.rotation);
        photonBullet.GetComponent<PhotonController>().StartPhotonAnimation();
        photonBulletCollider.GetComponent<PhotonColliderLauncher>().LaunchPhotonCollider();
    }

    public void DeflectElectron()
    {
        float deflectionAngle = Random.Range(45f, 90f);
        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, deflectionAngle, transform.rotation.eulerAngles.z);
        transform.rotation = newRotation;
        Destroy(gameObject, 4f);
    }
             
}
