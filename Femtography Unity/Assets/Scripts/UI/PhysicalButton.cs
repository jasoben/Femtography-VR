﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhysicalButton : MonoBehaviour
{
    public Color regularTextColor, highlightTextColor, regularButtonColor, highlightButtonColor, disabledColor;

    private Color startFadeButtonColor, endFadeButtonColor, startFadeTextColor, endFadeTextColor,
        currentFadingButtonColor, currentFadingTextColor, unToggledColor;
    MaterialPropertyBlock materialPropertyBlock;

    public bool isToggle, isScalable;

    public float regularAlphaAmount, highlightAlphaAmount, scaleSpeed;

    private float startAlpha, endAlpha, currentAlpha, fadeCounter = 0, normalScaleSpeed;

    public float fadeSpeed, clickDistance;

    bool canFadeIn = true, canFadeOut, isFadingIn, clicked, isToggled;
    
    public bool IsToggled { get { return isToggled; } set { isToggled = value; } }

    IEnumerator FadeInCoroutine, FadeOutCoroutine, clickCoroutine, scalingCoroutine;

    public Vector3 originalUITextOrImagePosition, clickedUITextOrImagePosition, originalUIHotKeyPosition, clickUIHotKeyPosition;

    Vector3 endScale, normalScale;

    GameObject checkedBox;

    // Start is called before the first frame update
    void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        currentFadingButtonColor = regularButtonColor;// look to the Coroutine FadeUI to see why we set these here
        currentFadingTextColor = regularTextColor;
        currentAlpha = regularAlphaAmount;

        unToggledColor = regularButtonColor;

        originalUITextOrImagePosition = transform.localPosition;
        clickedUITextOrImagePosition = originalUITextOrImagePosition + clickedUITextOrImagePosition;

        normalScale = transform.localScale;
        normalScaleSpeed = scaleSpeed;

        if (GetComponent<UIHelper>() != null)
            GetComponent<UIHelper>().SetScale();// set scale values for this component before shrinking button
        if (isScalable)
            transform.localScale = Vector3.zero;

        StartCoroutine(FadeUI());

        if (isToggle)
        {
            checkedBox = transform.Find("Checked").gameObject;
            isToggled = GetComponent<UIHelper>().menuManagerObject.isOn;
            StartCoroutine(ScaleToggle());
        }

    }

    // Update is called once per frame
    void Update()
    {
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

            scalingCoroutine = UIScaler();
            StartCoroutine(scalingCoroutine);
        }    

        else if (!isEnlarging)
        {
            endScale = Vector3.zero; 
            if (scalingCoroutine != null)
                StopCoroutine(scalingCoroutine);

            scalingCoroutine = UIScaler();
            StartCoroutine(scalingCoroutine);
        }    
    }

    IEnumerator UIScaler() // shrink or grow UI element
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

    IEnumerator FadeUI()
    {
        fadeCounter = 0;
        while (true)
        {
            if (isFadingIn)
            {
                startFadeButtonColor = currentFadingButtonColor;
                endFadeButtonColor = highlightButtonColor;

                startFadeTextColor = currentFadingTextColor;
                if (isToggle)
                {
                    endFadeTextColor = highlightButtonColor; // if it is a toggle the text should be the same color
                    // as the button, instead of a high contrast color against the button
                }
                else 
                    endFadeTextColor = highlightTextColor;

                startAlpha = currentAlpha;
                endAlpha = highlightAlphaAmount;
            }
            else if (!isFadingIn)
            {
                startFadeButtonColor = currentFadingButtonColor;
                endFadeButtonColor = regularButtonColor;

                startFadeTextColor = currentFadingTextColor;
                endFadeTextColor = regularTextColor;

                startAlpha = currentAlpha;
                endAlpha = regularAlphaAmount;
            }

            currentFadingTextColor = Color.Lerp(startFadeTextColor, endFadeTextColor, fadeCounter);
            currentFadingButtonColor = Color.Lerp(startFadeButtonColor, endFadeButtonColor, fadeCounter);
            currentAlpha = Mathf.Lerp(startAlpha, endAlpha, fadeCounter);

            materialPropertyBlock.SetColor("Color_", currentFadingButtonColor);
            materialPropertyBlock.SetColor("GlowColor", currentFadingButtonColor);
            materialPropertyBlock.SetFloat("Alpha_", currentAlpha);

            GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
            if (GetComponentInChildren<TextMeshPro>() != null)
            {
                foreach(TextMeshPro textMeshPro in GetComponentsInChildren<TextMeshPro>())
                {
                    textMeshPro.color = currentFadingTextColor;
                }
            }    
            if (GetComponentInChildren<SpriteRenderer>() != null)
                GetComponentInChildren<SpriteRenderer>().color = currentFadingTextColor;

            fadeCounter += fadeSpeed;

            if (fadeCounter > 1)
            {
                yield break;
            } else
                yield return new WaitForEndOfFrame();
        }
    }

    public void SetAlpha() // used to trigger alpha reset from other methods
    {
        materialPropertyBlock.SetFloat("Alpha_", regularAlphaAmount);
        GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
    }
    
    IEnumerator ClickUI()
    {
        float clickAmount = 0;
        while (true)
        {
            if (clicked)
            {
                transform.position = Vector3.Lerp(transform.position, transform.parent.TransformPoint(clickedUITextOrImagePosition), clickAmount);// since the button is a
            } else
            {
                transform.position = Vector3.Lerp(transform.position, transform.parent.TransformPoint(originalUITextOrImagePosition), clickAmount);// since the button is a
            }
            // child of the text object, we only need to move the text and the underline, which is not a child
            clickAmount += .05f;

            if (clickAmount > 1)
            {
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnMouseEnter()
    {
        if (GetComponent<UIHelper>().menuManagerObject.isActive)
        {
            isFadingIn = true;
            FadeInCoroutine = FadeUI();
            if (FadeOutCoroutine != null)
                StopCoroutine(FadeOutCoroutine);
            StartCoroutine(FadeInCoroutine);
        }
    }
    private void OnMouseExit()
    {
        if (GetComponent<UIHelper>().menuManagerObject.isActive)
        {
            isFadingIn = false;
            FadeOutCoroutine = FadeUI();
            if (FadeInCoroutine != null)
                StopCoroutine(FadeInCoroutine);
            StartCoroutine(FadeOutCoroutine);
        }
    }

    private void OnMouseDown()
    {
        GetComponent<UIWiggler>().WigglerPaused = true;
        clicked = true;
        if (clickCoroutine != null)
            StopCoroutine(clickCoroutine);
        clickCoroutine = ClickUI();
        StartCoroutine(clickCoroutine);
    }
    private void OnMouseUp()
    {
        GetComponent<UIWiggler>().WigglerPaused = false;
        clicked = false;
        if (clickCoroutine != null)
            StopCoroutine(clickCoroutine);
        clickCoroutine = ClickUI();
        StartCoroutine(clickCoroutine);

        if (isToggle)
        {
            SetToggle();
        }
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

    public void EnableDisable()
    {
        StopAllCoroutines();
        StartCoroutine(SetEnabledOrDisabled());
    }
    IEnumerator SetEnabledOrDisabled()
    {
        float fadeAmount = 0;
        Color startColor, endColor, currentColor;
        bool isEnabled = GetComponent<UIHelper>().menuManagerObject.isActive;

        if (isEnabled)
        {
            startColor = disabledColor;
            endColor = regularButtonColor;
        }
        else
        {
            startColor = regularButtonColor;
            endColor = disabledColor;
        }

        while (true)
        {
            currentColor = Color.Lerp(startColor, endColor, fadeAmount);
            materialPropertyBlock.SetColor("Color_", currentColor);
            materialPropertyBlock.SetColor("GlowColor", currentColor);
            foreach(TextMeshPro textMeshPro in GetComponentsInChildren<TextMeshPro>())
            {
                textMeshPro.color = currentColor;
            }
            GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
            fadeAmount += .05f;

            if (fadeAmount > 1)
                yield break;
            else
                yield return new WaitForEndOfFrame();
        }
    }
}
