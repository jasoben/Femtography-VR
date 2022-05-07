using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhoEnabledMe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Debug.Log("I just got enabled!", this);
    }

}
