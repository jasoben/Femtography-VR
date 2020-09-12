using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProtonController : MonoBehaviour
{
    public GameObject photon, photonCollider, protonLights;
    public GameObject[] quarks;
    public UnityEvent collisionWithProton, protonCreated, quarksRevealed, secondPhotonLaunched, protonDestroyed;
    public Particle particle;
    public float minXAngle, maxXAngle, minYAngle, maxYAngle;
    public GlobalBool firstPlayThrough, vehicleFollowingParticles, vehicleInFinalPosition;
    public RandomAngle photonAngle;
    public AudioSource collisionSound;
    public FloatReference q2;

    // Start is called before the first frame update
    void Start()
    {
        if (firstPlayThrough.boolValue)
        {
            particle.opacity.ConstantValue = 1f;
        }
        else
        {
            particle.opacity.ConstantValue = .1f;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (q2.Value > .2f)
        {
            foreach (GameObject quark in quarks)
            {
                quark.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject quark in quarks)
            {
                quark.SetActive(true);
            }
        }
    }

    public void DeformAndRotateProtonAndPlayCollisionSound()
    {
        float newXAngle = Random.Range(minXAngle, maxXAngle);
        float newYAngle = Random.Range(minYAngle, maxYAngle);
        Quaternion newRotation = Quaternion.Euler(newXAngle, newYAngle, transform.rotation.eulerAngles.z);
        transform.rotation = newRotation;
        protonLights.SetActive(false);
        collisionSound.time = .5f;
        collisionSound.Play();
    }

    public void RevealQuarks()
    {
        if (firstPlayThrough.boolValue)
        {
            quarksRevealed.Invoke();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "photon")
        {
            Destroy(other.gameObject);
            gameObject.layer = 14;
            DeformAndRotateProtonAndPlayCollisionSound();
            collisionWithProton.Invoke();
        }
    }

    public void LaunchPhoton()
    {
        float x = Random.Range(photonAngle.newAngleLowBound.x, photonAngle.newAngleHighBound.x);
        float y = Random.Range(photonAngle.newAngleLowBound.y, photonAngle.newAngleHighBound.y);
        float z = Random.Range(photonAngle.newAngleLowBound.z, photonAngle.newAngleHighBound.z);
        Quaternion newPhotonRotation = Quaternion.Euler(x, y, z);
        GameObject newPhoton = Instantiate(photon, transform.position + new Vector3(0,0,24), newPhotonRotation);
        protonLights.SetActive(true);
        Invoke("SecondPhotonLaunched", .5f);
    }

    private void SecondPhotonLaunched()
    {
        secondPhotonLaunched.Invoke();
    }

    public void DestroyProton()
    {
        protonDestroyed.Invoke();
        Destroy(gameObject);
    }

    public void DestroyIfNotFollowing() // if we aren't following the electron and we're still at the "start gate", 
        // we want to delete the proton 
        // when the system resets, otherwise we let it live until it hits the detector.
    {
        if (vehicleFollowingParticles.boolValue == false && vehicleInFinalPosition.boolValue == false)
        {
            DestroyProton();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "IntermediateTrigger")
        {
            RevealQuarks(); 
        }
        if (other.tag == "LaunchElectronTrigger")
        {
            LaunchPhoton();
        }
        if (other.tag == "DestructionTrigger")
        {
            DestroyProton();
        }
    }

    private void OnDestroy()
    {
    }
}
