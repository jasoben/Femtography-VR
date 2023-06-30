using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaAdjuster : MonoBehaviour
{
    public float alpha;
    private MaterialPropertyBlock materialPropertyBlock;
    public bool ChangeAlphaValue { get; private set; }
    private Renderer thisRenderer;

    // Start is called before the first frame update
    void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        thisRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ChangeAlphaValue)
            return;

        materialPropertyBlock.SetFloat("Alpha_", alpha);
        thisRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}
