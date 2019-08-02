using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FlexFramework.Excel;

public class CreatePoints : MonoBehaviour
{
    public GameObject[] pointTypes;
    private int availableNumber;
    private int lastCheckedColumn;
    public Document ThisData { get; set; }
    private List<Vector3> pointLocations;
    public ParseData thisParser;
    
    // Start is called before the first frame update
    void Start()
    {
        availableNumber = 0;
        pointLocations = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateVectors()
    {
        for (int i = 0; i < ThisData.Count; i++)
        {
            pointLocations.Add(new Vector3(0,0,0));
        }
    }

    public void CreateOrDestoryPoints(int column)
    {
        if (thisParser.toggles[column].GetComponent<Toggle>().isOn)
        {
            switch (availableNumber)
            {
                case 0:
                    thisParser.toggles[column].GetComponentInChildren<Text>().color = Color.red;
                    return;

            }

        }

        
    }

    private void AssignValues()
    {

    }
}
