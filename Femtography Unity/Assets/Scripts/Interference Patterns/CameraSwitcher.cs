using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public List<GameObject> waveSets;
    List<GameObject> cameraPositions = new List<GameObject>();
    List<GameObject> centerObjects = new List<GameObject>();
    int currentPosition = 0, finalPosition;

    public GameObject cubeScreen, sphereScreen, bothScreen;
    GameObject newLineObject;
    float distanceBetweenCubeAndSphere;

    Matrix4x4 cubeScreenMatrix, sphereScreenMatrix, bothScreenMatrix;

    LineRenderer cubeLineRenderer, sphereLineRenderer, bothRedLineRenderer, bothGreenLineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        cubeLineRenderer = cubeScreen.GetComponent<LineRenderer>();
        sphereLineRenderer = sphereScreen.GetComponent<LineRenderer>();
        bothRedLineRenderer = bothScreen.GetComponent<LineRenderer>();
        bothGreenLineRenderer = bothScreen.transform.GetChild(0).gameObject.GetComponent<LineRenderer>();

        cubeScreenMatrix = cubeScreen.transform.worldToLocalMatrix;
        sphereScreenMatrix = sphereScreen.transform.worldToLocalMatrix;
        bothScreenMatrix = bothScreen.transform.localToWorldMatrix;

        bothScreen.SetActive(false);

        foreach (GameObject gameObject in waveSets)
        {
            cameraPositions.Add(gameObject.transform.Find("CameraStartPosition").gameObject);
            centerObjects.Add(gameObject.transform.Find("CenterObject").gameObject);
        }

        distanceBetweenCubeAndSphere = Vector3.Distance(cubeScreen.transform.position, sphereScreen.transform.position);

        currentPosition = 0;
        finalPosition = waveSets.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            transform.position = cameraPositions[currentPosition].transform.position;
            GetComponent<RotateAroundObject>().centerObject = centerObjects[currentPosition];
            currentPosition++;
        }

        // In the position where we can see both the cube and the sphere, we want to see the two lines
        // superimposed 
        if (currentPosition > finalPosition)
        {
            if (!bothScreen.activeSelf)
            {
                bothScreen.SetActive(true);
                bothRedLineRenderer.positionCount = cubeLineRenderer.positionCount;
                bothGreenLineRenderer.positionCount = sphereLineRenderer.positionCount;
            }
            
            currentPosition = 0;
        }
        
        if (bothScreen.activeSelf)
        {
            Vector3 cubeLinePositionTransformedViaMatrix = new Vector3();
            Vector3 sphereLinePositionTransformedViaMatrix = new Vector3();
            for (int i = 0; i < cubeLineRenderer.positionCount; i++)
            {
                cubeLinePositionTransformedViaMatrix = bothScreenMatrix.MultiplyPoint3x4(
                    cubeScreenMatrix.MultiplyPoint3x4(cubeLineRenderer.GetPosition(i)));
                // switch from cubeScreen world to local to bothScreen local to world

                bothRedLineRenderer.SetPosition(i, cubeLinePositionTransformedViaMatrix);
            }
            for (int i = 0; i < sphereLineRenderer.positionCount; i++)
            {
                sphereLinePositionTransformedViaMatrix = bothScreenMatrix.MultiplyPoint3x4(
                    sphereScreenMatrix.MultiplyPoint3x4(sphereLineRenderer.GetPosition(i)));
                // switch from cubeScreen world to local to bothScreen local to world

                bothGreenLineRenderer.SetPosition(i, sphereLinePositionTransformedViaMatrix);
            }
        }

        if (currentPosition == 1)
        {
            bothScreen.SetActive(false);
        }
    }
}
