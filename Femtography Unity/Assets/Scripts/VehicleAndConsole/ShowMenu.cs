using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.Utility;

public class ShowMenu : MonoBehaviour
{
    public GameObject Console, Display;
    public float startWidth, endWidth, shiftSpeed, startAlpha, endAlpha, highlightAmount, unHighlightAmount, highlightSpeed;
    float currentHightlightAmount;
    MaterialPropertyBlock displayTubePropertyBlock, spherePropertyBlock;
    bool isTransmuting, openOrClose, highLighting;
    public UnityEvent openMenu, closeMenu;
    Renderer displayTubeRenderer, sphereRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Console.SetActive(false);
        currentHightlightAmount = unHighlightAmount;
        displayTubePropertyBlock = new MaterialPropertyBlock();
        spherePropertyBlock = new MaterialPropertyBlock();
        displayTubePropertyBlock.SetFloat("Width", startWidth);
        displayTubePropertyBlock.SetFloat("Alpha_", startAlpha);
        displayTubeRenderer = Display.GetComponent<Renderer>();
        sphereRenderer = GetComponent<Renderer>();
        displayTubeRenderer.SetPropertyBlock(displayTubePropertyBlock);
    }

    public void ShowOrHideTheMenu()
    {
        if (!isTransmuting)
            StartCoroutine(TransmuteMenu());
    }

    IEnumerator TransmuteMenu()
    {
        isTransmuting = true;
        float newShiftSpeed = shiftSpeed;
        float currentWidth = startWidth;
        float currentAlpha = startAlpha;

        GetComponent<PhysicalButton>().regularAlphaAmount = .02f;

        if (openOrClose) // If it's open, we reverse the procedure
        {
            closeMenu.Invoke();
            Console.SetActive(false);
            currentWidth = endWidth;
            currentAlpha = endAlpha;
            newShiftSpeed = -shiftSpeed;
            GetComponent<PhysicalButton>().regularAlphaAmount = .2f;
            foreach(PhysicalButton physicalButton in transform.parent.gameObject.GetComponentsInChildren<PhysicalButton>())
            {
                if (physicalButton.isScalable)
                    physicalButton.EnglargeOrShrink(false);
            }
        }
        else
        {
            foreach(PhysicalButton physicalButton in transform.parent.gameObject.GetComponentsInChildren<PhysicalButton>())
            {
                if (physicalButton.isScalable)
                    physicalButton.EnglargeOrShrink(true);
            }
        }


        float alphaShiftSpeed = (-newShiftSpeed / (startWidth - endWidth)) * (endAlpha - startAlpha); // Adjust alpha shift speed between start and end alpha to match shift speed 
        // between arbitrary widths, and also reverse it
        while (true)
        {
            currentWidth -= newShiftSpeed; 
            currentAlpha -= alphaShiftSpeed;
            displayTubePropertyBlock.SetFloat("Width", currentWidth);
            displayTubePropertyBlock.SetFloat("Alpha_", currentAlpha);
            displayTubeRenderer.SetPropertyBlock(displayTubePropertyBlock);
            if (!openOrClose && currentWidth < endWidth)
            {
                Console.SetActive(true);
                openOrClose = true;
                isTransmuting = false;
                openMenu.Invoke();
                GetComponent<PhysicalButton>().SetAlpha();
                yield break;
            } 
            else if (openOrClose && currentWidth > startWidth)
            {
                openOrClose = false;
                isTransmuting = false;
                GetComponent<PhysicalButton>().SetAlpha();
                yield break;
            }
            else
                yield return new WaitForEndOfFrame();
        }
    }


    private void Update()
    {
        
    }
}
