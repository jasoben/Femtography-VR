using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuadTextWriter : MonoBehaviour
{

    public List<TextLines> textLines;
    private int currentParagraph;
    public GameObject[] textObjects;

    public float perWordTiming = .5f;

    private bool fadingOut;

    // Start is called before the first frame update
    void Start()
    {
        currentParagraph = 0;
        StartCoroutine("DisplayText");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            CueNextText();
        }
    }

    public void CueNextText(int nextParagraphNumber = 0)
    {
        if (nextParagraphNumber == 0)
        {
            if (currentParagraph + 1 > textLines.Count)
            {
            } else { currentParagraph = currentParagraph + 1; }
        } else
        {
            currentParagraph = nextParagraphNumber;
        }

        StopAllCoroutines();
        StartCoroutine("DisplayText");
    }

    private IEnumerator DisplayText()
    {
        int currentLine = 0;
        string currentText = "currentText";
        while(true)
        {
            for (int i = 0; i < textObjects.Length; i++)
            {
                textObjects[i].GetComponent<TextMeshPro>().text = currentText = textLines[currentParagraph].textLine[currentLine];
            }

            currentLine++;

            if (currentLine + 1 > textLines[currentParagraph].textLine.Count)
            {
                yield break;
            } else
            {
                yield return new WaitForSeconds(CountWords(currentText) * perWordTiming);
                for (int i = 0; i < textObjects.Length; i++)
                {
                    StartCoroutine("FadeBetweenText", textObjects[i].GetComponent<TextMeshPro>());
                }
                yield return new WaitWhile(() => fadingOut);
            }
        }

    }

    private IEnumerator FadeBetweenText(TextMeshPro textObject)
    {
        byte i = 250;
        fadingOut = true;
        bool goingDown = true;

        while(true)
        {
            textObject.color = new Color32(255, 255, 255, i);
            if (goingDown)
                i = (byte)(i - 2);
            else
                i = (byte)(i + 2);
            if (i < 4)
            {
                i = 8;
                goingDown = false;
                fadingOut = false;
            }
            else if (i > 250)
            {
                yield break;
            } else { yield return new WaitForEndOfFrame(); }
        }
    }

    private int CountWords(string thisString)
    {
        int index = 0, wordCount = 0;
        while (index < thisString.Length && char.IsWhiteSpace(thisString[index]))
        {
            index++;
        }

        while (index < thisString.Length)
        {
            while (index < thisString.Length && !char.IsWhiteSpace(thisString[index]))
            {
                index++;                
            }

            wordCount++;

            while (index < thisString.Length && char.IsWhiteSpace(thisString[index]))
            {
                index++;
            }

        }

        return wordCount;
    }
}
