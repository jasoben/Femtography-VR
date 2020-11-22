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
        if (Input.GetKeyDown(KeyCode.Alpha8))
            currentText++;
        if (currentText == explanationTexts.Count)
            currentText = 0;
        uiText.text = explanationTexts[currentText].explanation;
    }

    public void NextText()
    {
        currentText++;
    }
    public void ChooseText(int whichNumber)
    {
        currentText = whichNumber;
    }
}

public class ParsedExplanationText
{
    public string explanation;
    public string title;
}
