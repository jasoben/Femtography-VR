using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityStandardAssets.Utility;

public class TestAttachPivot : MonoBehaviour
{
    public GameObject pivotObject;
    public XRSimpleInteractable xRSimpleInteractable;
    float initialY, initialZ;
    GameObject interactorObject;
    FollowTarget followTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        followTarget = GetComponent<FollowTarget>();
    }

    public void BeginTracking()
    {
        interactorObject = xRSimpleInteractable.selectingInteractor.gameObject;
        pivotObject.transform.position = interactorObject.transform.position;
        Vector3 targetOffset = transform.position - pivotObject.transform.position ;
        followTarget.offset = targetOffset;
        initialY = pivotObject.transform.position.y;
        initialZ = pivotObject.transform.position.z;
        StartCoroutine(TrackToInteractor());
    }
    public void StopTracking()
    {
        StopAllCoroutines();
        interactorObject = null;
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator TrackToInteractor()
    {
        while (true)
        {
            pivotObject.transform.position = new Vector3(interactorObject.transform.position.x, initialY, initialZ);
            yield return new WaitForEndOfFrame();
        }
    }
}
