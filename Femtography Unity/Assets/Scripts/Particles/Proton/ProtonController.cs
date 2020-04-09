﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProtonController : MonoBehaviour
{
    public GameObject photon, photonCollider, protonLights, idText;
    public GameObject[] quarks;
    public UnityEvent collisionWithProton, protonCreated, quarksRevealed, secondPhotonLaunched;
    public Particle particle;
    public float minXAngle, maxXAngle, minYAngle, maxYAngle;
    public GlobalBool firstPlayThrough;
    public RandomAngle photonAngle;
    public AudioSource collision;
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
        
        idText.GetComponent<MeshRenderer>().enabled = false;
    }

    public void ShowText()
    {
        idText.GetComponent<MeshRenderer>().enabled = true;
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

    public void DeformAndMoveProton()
    {
        float newXAngle = Random.Range(minXAngle, maxXAngle);
        float newYAngle = Random.Range(minYAngle, maxYAngle);
        Quaternion newRotation = Quaternion.Euler(newXAngle, newYAngle, transform.rotation.eulerAngles.z);
        GetComponent<TransformObject>().KineticSpeed = 1;
        transform.rotation = newRotation;
        protonLights.SetActive(false);
        collision.time = .5f;
        collision.Play();

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
        if (other.tag == "photonCollider")
        {
            Destroy(other.gameObject);
            gameObject.layer = 14;
            DeformAndMoveProton();
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
        newPhoton.GetComponent<TransformObject>().KineticSpeed = 1;
        protonLights.SetActive(true);
        Invoke("SecondPhotonLaunched", .5f);
    }

    private void SecondPhotonLaunched()
    {
        if (firstPlayThrough.boolValue)
        {
            secondPhotonLaunched.Invoke();
        }
    }

    public void DestroyProton()
    {
        Destroy(gameObject);
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
            Destroy(gameObject);
        }
    }
}
