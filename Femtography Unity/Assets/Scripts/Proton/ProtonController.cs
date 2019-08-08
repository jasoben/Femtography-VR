using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProtonController : MonoBehaviour
{
    private Animator protonAnimator;
    public GameObject photon, photonCollider, protonLights, protonShell;
    public UnityEvent collisionWithProton, protonCreated;
    public Particle particle;
    public float minXAngle, maxXAngle, minYAngle, maxYAngle;
    public GlobalBool firstPlayThrough;
    public RandomAngle photonAngle;

    // Start is called before the first frame update
    void Start()
    {
        protonAnimator = GetComponentInChildren<Animator>();
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
    }

    public void DeformAndMoveProton()
    {
        protonAnimator.Play("DeformProton", 0, 0);
        float newXAngle = Random.Range(minXAngle, maxXAngle);
        float newYAngle = Random.Range(minYAngle, maxYAngle);
        Quaternion newRotation = Quaternion.Euler(newXAngle, newYAngle, transform.rotation.eulerAngles.z);
        GetComponent<TransformObject>().KineticSpeed = 1;
        transform.rotation = newRotation;
        protonShell.SetActive(false);
        protonLights.SetActive(false);
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
        GameObject newPhoton = Instantiate(photon, transform.position, newPhotonRotation);
        newPhoton.GetComponent<PhotonController>().StartPhotonAnimation();
        protonShell.SetActive(true);
        protonLights.SetActive(true);

    }

    public void DestroyProton()
    {
        Destroy(gameObject);
    }

    void OnTriggerExit(Collider other)
    {
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
