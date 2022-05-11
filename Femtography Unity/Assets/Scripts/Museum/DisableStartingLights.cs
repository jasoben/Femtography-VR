using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class DisableStartingLights : MonoBehaviour
{
    public GameEvent startingLightsOff, totalDarkness;
    GameObject[] startingLights;
    // Start is called before the first frame update
    void Start()
    {
        startingLights = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            startingLights[i] = transform.GetChild(i).gameObject;
        }
    }

    public async void PlaySoundDisableLights()
    {
        for (int i = 0; i < startingLights.Length; i++)
        {
            startingLights[i].GetComponent<Renderer>().enabled = false;
            foreach (Light light in startingLights[i].GetComponentsInChildren<Light>())
            {
                light.enabled = false;
            }
            startingLights[i].GetComponent<AudioSource>().Play();
            await Task.Delay(1000);
            startingLights[i].SetActive(false);
        }
        totalDarkness.Raise();
        await Task.Delay(2000);
        startingLightsOff.Raise();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
