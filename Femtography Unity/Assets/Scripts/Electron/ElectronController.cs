using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronController : MonoBehaviour
{

    public Light electronLight;
    private GameObject proton, electronStartPosition, masterControlObject;
    public GameObject MyPhoton;
    private float lightIntensity, lightIntensityDelta, distanceToProton, lightIntensityDeltaCoefficient;

    // Start is called before the first frame update
    void Start()
    {
        lightIntensity = .5f;
        lightIntensityDelta = 1;
        distanceToProton = 14;
        proton = GameObject.Find("Proton");
        electronStartPosition = GameObject.Find("ElectronStartPosition");
        masterControlObject = GameObject.Find("MasterControlObject");
        transform.position = electronStartPosition.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    
        if (lightIntensity < 0.4f)
        {
            lightIntensityDelta = 1;
        }
        else if (lightIntensity > .6f)
        {
            lightIntensityDelta = -1;
        }

        lightIntensity += lightIntensityDelta * lightIntensityDeltaCoefficient;
        electronLight.intensity = lightIntensity;

        if (Vector3.Distance(transform.position, proton.transform.position) < distanceToProton)
        {
            masterControlObject.GetComponent<MasterControlScript>().collideEvent.Invoke();
            MyPhoton.GetComponent<PhotonController>().StartPhotonAnimation();
            Destroy(gameObject);
        }
            
            
    }

    public void LaunchElectron()
    {
        GetComponent<TransformObject>().moveThisObject = true;
    }

             
}
