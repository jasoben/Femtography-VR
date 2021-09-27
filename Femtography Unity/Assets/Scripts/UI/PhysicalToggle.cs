using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalToggle : PhysicalButton
{
    bool isToggled;
    public bool IsToggled { get { return isToggled; } set { isToggled = value; } }
    GameObject checkedBox;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        checkedBox = transform.Find("Checked").gameObject;
        isToggled = GetComponent<UIHelper>().menuManagerObject.isOn;
        StartCoroutine(ScaleToggle());
    }

    public new void OnSelectEnd()
    {
        base.OnSelectEnd();
        SetToggle();
    }
    public void SetToggle()
    {
        isToggled = !isToggled;
        StartCoroutine(ScaleToggle());
    }

    public void SetToggle(bool isOn)
    {
        isToggled = isOn;
        StartCoroutine(ScaleToggle());
    }

    IEnumerator ScaleToggle()
    {
        Vector3 startSize = default;
        Vector3 endSize = default;
        float scaleAmount = 0;
        if (!isToggled)
        {
            startSize = Vector3.one;
            endSize = Vector3.zero;
        }
        else if (isToggled)
        {
            startSize = Vector3.zero;
            endSize = Vector3.one;
        }

        while (true)
        {
            checkedBox.transform.localScale = Vector3.Lerp(startSize, endSize, scaleAmount);
            scaleAmount += fadeSpeed * 20;

            if (scaleAmount > 1)
            {
                yield break;
            } else
                yield return new WaitForEndOfFrame();
        }
    }
}
