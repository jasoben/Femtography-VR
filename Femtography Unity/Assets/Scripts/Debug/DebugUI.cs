using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class DebugUI
{
    static GameObject DebugCanvas;
    static Dictionary<string, GameObject> textAndTextObjects = new Dictionary<string, GameObject>();

    public static void ShowText(string textDescription, string textToShow)
    {
        if (DebugCanvas == null)
            DebugCanvas = GameObject.Find("DebugCanvas");
        if (!textAndTextObjects.ContainsKey(textDescription))
        {
            GameObject newTextObject = GameObject.Instantiate(DebugCanvas.transform.GetChild(0).gameObject);
            newTextObject.transform.SetParent(DebugCanvas.transform);
            newTextObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-100,20 + (textAndTextObjects.Count + 1) * 15,0);
            textAndTextObjects.Add(textDescription, newTextObject);
        }
        else if (textAndTextObjects.ContainsKey(textDescription))
        {
            textAndTextObjects[textDescription].GetComponent<Text>().text = textDescription + " " + textToShow;
        }
    }
}
