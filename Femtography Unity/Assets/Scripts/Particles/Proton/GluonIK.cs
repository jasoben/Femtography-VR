using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GluonIK : MonoBehaviour
{
    [SerializeField] List<GameObject> redQuarkGluonBones;
    [SerializeField] List<GameObject> blueQuarkGluonBones;
    [SerializeField] List<GameObject> greenQuarkGluonBones;

    [SerializeField] GameObject blueQuark, redQuark, greenQuark;
    [SerializeField] GameObject center;

    [SerializeField] float stretch = 2f;
    [SerializeField] float speed = .01f;
    private float[] moveAmounts = new float[5];

    // Start is called before the first frame update
    void OnEnable()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < redQuarkGluonBones.Count; i++)
        {
            float lerpAmount = (i + 1) / ((float)redQuarkGluonBones.Count);
            Debug.Log(i + " : " + lerpAmount);

            redQuarkGluonBones[i].transform.position = Vector3.Lerp(
                center.transform.position, redQuark.transform.position, lerpAmount);
            redQuarkGluonBones[i].transform.rotation = Quaternion.LookRotation(Vector3.up,
                redQuarkGluonBones[i].transform.position - redQuark.transform.position);
        }
    }

}
