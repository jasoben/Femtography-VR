using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyThis", lifeTime); 
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
