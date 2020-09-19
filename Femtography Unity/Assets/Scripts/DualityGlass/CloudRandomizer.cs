using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudRandomizer : MonoBehaviour
{
    MaterialPropertyBlock materialPropertyBlock;
    Vector3 rotationAxis;
    // Start is called before the first frame update
    void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetVector("RandomSeed_", new Vector4(Random.Range(0,10), Random.Range(0,10), 0, 0));
        GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
        Invoke("DestroyCloud", 2);
        rotationAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

    }

    private void Update()
    {
        transform.RotateAround(transform.parent.position, rotationAxis, 1f);
    }

    void DestroyCloud()
    {
        Destroy(gameObject);
    }
}
