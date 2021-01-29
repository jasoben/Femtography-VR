using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaler : MonoBehaviour
{
    float scaleSpeed = .01f, normalScaleSpeed;
    Vector3 endScale, normalScale;
    IEnumerator scalingCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        normalScale = transform.localScale;
        normalScaleSpeed = scaleSpeed;

        if (GetComponent<UIHelper>() != null)
            GetComponent<UIHelper>().SetScale();// set scale values for this component before shrinking button
        transform.localScale = Vector3.zero;
    }
    public void EnglargeOrShrink(bool isEnlarging)
    {
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
            endScale = Vector3.zero; 
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
