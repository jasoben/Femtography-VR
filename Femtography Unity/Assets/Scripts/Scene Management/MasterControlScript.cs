using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MasterControlScript : MonoBehaviour
{
    public GameObject proton, photon, electron, sensor, player, initializePointer;
    public List<GameObject> quarks;
    public bool enable2DQuarks;
    private GameObject newProton, newElectron;
    public Transform photonStartPosition, electronStartPosition, protonStartPosition, sensorStartPosition;
    public Particle protonParticle, electronParticle, photonParticle, playerParticle;
    public UnityEvent StartPlaying, StopPlaying, basicInstructions, teleporterUnlocked, dimProton, launchElectron, initializeEvent, showMenu;
    public float fallingTime, fallingDistance, fallingSlerp;
    private bool hasFallen;
    private BarrelState barrelState;
    public GlobalBool firstPlayThrough, isPlaying;
    public FloatReference q2slider;
    public FloatReference playbackSpeed;
    public Slider q2CanvasSlider, playbackCanvasSlider;
    public Button initialize, launch, teleport, play, pause;
    private float particlesCreated;
    public VectorConstant electronStartPositionVector;

    // Start is called before the first frame update
    void Start()
    {
        launch.interactable = false;
        teleport.interactable = false;
        play.interactable = false;
        pause.interactable = false;
        q2CanvasSlider.interactable = false;

        barrelState = BarrelState.empty;
        firstPlayThrough.boolValue = true;
        isPlaying.boolValue = true;
        particlesCreated = 0;
        q2slider.variableSlider.value = 0;
        initializePointer.GetComponent<PointerMover>().MakeVisible();
        play.interactable = false;
        pause.interactable = false;
        electronStartPositionVector.vectorValue = electronStartPosition.position;
        playbackSpeed.Value = 1;
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
            q2CanvasSlider.value += .01f;
        }
        if (Input.GetKey(KeyCode.H))
        {
            q2CanvasSlider.value -= .01f;
        }
        if (Input.GetKey(KeyCode.T))
        {
            playbackCanvasSlider.value += .01f;
        }
        if (Input.GetKey(KeyCode.G))
        {
            playbackCanvasSlider.value -= .01f;
        }
        if (Input.GetKey(KeyCode.M))
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
        initialize.interactable = false;
        launch.interactable = true;
        CreateNewProtonAndElectron();
        CreateNewObject(sensor, protonStartPosition.position, protonStartPosition);
    }

    public void Launch()
    {
        play.interactable = true;
        pause.interactable = true;
        sensor.GetComponent<MeshCollider>().enabled = false;
        CreateNewProtonAndElectron();
        StartCoroutine(SettleParticlesFastAndLaunch());
    }

    public void EnableQ2Slider()
    {
        q2CanvasSlider.interactable = true;
    }

    public void EnableTeleportButton()
    {
        teleport.interactable = true;
    }

    public void ResetEverything()
    {
        SceneManager.LoadScene(0);
    }
    public void ShowPlayPointer()
    {
        play.interactable = true;
        pause.interactable = false;
    }
    public void HidePlayPointer()
    {
        play.interactable = false;
        pause.interactable = true;
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
        fallingSlerp = 1f;
        yield return new WaitUntil(() => hasFallen);
        launchElectron.Invoke();
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

