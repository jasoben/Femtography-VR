using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSwitcher : MonoBehaviour
{

    public TextBlock currentText;

    void Start()
    {
    }

    void Update()
    {
        GetComponent<TextMeshPro>().text = currentText.text;
    }

    public void LoadText(TextBlock thisText)
    {
        currentText = thisText;
    }
}
