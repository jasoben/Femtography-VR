using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float lifeTime;
    public FloatReference Q2;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyThis", lifeTime * Q2.Value); 
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
