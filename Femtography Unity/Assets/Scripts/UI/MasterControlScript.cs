using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MasterControlScript : MonoBehaviour
{
    public GameObject proton, photon, electron, photonCollider, sensor, player;
    private GameObject newProton, newElectron;
    public Transform photonStartPosition, electronStartPosition, protonStartPosition;
    public Particle protonParticle, electronParticle, photonParticle, playerParticle, photonColliderParticle;
    public UnityEvent StartPlaying, StopPlaying, LaunchNewElectron;
    public float fallingTime, fallingDistance, fallingSlerp;
    private bool hasFallen;
    private BarrelState barrelState;

    // Start is called before the first frame update
    void Start()
    {
        barrelState = BarrelState.empty;
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
            CreateNewProtonAndElectron();
            StartCoroutine(SettleParticlesFastAndLaunch());
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            CreateNewProtonAndElectron();
            CreateNewObject(sensor, protonStartPosition);
        }
    }

    private IEnumerator SettleParticlesFastAndLaunch()
    {
        fallingSlerp = 1f;
        yield return new WaitUntil(() => hasFallen);
        newElectron.GetComponent<TransformObject>().KineticSpeed = 1;
        player.GetComponent<TransformObject>().KineticSpeed = 1;
    }

    private void PauseEverything()
    {
    }

    private void PlayEverything()
    {
    }

    public void CreateNewProtonAndElectron()
    {
        if (barrelState == BarrelState.empty)
        {
            barrelState = BarrelState.full;
            newProton = CreateNewObject(proton, protonStartPosition);
            newElectron = CreateNewObject(electron, electronStartPosition);
            StartCoroutine(ConnectProtonAndElectron(newProton, newElectron));
        }
    }

    public void BarrelIsEmpty()
    {
        barrelState = BarrelState.empty;
    }

    private IEnumerator ConnectProtonAndElectron(GameObject newProton, GameObject newElectron)
    {
        yield return new WaitWhile(() => newProton == null || newElectron == null);
        newElectron.GetComponent<ElectronController>().Proton = newProton;
    }

    public GameObject CreateNewObject(GameObject objectType, Transform objectTransform)
    {
        fallingSlerp = .03f;
        Vector3 startPosition = objectTransform.position + new Vector3(0, fallingDistance, 0);
        GameObject createdObject = Instantiate(objectType, startPosition, objectTransform.rotation);
        StartCoroutine(MoveObject(createdObject));
        return createdObject;
    }

    private IEnumerator MoveObject(GameObject createdObject)
    {
        hasFallen = false;
        Vector3 destination = createdObject.transform.position - new Vector3(0, fallingDistance, 0);
        while (true)
        {
            createdObject.transform.position = Vector3.Slerp(createdObject.transform.position, destination, fallingSlerp);

            if (Vector3.Distance(createdObject.transform.position, destination) < 1)
            {
                hasFallen = true;
                break;
            }

            float realFallingTime = fallingTime / fallingDistance;
            yield return new WaitForSeconds(realFallingTime);
        }
    }
}

public enum BarrelState
{
    empty,
    full
}
