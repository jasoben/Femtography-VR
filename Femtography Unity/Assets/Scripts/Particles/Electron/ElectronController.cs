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
    public FloatReference distanceToProtonReference;
    private bool photonLaunched, protonFound;
    public Particle particle, playerParticle;
    public UnityEvent photonBullet, pauseEverything, revealQuarks;
    public GlobalBool firstPlayThroughGlobal;
    public VectorConstant photonLaunchVector, startPosition;
    private float distanceToProton;

    // Start is called before the first frame update
    void Start()
    {
        photonLaunched = false;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToProton = distanceToProtonReference.Value * 60;
        if (proton != null && (Vector3.Distance(transform.position, proton.transform.position) < distanceToProton && !photonLaunched) || (transform.position.z > 1000f - distanceToProton && !photonLaunched))
        {
            LaunchPhoton();
            DeflectElectron();
            photonBullet.Invoke();
            if (firstPlayThroughGlobal.boolValue)
                Invoke("PauseTheSystemForFirstPlayThrough", .2f);
            playerParticle.normalSpeed = 0;
        }

    }

    public void PauseTheSystemForFirstPlayThrough()
    {
        pauseEverything.Invoke();
        revealQuarks.Invoke();
    }

    public void LaunchPhoton()
    {
        photonLaunched = true;
        GameObject photonBullet = Instantiate(photon, transform.position + photonLaunchVector.vectorValue, Quaternion.Euler(0, 0, 90));
        photonBullet.GetComponent<TransformObject>().KineticSpeed = 1;
    }

    public void DeflectElectron()
    {
        float deflectionAngle = 45f;
        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, deflectionAngle, transform.rotation.eulerAngles.z);
        transform.rotation = newRotation;
    }

    public void SetToFastLaunch()
    {
        startPosition.vectorValue = new Vector3(0, 0, 750);
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
