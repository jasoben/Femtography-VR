using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonController : MonoBehaviour
{

    private GameObject masterControlObject, photonStartPositionObject;
    public Animator photonAnimator;

    // Start is called before the first frame update
    void Start()
    {
        masterControlObject = GameObject.Find("MasterControlObject");
        photonStartPositionObject = GameObject.Find("PhotonStartPosition");
        GetComponentInChildren<Renderer>().enabled = false;
        transform.position = photonStartPositionObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MoveThePhoton()
    {
        transform.Translate(-26.6f, 0, 0, Space.Self);
    }

    public void StartPhotonAnimation()
    {
        GetComponentInChildren<Renderer>().enabled = true;
        photonAnimator.Play("LaunchPhoton", 0, 0);
        Destroy(gameObject, 8f);
    }
}
