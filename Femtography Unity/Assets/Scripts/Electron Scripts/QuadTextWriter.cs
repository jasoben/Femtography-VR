using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuadTextWriter : MonoBehaviour
{

    public List<TextLines> textLines;
    private int currentParagraph;
    public GameObject[] textObjects;

    public float perWordTiming;

    // Start is called before the first frame update
    void Start()
    {
        currentParagraph = 0;
        StartCoroutine("DisplayText");
        perWordTiming = .5f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CueNextText(int nextParagraphNumber = 0)
    {
        if (nextParagraphNumber == 0)
        {
            currentParagraph = currentParagraph + 1;
        } else
        {
            currentParagraph = nextParagraphNumber;
        }

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
            }
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
