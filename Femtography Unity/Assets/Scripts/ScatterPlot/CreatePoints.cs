using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FlexFramework.Excel;

public class CreatePoints : MonoBehaviour
{
    public GameObject[] pointTypes;
    private bool[] availableAxis;
    private AvailableAxis[] toggleIsInThisAxis;
    private int lastCheckedtoggle;
    public Document ThisData { get; set; }
    private List<Vector3> pointLocationsFirstData;
    private List<Vector3> pointLocationsSecondData;
    private List<GameObject> firstPoints;
    private List<GameObject> seconPoints;
    public ParseData thisParser;
    
    // Start is called before the first frame update
    void Start()
    {
        availableAxis = new bool[3];
        for (int i = 0; i < 3; i++)
        {
            availableAxis[i] = true;
        }
        toggleIsInThisAxis = new AvailableAxis[4];

        for (int i = 0; i < 4; i++)
        {
            toggleIsInThisAxis[i] = AvailableAxis.none;
        }
        pointLocationsFirstData = new List<Vector3>();
        pointLocationsSecondData = new List<Vector3>();
        firstPoints = new List<GameObject>();
        seconPoints = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateVectors()
    {
        for (int i = 0; i < ThisData.Count - 1; i++)
        {
            pointLocationsFirstData.Add(new Vector3(0,0,0));
            firstPoints.Add(Instantiate(pointTypes[0], new Vector3(0, 0, 0), Quaternion.Euler(0, 90, 0)));
            seconPoints.Add(Instantiate(pointTypes[1], new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 90)));
            pointLocationsSecondData.Add(new Vector3(0,0,0));
        }


    }

    public void CreateOrDestoryPoints(int toggle)
    {
        AvailableAxis thisAvailableAxis = CheckAvailable();
        toggleIsInThisAxis[toggle] = thisAvailableAxis;
        //switch (thisAvailableAxis)
        //{
        //    case AvailableAxis.xAxis:
        //        thisParser.toggles[toggle].GetComponentInChildren<Text>().color = Color.red;
        //        return;
        //    case AvailableAxis.yAxis:
        //        thisParser.toggles[toggle].GetComponentInChildren<Text>().color = Color.green;
        //        return;
        //    case AvailableAxis.zAxis:
        //        thisParser.toggles[toggle].GetComponentInChildren<Text>().color = Color.blue;
        //        return;

        //}
        AssignValues((int)thisAvailableAxis, toggle);
        UpdatePoints();



        
    }

    private AvailableAxis CheckAvailable()
    {
        int thisAvailableAxis = lastCheckedtoggle;
        for(int i = 0; i<3; i++)
        {
            if (availableAxis[i])
            {
                thisAvailableAxis = i;
                availableAxis[i] = false;
                lastCheckedtoggle = i;
                break;
            }
            else
            {
                thisAvailableAxis = lastCheckedtoggle;
            }
        }
        return (AvailableAxis)thisAvailableAxis;
    }
    private void TurnOffToggle(int theLastCheckedtoggle)
    {
        toggleIsInThisAxis[theLastCheckedtoggle] = AvailableAxis.none;
    }
    private void AssignValues(int thisAvailableAxis, int thisToggle)
    {

        for (int i = 0; i < pointLocationsFirstData.Count; i++)
        {
            switch(thisAvailableAxis)
            {
                case 0:
                    pointLocationsFirstData[i] = new Vector3(ValueConverter.Convert<float>(ThisData[i + 1][thisToggle].ToString()), pointLocationsFirstData[i].y, pointLocationsFirstData[i].z);
                    pointLocationsSecondData[i] = new Vector3(ValueConverter.Convert<float>(ThisData[i + 1][thisToggle + 4].ToString()), pointLocationsSecondData[i].y, pointLocationsSecondData[i].z);
                    break;
                case 1:
                    pointLocationsFirstData[i] = new Vector3(pointLocationsFirstData[i].x, ValueConverter.Convert<float>(ThisData[i + 1][thisToggle].ToString()), pointLocationsFirstData[i].z);
                    pointLocationsSecondData[i] = new Vector3(pointLocationsSecondData[i].x, ValueConverter.Convert<float>(ThisData[i + 1][thisToggle + 4].ToString()), pointLocationsSecondData[i].z);
                    break;
                case 2:
                    pointLocationsFirstData[i] = new Vector3(pointLocationsFirstData[i].x, pointLocationsFirstData[i].y, ValueConverter.Convert<float>(ThisData[i + 1][thisToggle].ToString()));
                    pointLocationsSecondData[i] = new Vector3(pointLocationsSecondData[i].x, pointLocationsSecondData[i].y, ValueConverter.Convert<float>(ThisData[i + 1][thisToggle + 4].ToString()));
                    break;
           }
        }


    }

    private void UpdatePoints()
    {

        for (int i = 0; i < firstPoints.Count; i++)
        {
            firstPoints[i].transform.position = pointLocationsFirstData[i]; 
            seconPoints[i].transform.position = pointLocationsSecondData[i]; 
        }

    }

    private void DestroyPoints(int thistoggle)
    {

    }
}

public enum AvailableAxis
{
    xAxis,
    yAxis,
    zAxis,
    none
}
