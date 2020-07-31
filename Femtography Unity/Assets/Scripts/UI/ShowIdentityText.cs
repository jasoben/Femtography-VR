using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIdentityText : MonoBehaviour
{
    public GlobalBool showText;
    private GameObject console;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.enabled = showText.boolValue;
    }

}
