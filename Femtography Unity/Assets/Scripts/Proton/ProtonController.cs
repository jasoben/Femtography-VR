using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtonController : MonoBehaviour
{
    private Animator protonAnimator;
    // Start is called before the first frame update
    void Start()
    {
        protonAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeformProton()
    {
        protonAnimator.Play("DeformProton", 0, 0);
    }
}
