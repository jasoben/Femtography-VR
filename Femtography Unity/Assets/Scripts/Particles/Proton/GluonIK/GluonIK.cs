using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluonIK : MonoBehaviour
{
    List<GameObject> redQuarkGluonBones;
    List<GameObject> blueQuarkGluonBones;
    List<GameObject> greenQuarkGluonBones;

    [SerializeField] GameObject blueQuark, redQuark, greenQuark;
    [SerializeField] GameObject center;
    [SerializeField] GameObject topBoneRed, topBoneBlue, topBoneGreen;

    [SerializeField] float stretch = 2f;
    [SerializeField] float speed = .01f;

    private float[] moveAmounts = new float[5];

    // Start is called before the first frame update
    void Start()
    {
        FillBoneList(topBoneRed, out redQuarkGluonBones);
        FillBoneList(topBoneBlue, out blueQuarkGluonBones);
        FillBoneList(topBoneGreen, out greenQuarkGluonBones);
    }

    void FillBoneList(GameObject topBoneObject, out List<GameObject> gluonBoneList)
    {
        gluonBoneList = new List<GameObject>();
        GameObject nextBone = topBoneObject;
        while (nextBone.transform.childCount > 0)
        {
            nextBone = nextBone.transform.GetChild(0).gameObject;
            gluonBoneList.Add(nextBone);
        }

    }

    // Update is called once per frame
    void Update()
    {
        MoveBones(topBoneRed, redQuarkGluonBones, redQuark);
        MoveBones(topBoneBlue, blueQuarkGluonBones, blueQuark);
        MoveBones(topBoneGreen, greenQuarkGluonBones, greenQuark);
    }

    void MoveBones(GameObject topBoneObject, List<GameObject> bonesList, GameObject quark)
    {
        for (int i = 0; i < bonesList.Count; i++)
        {
            float lerpAmount = (i + 1) / ((float)bonesList.Count);

            bonesList[i].transform.position = Vector3.Lerp(
                center.transform.position, quark.transform.position, lerpAmount);
            bonesList[i].transform.rotation = Quaternion.LookRotation(topBoneObject.transform.forward,
                quark.transform.position - bonesList[i].transform.position);
        }
    }

}
