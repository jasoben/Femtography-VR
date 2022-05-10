using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplanationText : MonoBehaviour
{
    public List<ParsedExplanationText> explanationTexts;
    public CustomText explainTexts;

    Text uiText;

    int currentText = 0;
    // Start is called before the first frame update
    void Start()
    {
        explanationTexts = new List<ParsedExplanationText>();
        for (int i = 0; i < explainTexts.customTexts.Count; i++)
        {
            ParsedExplanationText parsedExplanationText = new ParsedExplanationText();
            parsedExplanationText.explanation = explainTexts.customTexts[i];
            parsedExplanationText.title = explainTexts.titles[i];
            explanationTexts.Add(parsedExplanationText);
        }

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
        explanationTexts[currentText].explanation = explanationTexts[currentText].explanation.Replace("momentum transfer", "<color=red>momentum transfer</color>");
        explanationTexts[currentText].explanation = explanationTexts[currentText].explanation.Replace("speed", "<color=yellow>speed</color>");
        explanationTexts[currentText].explanation = explanationTexts[currentText].explanation.Replace("teleport", "<color=green>teleport</color>");
        explanationTexts[currentText].explanation = explanationTexts[currentText].explanation.Replace("Follow Particle", "<color=orange>Follow Particle</color>");
    }
}

public class ParsedExplanationText
{
    public string explanation;
    public string title;
}
