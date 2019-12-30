using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonController : MonoBehaviour
{

    public Particle particle;
    public GameObject photon;
    private float flipY;
    public float flipAmount;
    public GlobalBool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        flipY = 2.4f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, new Vector3(0,0,1000)) > 200)
        {
            Destroy(gameObject);
        }
        if (isPlaying.boolValue)
        {
            if (flipY > 2.35f)
            {
                flipAmount = -flipAmount;
            } else if (flipY < -2.35f)
            {
                flipAmount = -flipAmount;
            }
            flipY += flipAmount;
            photon.transform.localScale = new Vector3(2.4f, flipY, 1.2f);  
        }
    }


    public void DestroyPhoton()
    {
        Destroy(gameObject);
    }


}
