using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject player, console, computerGlass;
    public List<GameObject> teleportLocations;
    public List<GameObject> consoleLocations;
    private int currentLocation;
    private List<ParticleSystem> particleSystems;
    public float newConsoleSpin, distanceAmount;

    // Start is called before the first frame update
    void Start()
    {
        particleSystems = new List<ParticleSystem>();
        currentLocation = 0;
        for (int i =0; i < teleportLocations.Count; i++)
        {
            particleSystems.Add(teleportLocations[i].GetComponent<ParticleSystem>());
            particleSystems[i].Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GotoNextLocation();
        }

    }

    public void GotoNextLocation()
    {
        computerGlass.SetActive(false);
        currentLocation++;
        if (currentLocation > teleportLocations.Count - 1)
        {
            currentLocation = 0;
        }
        ActivateNextPortal();
        Invoke("ActivateThisPortal", 1.5f);
    }

    private void ActivateNextPortal()
    {
        particleSystems[currentLocation].Play();
    }

    private void ActivateThisPortal()
    {
        int previousLocation = currentLocation - 1;
        if (previousLocation == -1)
            previousLocation = teleportLocations.Count - 1;
        particleSystems[previousLocation].Play();
        Invoke("MoveToNextPortal", .5f);
    }

    private void MoveToNextPortal()
    {
        player.transform.position = teleportLocations[currentLocation].transform.position;
        console.transform.position = consoleLocations[currentLocation].transform.position;
        player.transform.rotation = teleportLocations[currentLocation].transform.rotation;
        console.transform.rotation = consoleLocations[currentLocation].transform.rotation;
        if (currentLocation == 0)
            computerGlass.SetActive(true);
        Invoke("ShutDownPortals", .5f);
    }

    private void ShutDownPortals()
    {
        foreach (ParticleSystem thisParticleSystem in particleSystems)
        {
            thisParticleSystem.Stop();
        }
    }




}
