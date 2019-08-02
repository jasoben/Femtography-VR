using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlexFramework.Excel;
using OxOD;
using UnityEngine.UI;

public class ParseData : MonoBehaviour
{
    public string fileLocation;
    public Document loadedData;
    public GameObject[] toggles;
    public FileDialog thisFileSelector;
    public CreatePoints pointCreator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SelectFile()
    {
        yield return StartCoroutine(thisFileSelector.Open());

        if (thisFileSelector.result != null)
        {
            fileLocation = thisFileSelector.result;
            loadedData = Document.LoadAt(fileLocation);
            AssignToggleNames();
            pointCreator.ThisData = loadedData;
        }

        else
        {
            Debug.Log("Dialog canceled");
        }
    }

    public void OnSelectFileButtonClick()
    {
        StartCoroutine(SelectFile());
    }

    private void AssignToggleNames()
    {
        for (int i = 0; i < loadedData[0].Count; i++)
        {
            toggles[i].GetComponentInChildren<Text>().text = loadedData[0][i];
        }
    }


}
