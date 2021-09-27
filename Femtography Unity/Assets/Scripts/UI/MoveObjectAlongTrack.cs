using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveObjectAlongTrack : MonoBehaviour
{
    // This class parents a tracker object to the VR controller and then moves the gameObject
    // along a line segment by projecting that tracker object's position onto the line.

    public GameObject trackerObjectPreProjection, lowBound, highBound;
    public XRSimpleInteractable xRSimpleInteractable;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetTrackerPosition()
    {
        trackerObjectPreProjection.transform.position = transform.position;
    }

    public void BeginTracking()
    {
        trackerObjectPreProjection.transform.SetParent(xRSimpleInteractable.selectingInteractor.gameObject.transform);
        StartCoroutine(TrackToInteractor());
    }
    public void StopTracking()
    {
        trackerObjectPreProjection.transform.SetParent(transform.parent);
        trackerObjectPreProjection.transform.position = transform.position;
        GetComponent<PhysicalSlider>().SetNewValue();
        StopAllCoroutines();
    }

    IEnumerator TrackToInteractor()
    {
        while (true)
        {
            GetComponent<PhysicalSlider>().UpdateSliderPosition(trackerObjectPreProjection.transform.position);
            yield return new WaitForEndOfFrame();
        }
    }
}
