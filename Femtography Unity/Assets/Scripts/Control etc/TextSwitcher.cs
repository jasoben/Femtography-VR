using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSwitcher : MonoBehaviour
{

    public TextBlock currentText;
    public ActivateNewTextBar thisNewTextBar;
    public GameObject newTextBar;

    void Start()
    {
    }

    void Update()
    {
        GetComponent<TextMeshPro>().text = currentText.text;
    }

    public void LoadText(TextBlock thisText)
    {
        thisNewTextBar.ActivateBar();
        currentText = thisText;
    }
}
