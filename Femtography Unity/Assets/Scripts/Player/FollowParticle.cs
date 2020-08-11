using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParticle : MonoBehaviour
{
    public GameObject Particle { get; set; }
    public GameObject playerFinalPosition;

    public Vector3 FollowOffsetPosition;
    public GlobalBool isFollowingParticles;
    public float moveSpeed;

    public GameEvent vehicleBackAtStart;

    public FloatReference playBackSpeed;

    // Start is called before the first frame update
    void Start()
    {
        FollowOffsetPosition = new Vector3(50, -50, -100);
    }

    public void ReturnToPosition()
    {
        StopAllCoroutines();
        StartCoroutine(ReturnToPosCoroutine());
    }

    IEnumerator ReturnToPosCoroutine()
    {
        while (true)
        {
            // We adjust for playbackspeed in the following lerps
            Vector3 newPos = Vector3.MoveTowards(transform.position, playerFinalPosition.transform.position, moveSpeed * playBackSpeed.Value);
            transform.position = newPos;

            Quaternion newRot = Quaternion.Lerp(transform.rotation, Quaternion.identity, .05f * playBackSpeed.Value);
            transform.rotation = newRot;

            if ((Vector3.Distance(transform.position, playerFinalPosition.transform.position) < 1) &&
                Quaternion.Angle(transform.rotation, Quaternion.identity) < 1)
            {
                vehicleBackAtStart.Raise();
                yield break;
            } else
                yield return new WaitForFixedUpdate();
        }
    }

    public void SetParticle(string particleTag)
    {
        StopAllCoroutines();
        if (particleTag != "null")
        {
            Particle = GameObject.FindGameObjectWithTag(particleTag);

            if (particleTag == "Proton")
                FollowOffsetPosition = new Vector3(50, -50, -100);
            else if (particleTag == "photon")
                FollowOffsetPosition = new Vector3(50, -50, -40);

            if (isFollowingParticles.boolValue)
                StartCoroutine(WaitThenStartFollow());
        } else if (particleTag == "null")
            Particle = null;
    }

    IEnumerator WaitThenStartFollow()
    {
        yield return new WaitForSeconds(.3f);
        while (true)
        {
            Vector3 newPosition = Particle.transform.position + FollowOffsetPosition;
            transform.position = Vector3.Lerp(transform.position, newPosition, .05f);
            Vector3 directionTowardsParticle = Particle.transform.position - transform.position;
            Quaternion lookAtParticle = Quaternion.LookRotation(directionTowardsParticle);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAtParticle, .05f);
            yield return new WaitForFixedUpdate();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
