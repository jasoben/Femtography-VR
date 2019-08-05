using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MasterControlScript : MonoBehaviour
{
    public UnityEvent launchEvent, collideEvent, playEvent, pauseEvent;
    public GameObject proton, photon, electron, photonStartPosition, electronStartPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (launchEvent == null)
            launchEvent = new UnityEvent();
        if (playEvent == null)
            playEvent = new UnityEvent();
        if (collideEvent == null)
            collideEvent = new UnityEvent();
        if (pauseEvent == null)
            pauseEvent = new UnityEvent();

        pauseEvent.AddListener(PauseEverything);
        playEvent.AddListener(PlayEverything);
        launchEvent.AddListener(LaunchElectron);
        launchEvent.AddListener(PlayEverything);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && launchEvent != null)
        {
            playEvent.Invoke();
        }

        else if (Input.GetKeyDown(KeyCode.P) && pauseEvent!=null)
        {
            pauseEvent.Invoke();
        }

        else if (Input.GetKeyDown(KeyCode.L))
        {
            launchEvent.Invoke();
        }
    }

    private void PauseEverything()
    {
        PlayBackControl.StopPlaying();
    }

    private void PlayEverything()
    {
        PlayBackControl.StartPlaying();
    }

    public void LaunchElectron()
    {
        Vector3 theElectronStartPosition = electronStartPosition.transform.position;
        GameObject newElectron = Instantiate(electron, theElectronStartPosition, Quaternion.identity);
        Vector3 thePhotonStartPosition = photonStartPosition.transform.position;
        Quaternion randomRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        GameObject newPhoton = Instantiate(photon, thePhotonStartPosition, randomRotation);
        newElectron.GetComponent<ElectronController>().MyPhoton = newPhoton;
    }

}
