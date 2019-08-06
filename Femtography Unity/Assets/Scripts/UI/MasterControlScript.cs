using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MasterControlScript : MonoBehaviour
{
    public GameObject proton, photon, electron, photonCollider, sensor; 
    public Transform photonStartPosition, electronStartPosition, protonStartPosition;
    public Particle protonParticle, electronParticle, photonParticle, playerParticle, photonColliderParticle;
    public UnityEvent StartPlaying, StopPlaying, LaunchNewElectron;
    public float fallingTime, fallingDistance;

    // Start is called before the first frame update
    void Start()
    {
        electronParticle.normalSpeed = 1;
        playerParticle.normalSpeed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            electronParticle.speed = 1;
            playerParticle.speed = 1;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            CreateNewObject(proton, protonStartPosition);
            CreateNewObject(sensor, protonStartPosition);
            CreateNewObject(electron, electronStartPosition);
        }
    }

    private void PauseEverything()
    {
    }

    private void PlayEverything()
    {
    }

    public void CreateNewProtonAndElectron()
    {
        CreateNewObject(proton, protonStartPosition);
        CreateNewObject(electron, electronStartPosition);
        electronParticle.normalSpeed = 5;
    }

    public void LaunchElectron()
    {
        Vector3 theElectronStartPosition = electronStartPosition.transform.position;
        GameObject newElectron = Instantiate(electron, theElectronStartPosition, Quaternion.identity);
    }

    public void CreateNewObject(GameObject newObject, Transform objectTransform)
    {
        Vector3 startPosition = objectTransform.position + new Vector3(0, fallingDistance, 0);
        GameObject createdObject = Instantiate(newObject, startPosition, objectTransform.rotation);
        StartCoroutine(MoveObject(createdObject));
    }

    private IEnumerator MoveObject(GameObject createdObject)
    {
        Vector3 destination = createdObject.transform.position - new Vector3(0, fallingDistance, 0);
        Debug.Log(destination.ToString());
        while (true)
        {
            createdObject.transform.position = Vector3.Slerp(createdObject.transform.position, destination, .03f);

            if (Vector3.Distance(createdObject.transform.position, destination) < 1)
            {
                break;
            }

            float realFallingTime = fallingTime / fallingDistance;
            yield return new WaitForSeconds(realFallingTime);
        }
    }


}
