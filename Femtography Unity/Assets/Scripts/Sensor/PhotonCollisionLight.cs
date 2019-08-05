using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonCollisionLight : MonoBehaviour
{
    private float intensity, range;
    private bool growing;
    private Light thisLight;

    // Start is called before the first frame update
    void Start()
    {

        growing = true;
        thisLight = GetComponent<Light>();
        intensity = range = 0;
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (growing && intensity < 5)
        {
            intensity += .1f;
            range += .1f;
        }
        else if (growing && intensity >= 5)
        {
            growing = false;
        }
        if (!growing)
        {
            intensity -= .2f;
            range -= .2f;
        }
        else if (!growing && intensity < .3f)
        {
            Destroy(gameObject);
        }
        thisLight.intensity = intensity;
        thisLight.range = range;
    }
}
