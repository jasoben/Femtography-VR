using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIHelper : MonoBehaviour
{
    public MenuManagerObject menuManagerObject;// Sometimes the menu is active and sometimes it isn't, so we enable
    // and disable objects through ScriptableObject data instead of directly. See the "MenuManager" under "Control Objects" 
    // in the hierarchy.
    ToggleTooltip toggleTooltip;

    public ObjectReference toolTipObjectReference;

    public GlobalBool showText, showMenu;

    bool referenceObjectSet;

    Text toolTipTextObject;

    List<ToolTipText> toolTips = new List<ToolTipText>();

    public float flashingScaleCoefficient;

    Vector3 startScale, flashingScale;

    // Start is called before the first frame update
    void Start()
    {
        toolTips = XmlParser.Read<ToolTipText>("assets/XML/toolTips.xml");
        startScale = transform.localScale;
        flashingScale = startScale * flashingScaleCoefficient;
    }
    private void OnEnable()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (toolTipObjectReference.referencedGameObject != null)
        {
            referenceObjectSet = true;
            toolTipTextObject = toolTipObjectReference.referencedGameObject.GetComponent<Text>();
        }
        if (referenceObjectSet)
        {
            if (menuManagerObject.isFlashing)
            {
                transform.localScale = Vector3.Lerp(startScale, flashingScale, FlashingController.FlashLerp);
            }
        }
    }

    public void ToggleOnOrOff(bool onOrOff)
    {
    }

    private void OnMouseEnter()
    {
        if (showText.boolValue && referenceObjectSet)
        {
            toolTipTextObject.text = FindToolTipText(menuManagerObject.toolTipTitle);
        }
    }
    private void OnMouseExit()
    {
        toolTipTextObject.text = "";
    }

    string FindToolTipText(string toolTipTitle)
    {
        string toolTipText = default;
        foreach (ToolTipText toolTip in toolTips)
        {
            if (toolTip.title == toolTipTitle)
                toolTipText = toolTip.tip;
        }
        return toolTipText;
    }

}

public class ToolTipText
{
    public string title;
    public string tip;
}
