using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIdentityText : MonoBehaviour
{
    public GlobalBool showText;
    private GameObject console;
    private MeshRenderer meshRenderer;

    bool isGrowing;
    Vector3 setScale;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        setScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrowing != showText.boolValue)// if the value changes
        {
            isGrowing = showText.boolValue;
            StopAllCoroutines();
            StartCoroutine(ShrinkOrGrow());
        }

    }

    IEnumerator ShrinkOrGrow()
    {
        float scaleValue = 0;

        Vector3 startScale, endScale;
        while (true)
        {
            if (isGrowing)
            {
                meshRenderer.enabled = true;
                startScale = Vector3.zero;
                endScale = setScale;
            }
            else
            {
                startScale = setScale;
                endScale = Vector3.zero;
            }

            transform.localScale = Vector3.Lerp(startScale, endScale, scaleValue);

            scaleValue += .04f;

            if (scaleValue > 1)
            {
                if (!isGrowing)
                    meshRenderer.enabled = false;
                yield break;
            } else
                yield return new WaitForEndOfFrame();
        }
    }

}
