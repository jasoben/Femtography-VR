using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

}
