using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherPhysicalButton : MonoBehaviour
{
    public GameObject UITextOrImage, UIHotKey;
    public Color regularTextColor, highlightTextColor, regularButtonColor, highlightButtonColor;

    private Color startFadeButtonColor, endFadeButtonColor, startFadeTextColor, endFadeTextColor,
        currentFadingButtonColor, currentFadingTextColor;
    MaterialPropertyBlock shiftingColor;

    public float regularAlphaAmount, highlightAlphaAmount;

    private float startAlpha, endAlpha, currentAlpha;

    float fadeCounter = 0;

    public float fadeSpeed, clickDistance;

    bool canFadeIn = true, canFadeOut, isFadingIn, clicked;

    IEnumerator FadeInCoroutine, FadeOutCoroutine, clickCoroutine;

    public Vector3 originalUITextOrImagePosition, clickedUITextOrImagePosition, originalUIHotKeyPosition, clickUIHotKeyPosition;

    // Start is called before the first frame update
    void Start()
    {
        shiftingColor = new MaterialPropertyBlock();
        currentFadingButtonColor = regularButtonColor;// look to the Coroutine FadeUI to see why we set these here
        currentFadingTextColor = regularTextColor;
        currentAlpha = regularAlphaAmount;

        if (UITextOrImage != null)
        {
            if (UITextOrImage.GetComponent<Text>() != null)
            {
                UITextOrImage.GetComponent<Text>().color = regularTextColor;
            }
            if (UITextOrImage.GetComponent<Image>() != null)
            {
                UITextOrImage.GetComponent<Image>().color = regularTextColor;
            }
            originalUITextOrImagePosition = UITextOrImage.transform.localPosition;
            if (clickedUITextOrImagePosition == Vector3.zero)
                clickedUITextOrImagePosition = originalUITextOrImagePosition + new Vector3(0, 0, 3f);
            else
                clickedUITextOrImagePosition = originalUITextOrImagePosition + clickedUITextOrImagePosition;
        }
        if (UIHotKey != null)
        {
            if (UIHotKey.GetComponent<Text>() != null)
                UIHotKey.GetComponent<Text>().color = regularTextColor;
            originalUIHotKeyPosition = UIHotKey.transform.localPosition;
            clickUIHotKeyPosition = originalUIHotKeyPosition + new Vector3(0, 0, 3f);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
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

            shiftingColor.SetColor("Color_", currentFadingButtonColor);
            shiftingColor.SetColor("GlowColor", currentFadingButtonColor);
            shiftingColor.SetFloat("Alpha_", currentAlpha);

            GetComponent<Renderer>().SetPropertyBlock(shiftingColor);
            if (UITextOrImage != null)
            {
                if (UITextOrImage.GetComponent<Text>() != null)
                    UITextOrImage.GetComponent<Text>().color = currentFadingTextColor;
                if (UITextOrImage.GetComponent<Image>() != null)
                    UITextOrImage.GetComponent<Image>().color = currentFadingTextColor;
            }
            if (UIHotKey != null)
                UIHotKey.GetComponent<Text>().color = currentFadingTextColor;

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
        shiftingColor.SetFloat("Alpha_", regularAlphaAmount);
        GetComponent<Renderer>().SetPropertyBlock(shiftingColor);
    }
    
    IEnumerator ClickUI()
    {
        float clickAmount = 0;
        while (true)
        {
            if (clicked)
            {
                UITextOrImage.transform.position = Vector3.Lerp(UITextOrImage.transform.position, UITextOrImage.transform.parent.TransformPoint(clickedUITextOrImagePosition), clickAmount);// since the button is a
                if (UIHotKey != null)
                    UIHotKey.transform.position = Vector3.Lerp(UIHotKey.transform.position, UIHotKey.transform.parent.TransformPoint(clickUIHotKeyPosition), clickAmount);
            } else
            {
                UITextOrImage.transform.position = Vector3.Lerp(UITextOrImage.transform.position, UITextOrImage.transform.parent.TransformPoint(originalUITextOrImagePosition), clickAmount);// since the button is a
                if (UIHotKey != null)
                    UIHotKey.transform.position = Vector3.Lerp(UIHotKey.transform.position, UIHotKey.transform.parent.TransformPoint(originalUIHotKeyPosition), clickAmount);
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
        isFadingIn = true;
        FadeInCoroutine = FadeUI();
        if (FadeOutCoroutine != null)
            StopCoroutine(FadeOutCoroutine);
        StartCoroutine(FadeInCoroutine);
    }
    private void OnMouseExit()
    {
        isFadingIn = false;
        FadeOutCoroutine = FadeUI();
        if (FadeInCoroutine != null)
            StopCoroutine(FadeInCoroutine);
        StartCoroutine(FadeOutCoroutine);
    }

    private void OnMouseDown()
    {
        clicked = true;
        if (clickCoroutine != null)
            StopCoroutine(clickCoroutine);
        clickCoroutine = ClickUI();
        StartCoroutine(clickCoroutine);
    }
    private void OnMouseUp()
    {
        clicked = false;
        if (clickCoroutine != null)
            StopCoroutine(clickCoroutine);
        clickCoroutine = ClickUI();
        StartCoroutine(clickCoroutine);
    }
}
