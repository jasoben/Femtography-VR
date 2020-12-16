﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIHelper : MonoBehaviour
{
    public MenuManagerObject menuManagerObject;// Sometimes the menu is active and sometimes it isn't, so we enable
    // and disable objects through ScriptableObject data instead of directly. See the "MenuManager" under "Control Objects" 
    // in the hierarchy.
    Image image;
    Toggle toggle;
    ToggleTooltip toggleTooltip;

    // Start is called before the first frame update
    void Start()
    {
        toggleTooltip = GetComponentInChildren<ToggleTooltip>();

        // We define these OnPointer events in code since we have so many objects that use them, and it would be inefficient to set them all
        // up manually since they do the same thing.

        gameObject.AddComponent<EventTrigger>();
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerEnterEntry.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
        trigger.triggers.Add(pointerEnterEntry);

        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerExitEntry.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
        trigger.triggers.Add(pointerExitEntry);
    }
    private void OnEnable()
    {
        if (toggle != null && toggle.isActiveAndEnabled)
            toggle.isOn = menuManagerObject.isOn;
    }

    public void OnPointerEnterDelegate(PointerEventData data)
    {
        toggleTooltip.ShowToolTip();
    }

    public void OnPointerExitDelegate(PointerEventData data)
    {
        toggleTooltip.HideToolTip();
    }
    // Update is called once per frame
    void Update()
    {
        if (toggle != null)
            toggle.interactable = menuManagerObject.isActive;
    }

    public void ToggleOnOrOff(bool onOrOff)
    {
        if (toggle.interactable)
            toggle.isOn = onOrOff;
    }


}
