using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIHelper : MonoBehaviour
{
    public bool highlightButton { get; set; }
    Color baseColor, flashingColor, newColor, regularColor, highlightColor;
    ColorBlock toggleColorBlock, buttonColorBlock;
    public MenuManagerObject menuManagerObject;// Sometimes the menu is active and sometimes it isn't, so we enable
    // and disable objects through ScriptableObject data instead of directly. See the "MenuManager" under "Control Objects" 
    // in the hierarchy.
    Image image;
    Toggle toggle;
    ToggleTooltip toggleTooltip;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Image>() != null)
            baseColor = GetComponent<Image>().color;
        else
            baseColor = Color.white;
        flashingColor = Color.gray;
        newColor = new Color();
        if (GetComponent<Image>() != null)
            image = GetComponent<Image>();
        if (GetComponent<Toggle>() != null)
        {
            toggle = GetComponent<Toggle>();
            toggleColorBlock = toggle.colors;
        }

        highlightColor = new Color(255/255f, 241/255f, 0, .7f);
        regularColor = new Color(124/255f, 144/255f, 191/255f, .7f);
        //regularColor = Color.blue;

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
        if (highlightButton)
        {
            newColor = Color.Lerp(highlightColor, flashingColor, FlashingController.FlashLerp);
            if (image != null)
                image.color = newColor;
            if (toggle != null)
            {
                toggleColorBlock.normalColor = newColor;
                toggle.colors = toggleColorBlock;
                toggle.transform.Find("Label").gameObject.GetComponent<Text>().color = newColor;
            }
        }
        else if (!highlightButton)
        {
            if (image != null)
                image.color = baseColor;
            if (toggle != null)
            {
                toggleColorBlock.normalColor = baseColor;
                toggle.colors = toggleColorBlock;
                toggle.transform.Find("Label").gameObject.GetComponent<Text>().color = baseColor;
            }
        }

        if (toggle != null)
            toggle.interactable = menuManagerObject.isActive;

        highlightButton = menuManagerObject.isFlashing;
    }

    public void ToggleOnOrOff(bool onOrOff)
    {
        if (toggle.interactable)
            toggle.isOn = onOrOff;
    }

}
