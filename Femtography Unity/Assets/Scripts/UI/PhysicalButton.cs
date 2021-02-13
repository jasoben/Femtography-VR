using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhysicalButton : MonoBehaviour
{
    public Color regularTextColor, highlightTextColor, regularButtonColor, highlightButtonColor, disabledColor;

    private Color startFadeButtonColor, endFadeButtonColor, startFadeTextColor, endFadeTextColor,
        currentFadingButtonColor, currentFadingTextColor, unToggledColor;
    MaterialPropertyBlock materialPropertyBlock;

    public float regularAlphaAmount, highlightAlphaAmount;

    private float startAlpha, endAlpha, currentAlpha, fadeCounter = 0;

    public float fadeSpeed, clickDistance;

    bool canFadeIn = true, canFadeOut, isFadingIn, clicked;

    IEnumerator FadeInCoroutine, FadeOutCoroutine, clickCoroutine;

    protected Vector3 originalUITextOrImagePosition, clickedUITextOrImagePosition; // move it slightly when clicked

    // Start is called before the first frame update
    protected void Start()
    {
        originalUITextOrImagePosition = Vector3.zero;
        clickedUITextOrImagePosition = new Vector3(0, 0, .01f);

        materialPropertyBlock = new MaterialPropertyBlock();
        currentFadingButtonColor = regularButtonColor;// look to the Coroutine FadeUI to see why we set these here
        currentFadingTextColor = regularTextColor;
        currentAlpha = regularAlphaAmount;

        unToggledColor = regularButtonColor;

        originalUITextOrImagePosition = transform.localPosition;
        clickedUITextOrImagePosition = originalUITextOrImagePosition + clickedUITextOrImagePosition;

        StartCoroutine(FadeUI());
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
                if (GetType() == typeof(PhysicalToggle))
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

    protected void OnMouseEnter()
    {
        if (GetComponent<UIHelper>() == null ||
            GetComponent<UIHelper>().menuManagerObject.isActive)
        {
            isFadingIn = true;
            FadeInCoroutine = FadeUI();
            if (FadeOutCoroutine != null)
                StopCoroutine(FadeOutCoroutine);
            StartCoroutine(FadeInCoroutine);
        }
    }
    protected void OnMouseExit()
    {
        if (GetComponent<UIHelper>() == null ||
            GetComponent<UIHelper>().menuManagerObject.isActive)
        {
            isFadingIn = false;
            FadeOutCoroutine = FadeUI();
            if (FadeInCoroutine != null)
                StopCoroutine(FadeInCoroutine);
            StartCoroutine(FadeOutCoroutine);
        } else
            EnableDisable();
    }

    protected void OnMouseDown()
    {
        if (GetComponent<UIWiggler>() != null)
            GetComponent<UIWiggler>().WigglerPaused = true;
        clicked = true;
        if (clickCoroutine != null)
            StopCoroutine(clickCoroutine);
        clickCoroutine = ClickUI();
        StartCoroutine(clickCoroutine);
    }
    protected void OnMouseUp()
    {
        if (GetComponent<UIWiggler>() != null)
            GetComponent<UIWiggler>().WigglerPaused = false;
        clicked = false;
        if (clickCoroutine != null)
            StopCoroutine(clickCoroutine);
        clickCoroutine = ClickUI();
        StartCoroutine(clickCoroutine);
    }


    public void EnableDisable()
    {
        StopAllCoroutines();
        StartCoroutine(SetEnabledOrDisabled());
    }

    IEnumerator SetEnabledOrDisabled()
    {
        float fadeAmount = 0, startAlpha, endAlpha, currentAlpha;
        Color startColor, endColor, currentColor, startTextColor, endTextColor, currentTextColor;
        bool isEnabled = GetComponent<UIHelper>().menuManagerObject.isActive;

        if (isEnabled)
        {
            startColor = GetComponent<Renderer>().material.GetColor("Color_");
            startColor = disabledColor;
            endColor = regularButtonColor;
            startTextColor = disabledColor + new Color(0, 0, 0, .1f);
            endTextColor = regularTextColor;
            startAlpha = regularAlphaAmount;
            endAlpha = .2f;
        }
        else
        {
            startColor = highlightButtonColor;// We want it to fade from it's "active" color when disabling
            endColor = disabledColor;
            startTextColor = regularTextColor;
            endTextColor = disabledColor + new Color(0, 0, 0, .1f);
            startAlpha = .2f;
            endAlpha = regularAlphaAmount;
        }

        while (true)
        {
            currentColor = Color.Lerp(startColor, endColor, fadeAmount);
            currentTextColor = Color.Lerp(startTextColor, endTextColor, fadeAmount);
            currentAlpha = Mathf.Lerp(startAlpha, endAlpha, fadeAmount);

            materialPropertyBlock.SetColor("Color_", currentColor);
            materialPropertyBlock.SetColor("GlowColor", currentColor);
            materialPropertyBlock.SetFloat("Alpha_", currentAlpha);

            GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);

            if (GetComponentInChildren<TextMeshPro>() != null)
            {
                foreach(TextMeshPro textMeshPro in GetComponentsInChildren<TextMeshPro>())
                {
                    textMeshPro.color = currentTextColor;
                }
            }    

            if (GetComponentInChildren<SpriteRenderer>() != null)
                GetComponentInChildren<SpriteRenderer>().color = currentTextColor;

            fadeAmount += .03f;

            if (fadeAmount > 1)
                yield break;
            else
                yield return new WaitForEndOfFrame();
        }
    }
}
