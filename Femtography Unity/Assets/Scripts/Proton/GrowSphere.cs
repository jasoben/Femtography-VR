using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowSphere : MonoBehaviour
{
    private float sphereSize;
    public float maxSphereSize = 10;
    public float containerSphereSize, sphereGrowSpeed;

    public GameObject opacityControlObject;

    // Start is called before the first frame update
    void Start()
    {
        sphereSize = 1;
        opacityControlObject = GameObject.Find("OpacityControlObject");
        ChangeColor();

   }


    // Update is called once per frame
    void Update()
    {
        if (PlayBackControl.isPlaying)
        {
            ChangeColor();
            sphereSize += .1f / sphereGrowSpeed;
            transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);
            if (sphereSize > maxSphereSize)
                Destroy(gameObject); 
        }
    }
    private void ChangeColor()
    {
        float redAmount = containerSphereSize / 10 / Mathf.Pow(maxSphereSize, 1);
        float blueAmount = Mathf.Pow(maxSphereSize, 2) / 400;
        float newOpacity = opacityControlObject.GetComponent<OpacityController>().GrowSphereOpacity;
        Color newColor = new Color(redAmount, -1, blueAmount, newOpacity);
        GetComponent<Renderer>().material.color = newColor;
    }
}
