using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtonTransform : MonoBehaviour
{
    public GameObject electron;
    public float distanceToElectron;
    private bool moveOrNot;
    public GameObject masterControlObject;
    
    // Start is called before the first frame update
    void Start()
    {
        moveOrNot = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (moveOrNot)
        {
            if (Vector3.Distance(gameObject.transform.position, electron.transform.position) > distanceToElectron)
            {
                gameObject.transform.Translate(1, 0, 0);
            }
            else
            {
                moveOrNot = false;
                masterControlObject.GetComponent<MasterControlScript>().Collided();
            }
        }

        
    }

    public void StartMovingProton()
    {
        moveOrNot = true;
    }

    public void StopMovingProton()
    {
        moveOrNot = false;
    }
}
