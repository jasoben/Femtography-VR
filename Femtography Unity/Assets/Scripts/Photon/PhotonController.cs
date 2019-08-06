using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonController : MonoBehaviour
{

    public Animator photonAnimator;
    public Particle particle;


    // Start is called before the first frame update
    void Start()
    {
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
        photonAnimator.Play("LaunchPhoton", 0, 0);
        Destroy(gameObject, 8f);
    }

    public void DestroyPhoton()
    {
        Destroy(gameObject);
    }
}
