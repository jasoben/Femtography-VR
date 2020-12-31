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

    public GlobalBool showText, menuOpen;

    bool referenceObjectSet;

    Text toolTipTextObject;

    List<ToolTipText> toolTips = new List<ToolTipText>();

    public float flashingScaleCoefficient;

    Vector3 startScale, flashingScale;

    // Start is called before the first frame update
    void Start()
    {
        toolTips = XmlParser.Read<ToolTipText>("assets/XML/toolTips.xml");
    }
    public void SetScale()
    {
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
        if (referenceObjectSet && menuOpen.boolValue)
        {
            if (menuManagerObject.isFlashing)
            {
                transform.localScale = Vector3.Lerp(startScale, flashingScale, FlashingController.FlashLerp);
            } else if (!menuManagerObject.isFlashing)
                StartCoroutine(ReturnToNormalScale());
        }
        if (menuManagerObject.isActive != GetComponent<BoxCollider>().enabled) // if the 
            // value changes 
        {
            GetComponent<PhysicalButton>().EnableDisable();
            GetComponent<BoxCollider>().enabled = menuManagerObject.isActive;
        }
    }

    IEnumerator ReturnToNormalScale()
    {
        Vector3 currentStartScale = transform.localScale, currentScale;
        float scaleAmount = 0;

        while (true)
        {
            currentScale = Vector3.Lerp(currentStartScale, startScale, scaleAmount);
            transform.localScale = currentScale;
            scaleAmount += .05f;

            if (scaleAmount > 1)
                yield break;
            else
                yield return new WaitForEndOfFrame();
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
