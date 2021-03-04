using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Teleporter : MonoBehaviour
{
    public GameObject playerVehicle;
    public List<GameObject> teleportLocations;
    private int currentLocation, previousLocation;
    public Material teleportMaterial;
    public UnityEvent teleporterActivated, teleportFinished;
    public MenuManagerObject teleporter; 
    
    // Start is called before the first frame update
    void Start()
    {
        currentLocation = 0;
        teleportMaterial.SetFloat("Transparency_", 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && teleporter.isActive)
        {
            teleporterActivated.Invoke();
        }
    }

    public void GotoNextLocation()
    {
        GetComponent<AudioSource>().PlayDelayed(.8f);
        previousLocation = currentLocation;
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
        teleportLocations[previousLocation].SetActive(true);
        teleportLocations[currentLocation].SetActive(true);
        StartCoroutine(RunTeleporterAnimation());
    }

    IEnumerator RunTeleporterAnimation()
    {
        float newTransparency = 5;
        while (true)
        {
            newTransparency -= .03f;
            teleportMaterial.SetFloat("Transparency_", newTransparency);
            if (newTransparency < 1f)
            {
                yield break;
            } else
                yield return new WaitForEndOfFrame();
        }
    }

    private void ActivateThisPortal()
    {
        playerVehicle.transform.position = teleportLocations[currentLocation].transform.position;
        playerVehicle.transform.rotation = teleportLocations[currentLocation].transform.rotation;

        // The following is so the vehicle returns to the new position after following particles along their path
        playerVehicle.GetComponent<FollowParticle>().playerFinalPosition.transform.position = playerVehicle.transform.position;
        playerVehicle.GetComponent<FollowParticle>().playerFinalPosition.transform.rotation = playerVehicle.transform.rotation;

        teleportLocations[currentLocation].SetActive(false);
        teleportLocations[previousLocation].SetActive(false);

        teleportFinished.Invoke();
    }





}
