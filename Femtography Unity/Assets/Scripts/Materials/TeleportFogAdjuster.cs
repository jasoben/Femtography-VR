using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportFogAdjuster : MonoBehaviour
{
    MaterialPropertyBlock fogPropertyBlock;
    Renderer renderer;
    float upperEdge, edgeLerp, startEdge = 4, endEdge = .24f;
    [SerializeField] float lerpSpeed;
    bool isHoveredOver;

    // Start is called before the first frame update
    void Start()
    {
        fogPropertyBlock = new MaterialPropertyBlock();
        renderer = GetComponent<Renderer>();
    }

    public void OnHover()
    {
        isHoveredOver = true;
    }

    public void OnHoverExit()
    {
        isHoveredOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ( !isHoveredOver && edgeLerp < .01f )
        {
            gameObject.SetActive( false );
        }
            
        else if ( isHoveredOver && edgeLerp > .99f)
            return;

        if (isHoveredOver)
        {
            edgeLerp += lerpSpeed;
        }

        else if (!isHoveredOver)
        {
            edgeLerp -= lerpSpeed;
        }

        edgeLerp = Mathf.Clamp01(edgeLerp);

        fogPropertyBlock.SetFloat("_Edge2", Mathf.Lerp(startEdge, endEdge, edgeLerp));
        renderer.SetPropertyBlock(fogPropertyBlock);
    }
}
