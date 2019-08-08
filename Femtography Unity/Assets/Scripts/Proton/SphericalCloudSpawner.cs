using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalCloudSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float objectSize;
    private float x, y, z;
    private float creationTimer;
    public float creationTime, initialSpawnCount;

    // Start is called before the first frame update
    void Start()
    {
        creationTimer = 0;
        for (int i = 0; i < initialSpawnCount; i++)
        {
            Vector3 randomLocation = Random.insideUnitSphere * objectSize;
            Debug.Log(randomLocation.ToString());
            Quaternion randomRotation = Random.rotation;
            GameObject newObject = Instantiate(objectToSpawn, transform, true);
            newObject.GetComponent<RandomQuarkChooser>().ChooseRandomQuark();
            newObject.transform.position = transform.parent.position + randomLocation;
            newObject.transform.rotation = randomRotation; 
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (CheckTiming())
        {
            Vector3 randomLocation = Random.insideUnitSphere * objectSize;
            Debug.Log(randomLocation.ToString());
            Quaternion randomRotation = Random.rotation;
            GameObject newObject = Instantiate(objectToSpawn, transform, true);
            newObject.transform.position = transform.parent.position + randomLocation;
            newObject.transform.rotation = randomRotation;
        }    
    }

    private bool CheckTiming()
    {
        creationTimer++;
        if (creationTimer / 60 >= creationTime)
        {
            creationTimer = 0;
            return true;
        }
        else { return false; }


    }
}
