using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIdentityText : MonoBehaviour
{
    public GlobalBool showText;
    private GameObject console;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        //console = GameObject.Find("Console");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<MeshRenderer>().enabled = showText.boolValue;
        //transform.rotation = Quaternion.LookRotation((transform.position - console.transform.position), Vector3.up);
    }

}
