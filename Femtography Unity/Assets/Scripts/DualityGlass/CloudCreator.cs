using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCreator : MonoBehaviour
{
    public GameObject cloud;
    public float spawnTime;
    public float spawnDistance;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CloudCreatorCoroutine());
    }

    IEnumerator CloudCreatorCoroutine()
    {
        GameObject newCloud = Instantiate(cloud, transform);
        newCloud.transform.localPosition = new Vector3(Random.Range(-spawnDistance, spawnDistance), Random.Range(-spawnDistance, spawnDistance), Random.Range(-spawnDistance, spawnDistance));
        float spawnSize = Random.Range(1f, 2f);
        newCloud.transform.localScale = new Vector3(spawnSize, spawnSize, spawnSize);
        Invoke("StartCoroutine", spawnTime);
        yield break;
    }

    void StartCoroutine()
    {
        StartCoroutine(CloudCreatorCoroutine());
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
