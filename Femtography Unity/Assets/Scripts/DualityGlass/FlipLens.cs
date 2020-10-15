using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipLens : MonoBehaviour
{
    bool coroutineRunning;
    public GameObject otherLens;
    public bool lensVisible;

    // Start is called before the first frame update
    void Start()
    {
        if (!lensVisible)
        {
            SetVisible(gameObject, false);
        }
    }
    void SetVisible(GameObject thisObject, bool visibleOrNot)
    {
        thisObject.GetComponent<MeshRenderer>().enabled = visibleOrNot;
        thisObject.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = visibleOrNot;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !coroutineRunning)
        {
            TrackOnOrOff(false);
            StartCoroutine(FlipTheLens());
        }
    }

    IEnumerator FlipTheLens()
    {
        coroutineRunning = true;
        int i = 0;
        bool flipVisible = lensVisible;
        while (true)
        {
            transform.Rotate(new Vector3(0, 0, 1));
            i++;
            if (i > 89 && lensVisible && flipVisible)
            {
                lensVisible = !lensVisible;
                SetVisible(gameObject, lensVisible);

                otherLens.GetComponent<FlipLens>().lensVisible = !otherLens.GetComponent<FlipLens>().lensVisible;
                SetVisible(otherLens, otherLens.GetComponent<FlipLens>().lensVisible);
            }
            if (i > 179)
            {
                coroutineRunning = false;
                TrackOnOrOff(true);
                yield break;
            } else
                yield return new WaitForEndOfFrame();
        }
    }

    void TrackOnOrOff(bool onOrOff)
    {
        GetComponent<TrackObjectToCursor>().enabled = onOrOff;
        otherLens.GetComponent<TrackObjectToCursor>().enabled = onOrOff;
    }
}
