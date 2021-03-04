using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScreenPortionController : MonoBehaviour
{
    public float ratioToActivate;// This is the screen ratio at which we activate or de-activate mouselook
    float screenHeight, mousePositionY;
    public GlobalBool menuOpen, mouseLook;
    public UnityEvent cursorOverMenu, cursorOutsideMenu;
    // Start is called before the first frame update
    void Start()
    {
        screenHeight = Camera.main.pixelHeight;
    }

    // Update is called once per frame
    void Update()
    {
        mousePositionY = Input.mousePosition.y;
        if (Input.mousePosition.y < screenHeight / ratioToActivate && menuOpen.boolValue && mouseLook.boolValue)
            cursorOverMenu.Invoke();
        else if (Input.mousePosition.y > screenHeight / ratioToActivate && menuOpen.boolValue && mouseLook.boolValue)
            cursorOutsideMenu.Invoke();
    }
}
