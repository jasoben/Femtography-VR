using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MasterControlScript : MonoBehaviour
{
    [Header("Game Objects")]
    [Space(10)]
    public GameObject proton;
    public GameObject photon, electron, detector, player, initializePointer;
    public List<GameObject> quarks;
    private GameObject newProton, newElectron;

    [Header("Start Positions")]
    [Space(10)]
    public Transform photonStartPosition;
    public Transform electronStartPosition, protonStartPosition, detectorStartPosition;

    [Header("Particles")]
    [Space(10)]
    public Particle protonParticle;
    public Particle electronParticle, photonParticle, playerParticle;

    [Header("Float Values and References")]
    public float fallingTime;
    public float fallingDistance, fallingSlerp;
    public FloatReference q2slider, playbackSpeed;
    private bool hasFallen;
    private BarrelState barrelState;

    [Header("Bools")]
    public GlobalBool firstPlayThrough;
    public GlobalBool isPlaying, showLabel;
    public bool enable2DQuarks;

    private float particlesCreated;

    [Header("Vector Constants")]
    public VectorConstant electronStartPositionVector;

    [Header("Events")]
    public UnityEvent StartPlaying; 
    public UnityEvent StopPlaying, basicInstructions, teleporterUnlocked, 
        dimProton, launchElectron, initializeEvent, showMenu, reset;
    // Start is called before the first frame update
    void Start()
    {
        barrelState = BarrelState.empty;
        firstPlayThrough.boolValue = true;
        isPlaying.boolValue = true;
        particlesCreated = 0;
        q2slider.variableSlider.value = 0;
        initializePointer.GetComponent<PointerMover>().MakeVisible();
        electronStartPositionVector.vectorValue = electronStartPosition.position;
        playbackSpeed.Value = 1;
        showLabel.boolValue = true;

        reset.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        //playbackSpeed.variableSlider.value = playbackCanvasSlider.value;
        //q2slider.variableSlider.value = q2CanvasSlider.value;

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
            launchElectron.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            initializeEvent.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKey(KeyCode.Y))
        {
            //q2CanvasSlider.value += .01f;
        }
        if (Input.GetKey(KeyCode.H))
        {
            //q2CanvasSlider.value -= .01f;
        }
        if (Input.GetKey(KeyCode.T))
        {
            //playbackCanvasSlider.value += .01f;
        }
        if (Input.GetKey(KeyCode.G))
        {
            //playbackCanvasSlider.value -= .01f;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            showMenu.Invoke();
        }
    }
    

    public void RevealTheQuarks()
    {

        if (enable2DQuarks)
        {
            foreach (GameObject quark in quarks)
            {
                quark.SetActive(true);
            }
        }
    }

    //methods for UI events
    public void Initiate()
    {
        GameObject newDetector = CreateNewObject(detector, detectorStartPosition.position, detectorStartPosition);
        newDetector.transform.rotation = detectorStartPosition.transform.rotation;
        CreateNewProtonAndElectron();
    }

    public void Launch()
    {
        detector.GetComponent<MeshCollider>().enabled = false;
        CreateNewProtonAndElectron();
        StartCoroutine(SettleParticlesFastAndLaunch());
    }

    public void EnableQ2Slider()
    {
        //q2CanvasSlider.interactable = true;
    }

    public void EnableTeleportButton()
    {
        //teleport.interactable = true;
    }

    public void ResetEverything()
    {
        SceneManager.LoadScene(0);
    }
    public void ShowPlayPointer()
    {
    }
    public void HidePlayPointer()
    {
    }

    public void PlayEverything()
    {
        StartPlaying.Invoke();
    }
    
    public void PauseEverything()
    {
        StopPlaying.Invoke();
    }

    public void ReloadText()
    {
        particlesCreated = 0;
        firstPlayThrough.boolValue = true;
    }

    private IEnumerator SettleParticlesFastAndLaunch()
    {
        newElectron.GetComponent<TransformObject>().CanBeMoved = false;
        fallingSlerp = 1f;
        yield return new WaitUntil(() => hasFallen);
        newElectron.GetComponent<TransformObject>().CanBeMoved = true;
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
                    dimProton.Invoke();
                else if (particlesCreated > 3 && particlesCreated < 5)
                    teleporterUnlocked.Invoke();

            }

            barrelState = BarrelState.full;
            newProton = CreateNewObject(proton, protonStartPosition.position, protonStartPosition);
            newElectron = CreateNewObject(electron, electronStartPositionVector.vectorValue, electronStartPosition);
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

    public GameObject CreateNewObject(GameObject objectType, Vector3 objectTransformPosition, Transform objectTransform)
    {
        fallingSlerp = .03f;
        Vector3 startPosition = objectTransformPosition + new Vector3(0, fallingDistance, 0);
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

