using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllipticalOrbit : MonoBehaviour
{

    private float a, b, x, y, alpha, X, Y;
    public GameObject largeSphere;
    [Range(0,20)]
    public float MagnitudeFraction;

    // Start is called before the first frame update
    void Start()
    {
        alpha = 0;
        y = 0;
        a = 5f;
        b = 5f;
    }

    // Update is called once per frame
    void Update()
    {

        a = 5f + MagnitudeFraction / 4;
        x = MagnitudeFraction / 4f;
        float scaleMagnitude = (MagnitudeFraction / 10) + 1;
        largeSphere.transform.localScale = new Vector3(scaleMagnitude,scaleMagnitude,scaleMagnitude) ;

        float distanceBetweenSpheres = Vector3.Distance(transform.position, largeSphere.transform.position);

        distanceBetweenSpheres = Mathf.Clamp(distanceBetweenSpheres, 0, 10);
        alpha += 15 - distanceBetweenSpheres;
        X = x + (a * Mathf.Cos(alpha *.005f));
        Y= y + (b * Mathf.Sin(alpha *.005f));
        this.gameObject.transform.position = new Vector3(X,1,Y);    
    }
}
