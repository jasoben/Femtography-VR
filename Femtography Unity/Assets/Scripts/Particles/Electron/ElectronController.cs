using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ElectronController : MonoBehaviour
{

    public Light electronLight;
    private GameObject proton, electronStartPosition, masterControlObject;
    public GameObject Proton
    {
        get { return proton; }
        set { proton = value; }
    }
    public GameObject photon, photonCollider, labelText;
    public Material electronMaterial;

    private bool photonLaunched, protonFound;
    public Particle particle, playerParticle;
    public UnityEvent photonBullet, pauseEverything, revealQuarks, loadNewElectron;
    public GlobalBool firstPlayThroughGlobal, vehicleFollowElectron, vehicleInPosition;
    public VectorConstant photonLaunchVector, startPosition;
    public float distanceFromProtonToLaunchPhoton;

    // Start is called before the first frame update
    void Start()
    {
        photonLaunched = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (proton != null && 
            (Vector3.Distance(transform.position, proton.transform.position) < distanceFromProtonToLaunchPhoton && 
            !photonLaunched) && 
            vehicleFollowElectron.boolValue == true)
        {
            LaunchPhoton();
            DeflectElectron();
            photonBullet.Invoke();
            if (firstPlayThroughGlobal.boolValue)
                Invoke("PauseTheSystemForFirstPlayThrough", .2f);
        }
        else if (proton != null && 
            Vector3.Distance(transform.position, proton.transform.position) < (distanceFromProtonToLaunchPhoton + 15) &&
            vehicleFollowElectron.boolValue == false) // If we aren't following the electron, we don't want the whole cascade 
            // of events to happen, so we check a bit further out from the proton and reset the system before all those events
            // can trigger.
        {
            loadNewElectron.Invoke();
            Destroy(gameObject);
        }


        // Change the visual "spin" speed of the electron based on playback speed

        //electronMaterial.SetFloat("Speed_", particle.speed);

        DebugUI.ShowText("particle speed", playerParticle.playbackSpeed.Value.ToString());

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
    }

    public void HideLabel()
    {
        labelText.SetActive(false); 
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
        if (other.tag == "VehiclePositionTrigger")
        {
            if (vehicleFollowElectron.boolValue && vehicleInPosition.boolValue)
            {
                SetToFastLaunch();
            }            
        }
        if (other.tag == "DestructionTrigger")
        {
            Destroy(gameObject);
        }
    }

}
