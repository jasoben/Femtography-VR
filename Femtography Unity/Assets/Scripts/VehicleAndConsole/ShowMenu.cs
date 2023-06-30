using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.Utility;
using System.Linq;
using UnityEngine.Serialization;

public class ShowMenu : MonoBehaviour
{
    [SerializeField] private GameObject display, MenuObjectsParent;
    [SerializeField] GameObject expandedPanel;

    [SerializeField] private float scaleMenuButtonAdjuster;

    private Animator animator;

    bool scaleUpMenuOpenButton;
    [SerializeField] private GlobalBool menuOpen;

    [SerializeField] private UnityEvent openMenu, closeMenu;
    Renderer displayTubeRenderer, sphereRenderer;

    List<Renderer> menuObjectRenderers = new List<Renderer>();
    List<Collider> menuObjectColliders = new List<Collider>();

    UIScaler showHideMenuButtonScaler;

    // Start is called before the first frame update
    void Start()
    {
        showHideMenuButtonScaler = GetComponent<UIScaler>();
        expandedPanel.SetActive(false);
        animator = display.GetComponent<Animator>();
        animator.enabled = false;

        menuObjectRenderers = MenuObjectsParent.GetComponentsInChildren<Renderer>().ToList();
        menuObjectColliders = MenuObjectsParent.GetComponentsInChildren<Collider>().ToList();

        menuOpen.boolValue = false;
        displayTubeRenderer = display.GetComponent<Renderer>();
        sphereRenderer = GetComponent<Renderer>();
    }

    public void StartOpeningAnimation()
    {
        animator.enabled = true;
        animator.SetTrigger("MenuOpen");
        showHideMenuButtonScaler.EnglargeOrShrink(false);

        foreach(UIScaler uiScaler in MenuObjectsParent.GetComponentsInChildren<UIScaler>())
        {
            if (uiScaler.IncludeInListOfMenuButtons)// Ignore some buttons
                uiScaler.EnglargeOrShrink(true);
        }

    }

    public void OnOpeningAnimationComplete()
    {
        openMenu.Invoke();
        menuOpen.boolValue = true;

        expandedPanel.SetActive(true);// Show the expanded panel (without the dongle)

        SetActiveMenuObjects(true);// show them when opening (hide happens at the end of this coroutine)

        foreach (UIScaler uiScaler in MenuObjectsParent.GetComponentsInChildren<UIScaler>())
        {
            if (uiScaler.IncludeInListOfMenuButtons)// Ignore some buttons
                uiScaler.EnglargeOrShrink(true);
        }
        scaleUpMenuOpenButton = true;
    }

    public void StartCloseAnimation()
    {
        foreach(UIScaler uiScaler in MenuObjectsParent.GetComponentsInChildren<UIScaler>())
        {
            if (uiScaler.IncludeInListOfMenuButtons)// Ignore some buttons
                uiScaler.EnglargeOrShrink(false);
        }
        animator.SetTrigger("MenuClosed");
        expandedPanel.SetActive(false);// Hide the expanded panel (without the dongle)
    }

    public void OnCloseAnimationComplete()
    {
        menuOpen.boolValue = false;
        SetActiveMenuObjects(false);// hide them when closed (open happens at the start of this coroutine)
        scaleUpMenuOpenButton = false;
        showHideMenuButtonScaler.EnglargeOrShrink(true);
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
