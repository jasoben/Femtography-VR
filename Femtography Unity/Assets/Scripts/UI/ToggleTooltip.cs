using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTooltip : MonoBehaviour
{
    Image image;
    Text text;
    public GlobalBool showTooltip;

    bool isEnableable;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        HideToolTip();
    }

    // Update is called once per frame
    void Update()
    {
        isEnableable = showTooltip.boolValue;
    }

    public void ShowToolTip()
    {
        if (isEnableable)
        {
            image.enabled = true;
            text.enabled = true;
        }    
    }
    public void HideToolTip()
    {
        if (image != null)
            image.enabled = false;
        text.enabled = false;
    }
}
