using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public GameObject elevatorStart, elevatorStop,player;
    private bool goingUp = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (goingUp && Vector3.Distance(transform.position, elevatorStop.transform.position) > 2)
        {
            transform.Translate(Vector3.up * .1f);
            player.transform.Translate(Vector3.up * .1f);

        } else if (!goingUp && Vector3.Distance(transform.position, elevatorStart.transform.position) > 2)
        {
            transform.Translate(-Vector3.up * .1f);
            player.transform.Translate(-Vector3.up * .1f);

        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BodyCollider")
            goingUp = true;
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "BodyCollider")
            goingUp = false;
    }
}
