using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.Utility;
using System.Linq;

public class ShowMenu : MonoBehaviour
{
    public GameObject Console, Display, MenuObjectsParent;
    public float startWidth, endWidth, shiftSpeed, startAlpha, endAlpha, highlightAmount, unHighlightAmount, highlightSpeed;
    float currentHightlightAmount;
    MaterialPropertyBlock displayTubePropertyBlock, spherePropertyBlock;
    bool isTransmuting, openOrClose, highLighting;
    public UnityEvent openMenu, closeMenu;
    Renderer displayTubeRenderer, sphereRenderer;
    public GlobalBool menuOpen;
    List<Renderer> menuObjectRenderers = new List<Renderer>();
    List<Collider> menuObjectColliders = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {

        menuObjectRenderers = MenuObjectsParent.GetComponentsInChildren<Renderer>().ToList();
        menuObjectColliders = MenuObjectsParent.GetComponentsInChildren<Collider>().ToList();

        menuOpen.boolValue = false;
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
            menuOpen.boolValue = false; // we set this here so the menu shrinks correctly the instant it "close menu"
            // is initiated.
            closeMenu.Invoke();
            Console.SetActive(false);
            currentWidth = endWidth;
            currentAlpha = endAlpha;
            newShiftSpeed = -shiftSpeed;
            GetComponent<PhysicalButton>().regularAlphaAmount = .2f;
            foreach(UIScaler uiScaler in transform.parent.gameObject.GetComponentsInChildren<UIScaler>())
            {
                uiScaler.EnglargeOrShrink(false);
            }
        }
        else
        {
            foreach(UIScaler uiScaler in transform.parent.gameObject.GetComponentsInChildren<UIScaler>())
            {
                SetActiveMenuObjects(true);// show them when opening (hide happens at the end of this coroutine)
                uiScaler.EnglargeOrShrink(true);
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
                menuOpen.boolValue = true;
                yield break;
            } 
            else if (openOrClose && currentWidth > startWidth)
            {
                openOrClose = false;
                isTransmuting = false;
                GetComponent<PhysicalButton>().SetAlpha();
                SetActiveMenuObjects(false);// hide them when closed (open happens at the start of this coroutine)
                yield break;
            }
            else
                yield return new WaitForEndOfFrame();
        }
    }

    private void SetActiveMenuObjects(bool isEnabled)
    {
        foreach(Renderer renderer in menuObjectRenderers)
        {
            renderer.enabled = isEnabled;
        }
        foreach(Collider collider in menuObjectColliders)
        {
            collider.enabled = isEnabled;
        }
    }

    private void Update()
    {
        
    }
}
