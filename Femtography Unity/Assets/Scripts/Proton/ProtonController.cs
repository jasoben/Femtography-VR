using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProtonController : MonoBehaviour
{
    private Animator protonAnimator;
    public GameObject photon, photonCollider;
    public UnityEvent collisionWithProton, protonCreated;
    public Particle particle;

    // Start is called before the first frame update
    void Start()
    {
        particle.speed = 0; 
        protonAnimator = GetComponentInChildren<Animator>();
        protonCreated.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeformAndMoveProton()
    {
        protonAnimator.Play("DeformProton", 0, 0);
        float newXAngle = Random.Range(315, 345);
        float newYAngle = Random.Range(270, 315);
        Quaternion newRotation = Quaternion.Euler(newXAngle, newYAngle, transform.rotation.eulerAngles.z);
        transform.rotation = newRotation;
        particle.speed = 1f;
        Destroy(gameObject, 4f);
        Invoke("LaunchPhoton", .5f);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        gameObject.GetComponent<SphereCollider>().enabled = false;
        collisionWithProton.Invoke();
    }

    public void LaunchPhoton()
    {
        Quaternion newPhotonRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        GameObject newPhoton = Instantiate(photon, transform.position, newPhotonRotation);
        Quaternion newPhotonColliderRotation = Quaternion.LookRotation(-newPhoton.transform.right, -newPhoton.transform.up);
        GameObject newPhotonCollider = Instantiate(photonCollider, transform.position, newPhotonColliderRotation);
        newPhoton.GetComponent<PhotonController>().StartPhotonAnimation();
        newPhotonCollider.GetComponent<PhotonColliderLauncher>().LaunchPhotonCollider();
    }

    public void DestroyProton()
    {
        Destroy(gameObject);
    }
}
