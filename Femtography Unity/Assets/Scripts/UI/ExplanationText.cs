using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplanationText : MonoBehaviour
{
    public List<ParsedExplanationText> explanationTexts;

    Text uiText;

    int currentText = 0;
    // Start is called before the first frame update
    void Start()
    {
        explanationTexts = XmlParser.Read<ParsedExplanationText>("assets/XML/explanationText.xml");

        uiText = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        if (currentText == explanationTexts.Count)
            currentText = 0;
        uiText.text = explanationTexts[currentText].explanation;
    }

    public void NextText()
    {
        currentText++;
        ApplyTextColors();
    }
    public void ChooseTextNumber(int whichNumber)
    {
        currentText = whichNumber;
        ApplyTextColors();
    }
    public void ChooseTextTitle(string textTitle)
    {
        foreach(ParsedExplanationText thisExplanation in explanationTexts)
        {
            if (thisExplanation.title == textTitle)
                currentText = explanationTexts.IndexOf(thisExplanation);
        }
        ApplyTextColors();
    }
    void ApplyTextColors()
    {
        explanationTexts[currentText].explanation = explanationTexts[currentText].explanation.Replace("electron", "<color=orange>electron</color>");
        explanationTexts[currentText].explanation = explanationTexts[currentText].explanation.Replace("photon", "<color=yellow>photon</color>");
        explanationTexts[currentText].explanation = explanationTexts[currentText].explanation.Replace("proton", "<color=cyan>proton</color>");
    }
}

public class ParsedExplanationText
{
    public string explanation;
    public string title;
}
