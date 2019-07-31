using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCreator : MonoBehaviour
{
    public GameObject sphereToGrow;
    public float sphereSize;
    private float x, y, z;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CreateSphere");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CreateSphere()
    {
        while (true)
        {
            float randomSphereSize = Random.Range(0, sphereSize);
            int sign = Random.Range(0, 100);
            bool positiveOrNegative;
            if (sign > 50)
                positiveOrNegative = true;
            else
                positiveOrNegative = false;

            int axisChooser = Random.Range(0, 2);
            switch (axisChooser)
            {
                case 0:
                    x = Random.Range(-randomSphereSize, randomSphereSize);
                    y = Random.Range(-1 * Mathf.Sqrt(Mathf.Pow(randomSphereSize,2) - Mathf.Pow(x,2)), Mathf.Sqrt(Mathf.Pow(randomSphereSize,2) - Mathf.Pow(x,2)));
                    z = Mathf.Sqrt(Mathf.Pow(randomSphereSize, 2) - Mathf.Pow(x, 2) - Mathf.Pow(y, 2));
                    if (positiveOrNegative)
                        z = z * 1;
                    else
                        z = -z;
                    break;

                case 1:
                    x = Random.Range(-randomSphereSize, randomSphereSize);
                    z = Random.Range(-1 * Mathf.Sqrt(Mathf.Pow(randomSphereSize,2) - Mathf.Pow(x,2)), Mathf.Sqrt(Mathf.Pow(randomSphereSize,2) - Mathf.Pow(x,2)));
                    y = Mathf.Sqrt(Mathf.Pow(randomSphereSize, 2) - Mathf.Pow(x, 2) - Mathf.Pow(z, 2));
                    if (positiveOrNegative)
                        y = y * 1;
                    else
                        y = -y;
                     break;

                case 2:
                    z = Random.Range(-randomSphereSize, randomSphereSize);
                    y = Random.Range(-1 * Mathf.Sqrt(Mathf.Pow(randomSphereSize,2) - Mathf.Pow(z,2)), Mathf.Sqrt(Mathf.Pow(randomSphereSize,2) - Mathf.Pow(z,2)));
                    x = Mathf.Sqrt(Mathf.Pow(randomSphereSize, 2) - Mathf.Pow(z, 2) - Mathf.Pow(y, 2));
                    if (positiveOrNegative)
                        x = x * 1;
                    else
                        x = -x;
                     break;

            }

            float adjustedX = x + transform.position.x;
            float adjustedY = y + transform.position.y;
            float adjustedZ = z + transform.position.z;
            GameObject newSphere = Instantiate(sphereToGrow, new Vector3(adjustedX, adjustedY, adjustedZ), Quaternion.identity);
            float maxValue = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y), Mathf.Abs(z));
            newSphere.GetComponent<GrowSphere>().maxSphereSize = (sphereSize - maxValue) / 2;
            newSphere.GetComponent<GrowSphere>().containerSphereSize = sphereSize;
            
            yield return new WaitForSeconds(.02f);

        }
    }
}
