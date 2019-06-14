using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowSphere : MonoBehaviour
{
    private float sphereSize;
    public float maxSphereSize = 10;
    public float containerSphereSize;

    // Start is called before the first frame update
    void Start()
    {
        sphereSize = 1;

   }


    // Update is called once per frame
    void Update()
    {
        float redAmount = containerSphereSize / 10 / Mathf.Pow(maxSphereSize, 1);
        float blueAmount = Mathf.Pow(maxSphereSize, 2) / 400;
        Color newColor = new Color(redAmount, -1, blueAmount);
        GetComponent<Renderer>().material.color = newColor;
        GetComponent<Renderer>().material.SetColor("_EmissionColor", newColor);
        sphereSize += .1f;
        transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);
        if (sphereSize > maxSphereSize)
            Destroy(gameObject);
    }
}
