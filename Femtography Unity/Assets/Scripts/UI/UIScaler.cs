using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaler : MonoBehaviour
{
    float scaleSpeed = .01f, normalScaleSpeed;
    Vector3 endScale, normalScale, shrunkenScale;
    IEnumerator scalingCoroutine;
    public bool startShrunken = true;
    // Start is called before the first frame update
    void Start()
    {
        shrunkenScale = new Vector3(.0001f, .0001f, .0001f);// we need this to be non-zero b/c otherwise it 
            // messes up local positions in scaled objects
        normalScale = transform.localScale;
        normalScaleSpeed = scaleSpeed;

        if (GetComponent<UIHelper>() != null)
            GetComponent<UIHelper>().SetScale();// set scale values for this component before shrinking button
        if (GetComponentInChildren<UIHelper>() != null)
            GetComponentInChildren<UIHelper>().SetScale();// set scale values for this component before shrinking button
        // (We use "InChildren" for cases when this references a Slider or Toggle, which are slightly different from
        // a regular button)
        if (startShrunken)
            transform.localScale = shrunkenScale;
    }
    public void EnglargeOrShrink(bool isEnlarging)
    {
        Debug.Log(isEnlarging);
        float randomScaleSpeedModifier = Random.Range(-.005f, .005f);
        scaleSpeed = normalScaleSpeed + randomScaleSpeedModifier;
        if (isEnlarging)
        {
            endScale = normalScale;
            if (scalingCoroutine != null)
                StopCoroutine(scalingCoroutine);

            scalingCoroutine = ScaleUI();
            StartCoroutine(scalingCoroutine);
        }    

        else if (!isEnlarging)
        {
            endScale = shrunkenScale; 
            if (scalingCoroutine != null)
                StopCoroutine(scalingCoroutine);

            scalingCoroutine = ScaleUI();
            StartCoroutine(scalingCoroutine);
        }    
    }
    IEnumerator ScaleUI() // shrink or grow UI element
    {
        float scaleFactor = 0;
        while (true)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, endScale, scaleFactor);
            scaleFactor += scaleSpeed;

            if (scaleFactor > 1)
                yield break;

            else
                yield return new WaitForEndOfFrame();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
