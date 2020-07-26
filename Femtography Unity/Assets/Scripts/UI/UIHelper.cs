using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour
{
    public bool highlightButton { get; set; }
    Color originalColor, flashingColor, newColor;
    float flashSpeed = .005f;
    float flashLerp = 0;
    int upOrDown;
    public MenuManagerObject menuManagerObject;// Sometimes the menu is active and sometimes it isn't, so we enable
    // and disable objects through ScriptableObject data instead of directly. See the "MenuManager" under "Control Objects" 
    // in the hierarchy.
    Button button;
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Image>() != null)
            originalColor = GetComponent<Image>().color;
        flashingColor = Color.gray;
        newColor = new Color();
        if (GetComponent<Button>() != null)
            button = GetComponent<Button>();
        if (GetComponent<Image>() != null)
            image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (highlightButton)
        {
            newColor = Color.Lerp(originalColor, flashingColor, flashLerp);
            image.color = newColor;

            //Change the lerp value between the colors to produce a flashing effect
            if (flashLerp > 1)
            {
                upOrDown = -1;
            }             
            else if (flashLerp <= 0)
            {
                upOrDown = 1;
            }

            flashLerp += flashSpeed * upOrDown;
        }

        if (button != null)
            button.interactable = menuManagerObject.isActive; 
        highlightButton = menuManagerObject.isFlashing;

        if (gameObject.name == "Play")
            DebugUI.ShowText("Play is active: ", menuManagerObject.isActive.ToString());
    }

}
