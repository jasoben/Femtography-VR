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
        photonAnimator.speed = particle.playbackSpeed.Value;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, new Vector3(0,0,1000)) > 200)
        {
            Destroy(gameObject);
        }
    }

    public void MoveThePhoton()
    {
        transform.Translate(-26.6f, 0, 0, Space.Self);
    }

    public void StartPhotonAnimation()
    {
        photonAnimator.Play("LaunchPhoton", 0, 0);
    }

    public void DestroyPhoton()
    {
        Destroy(gameObject);
    }


}
