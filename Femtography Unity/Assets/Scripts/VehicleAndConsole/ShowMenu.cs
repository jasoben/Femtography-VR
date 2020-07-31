using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.Utility;

public class ShowMenu : MonoBehaviour
{
    public GameObject Console, Display;
    public float startWidth, endWidth, shiftSpeed;
    MaterialPropertyBlock transmuteMaterialPropertyBlock;
    bool isTransmuting, openOrClose;
    public UnityEvent openMenu, closeMenu;
    Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        Console.SetActive(false);
        transmuteMaterialPropertyBlock = new MaterialPropertyBlock();
        transmuteMaterialPropertyBlock.SetFloat("Width", startWidth);
        renderer = Display.GetComponent<Renderer>();
        renderer.SetPropertyBlock(transmuteMaterialPropertyBlock);
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
        if (openOrClose) // If it's open, we reverse the procedure
        {
            closeMenu.Invoke();
            Console.SetActive(false);
            currentWidth = endWidth;
            newShiftSpeed = -shiftSpeed;
        }
        while (true)
        {
            currentWidth -= newShiftSpeed; 
            transmuteMaterialPropertyBlock.SetFloat("Width", currentWidth);
            renderer.SetPropertyBlock(transmuteMaterialPropertyBlock);
            if (!openOrClose && currentWidth < endWidth)
            {
                Console.SetActive(true);
                openOrClose = true;
                isTransmuting = false;
                openMenu.Invoke();
                yield break;
            } 
            else if (openOrClose && currentWidth > startWidth)
            {
                openOrClose = false;
                isTransmuting = false;
                yield break;
            }
            else
                yield return new WaitForEndOfFrame();
        }
    }

    public void Highlight()
    {
        transmuteMaterialPropertyBlock.SetFloat("Gray_", .12f);
        renderer.SetPropertyBlock(transmuteMaterialPropertyBlock);
    }

    public void UnHighlight()
    {
        transmuteMaterialPropertyBlock.SetFloat("Gray_", .08f);
        renderer.SetPropertyBlock(transmuteMaterialPropertyBlock);
    }

}
