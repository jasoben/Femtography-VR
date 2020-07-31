using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIHelper : MonoBehaviour
{
    public bool highlightButton { get; set; }
    Color originalColor, flashingColor, newColor;
    ColorBlock toggleColorBlock;
    public MenuManagerObject menuManagerObject;// Sometimes the menu is active and sometimes it isn't, so we enable
    // and disable objects through ScriptableObject data instead of directly. See the "MenuManager" under "Control Objects" 
    // in the hierarchy.
    Button button;
    Image image;
    Toggle toggle;
    ToggleTooltip toggleTooltip;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Image>() != null)
            originalColor = GetComponent<Image>().color;
        else
            originalColor = Color.white;
        flashingColor = Color.gray;
        newColor = new Color();
        if (GetComponent<Button>() != null)
            button = GetComponent<Button>();
        if (GetComponent<Image>() != null)
            image = GetComponent<Image>();
        if (GetComponent<Toggle>() != null)
        {
            toggle = GetComponent<Toggle>();
            toggleColorBlock = toggle.colors;
        }

        toggleTooltip = GetComponentInChildren<ToggleTooltip>();

        // We define these events in code since we have so many objects that use them, and it would be inefficient to set them all
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
            newColor = Color.Lerp(originalColor, flashingColor, FlashingController.FlashLerp);
            if (image != null)
                image.color = newColor;
            if (toggle != null)
            {
                toggleColorBlock.normalColor = newColor;
                toggle.colors = toggleColorBlock;
                toggle.transform.Find("Label").gameObject.GetComponent<Text>().color = newColor;
            }
        }

        if (button != null)
            button.interactable = menuManagerObject.isActive;
        else if (toggle != null)
            toggle.interactable = menuManagerObject.isActive;

        highlightButton = menuManagerObject.isFlashing;

        if (gameObject.name == "Play")
            DebugUI.ShowText("Play is active: ", menuManagerObject.isActive.ToString());
    }


    private void OnMouseEnter()
    {
        transform.Find("Tooltip").GetComponent<ToggleTooltip>().ShowToolTip();
    }
    private void OnMouseExit()
    {
        transform.Find("Tooltip").GetComponent<ToggleTooltip>().HideToolTip();
    }

}
