﻿using System.Collections;
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
    private bool shouldReturn;
    private bool shouldFollow;

    // Start is called before the first frame update
    void Start()
    {
        FollowOffsetPosition = new Vector3(50, -50, -100);
    }

    public void ReturnToPosition()
    {
        shouldReturn = true;
    }

    IEnumerator ReturnToPosCoroutine()
    {
        while (true)
        {
            // We adjust for playbackspeed in the following lerps
            Vector3 newPos = Vector3.MoveTowards(transform.position, playerFinalPosition.transform.position, moveSpeed * playBackSpeed.Value);
            transform.position = newPos;

            Quaternion newRot = Quaternion.Lerp(transform.rotation, playerFinalPosition.transform.rotation, .05f * playBackSpeed.Value);
            transform.rotation = newRot;

            if ((Vector3.Distance(transform.position, playerFinalPosition.transform.position) < 1) &&
                Quaternion.Angle(transform.rotation, playerFinalPosition.transform.rotation) < 1)
            {
                vehicleBackAtStart.Raise();
                yield break;
            } else
                yield return new WaitForFixedUpdate();
        }
    }

    public void SetParticle(string particleTag)
    {
        if (particleTag != "null")
        {
            Particle = GameObject.FindGameObjectWithTag(particleTag);

            if (particleTag == "Proton")
                FollowOffsetPosition = new Vector3(50, -50, -100);
            else if (particleTag == "photon")
                FollowOffsetPosition = new Vector3(50, -50, -40);

            if (isFollowingParticles.boolValue)
                shouldFollow = true;
        } else if (particleTag == "null")
            Particle = null;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (shouldReturn)
        {
            shouldFollow = false;
            Vector3 newPos = Vector3.MoveTowards(transform.position, playerFinalPosition.transform.position, moveSpeed * playBackSpeed.Value);
            transform.position = newPos;

            Quaternion newRot = Quaternion.Lerp(transform.rotation, playerFinalPosition.transform.rotation, .05f * playBackSpeed.Value);
            transform.rotation = newRot;

            if ((Vector3.Distance(transform.position, playerFinalPosition.transform.position) < 1) &&
                Quaternion.Angle(transform.rotation, playerFinalPosition.transform.rotation) < 1)
            {
                shouldReturn = false;
                vehicleBackAtStart.Raise();
            } 
        }
        if (shouldFollow)
        {
            Vector3 newPosition = Particle.transform.position + FollowOffsetPosition;
            transform.position = Vector3.Lerp(transform.position, newPosition, .05f);
            Vector3 directionTowardsParticle = Particle.transform.position - transform.position;
            Quaternion lookAtParticle = Quaternion.LookRotation(directionTowardsParticle);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAtParticle, .1f);
        }
    }
}
