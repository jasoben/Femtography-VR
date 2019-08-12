using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIdentityText : MonoBehaviour
{
    public GlobalBool firstPlayThrough;
    private GameObject console;
    // Start is called before the first frame update
    void Start()
    {
        console = GameObject.Find("Console");
    }

    // Update is called once per frame
    void Update()
    {
        if (!firstPlayThrough.boolValue)
            gameObject.SetActive(false);
        transform.rotation = Quaternion.LookRotation((transform.position - console.transform.position), Vector3.up);
    }
}
