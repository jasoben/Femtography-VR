using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowOverTime : MonoBehaviour
{
    public float normalGrowSpeed;
    public FloatReference playbackSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float growSpeed = normalGrowSpeed * playbackSpeed.Value;
        gameObject.transform.localScale += new Vector3(growSpeed, growSpeed, growSpeed);
    }
}
