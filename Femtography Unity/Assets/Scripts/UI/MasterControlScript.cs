using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MasterControlScript : MonoBehaviour
{
    public GameObject proton, photon, electron, sensor, player;
    public List<GameObject> quarks;
    private GameObject newProton, newElectron;
    public Transform photonStartPosition, electronStartPosition, protonStartPosition;
    public Particle protonParticle, electronParticle, photonParticle, playerParticle;
    public UnityEvent StartPlaying, StopPlaying, basicInstructions, teleporterUnlocked;
    public float fallingTime, fallingDistance, fallingSlerp;
    private bool hasFallen;
    private BarrelState barrelState;
    public GlobalBool firstPlayThrough;
    public FloatReference q2slider;
    private float particlesCreated;

    // Start is called before the first frame update
    void Start()
    {
        barrelState = BarrelState.empty;
        electronParticle.normalSpeed = 1;
        playerParticle.normalSpeed = 1;
        firstPlayThrough.boolValue = true;
        particlesCreated = 0;
        q2slider.variable.value = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartPlaying.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            StopPlaying.Invoke();
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void RevealTheQuarks()
    {
        foreach(GameObject quark in quarks)
        {
            quark.SetActive(true);
        }
    }

    //methods for UI events
    public void Initiate()
    {
        CreateNewProtonAndElectron();
        CreateNewObject(sensor, protonStartPosition);
    }

    public void Launch()
    {
        CreateNewProtonAndElectron();
        StartCoroutine(SettleParticlesFastAndLaunch());
    }

    public void ResetEverything()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayEverything()
    {
        StartPlaying.Invoke();
    }
    
    public void PauseEverything()
    {
        StopPlaying.Invoke();
    }

    private IEnumerator SettleParticlesFastAndLaunch()
    {
        fallingSlerp = 1f;
        yield return new WaitUntil(() => hasFallen);
        newElectron.GetComponent<TransformObject>().KineticSpeed = 1;
        player.GetComponent<TransformObject>().KineticSpeed = 1;
    }

    public void CreateNewProtonAndElectron()
    {
        if (barrelState == BarrelState.empty)
        {
            particlesCreated++;
            if (particlesCreated > 1)
            {
                firstPlayThrough.boolValue = false;
                if (particlesCreated < 3)
                    basicInstructions.Invoke();
                else if (particlesCreated > 4 && particlesCreated < 6)
                    teleporterUnlocked.Invoke();

            }

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


//-39, 0, 83

//55, 0, 82

//57, 0, 104

//-66, 0, 107

//Console at teleport - 62,0,90 
//Player is at -66,0,87

//Console - 67,0,90

