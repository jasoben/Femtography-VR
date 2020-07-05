using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMeshCollisionSensor : MonoBehaviour
{
    Color hitColor = Color.green;
    MaterialPropertyBlock materialPropertyBlock;
    // Start is called before the first frame update
    void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetColor("_BaseColor", hitColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
        Debug.Log("stuff");
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
