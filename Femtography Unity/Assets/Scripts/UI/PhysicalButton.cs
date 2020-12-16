using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalButton : MonoBehaviour
{
    public Text UIText;
    public Color regularTextColor, highlightTextColor, regularButtonColor, highlightButtonColor;

    private Color startFadeButtonColor, endFadeButtonColor, startFadeTextColor, endFadeTextColor,
        currentFadingButtonColor, currentFadingTextColor;
    MaterialPropertyBlock shiftingColor;

    public float regularAlphaAmount, highlightAlphaAmount;

    private float startAlpha, endAlpha, currentAlpha;

    float fadeCounter = 0;

    public float fadeSpeed;

    bool canFadeIn = true, canFadeOut, isFadingIn;

    IEnumerator FadeInCoroutine, FadeOutCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        shiftingColor = new MaterialPropertyBlock();
        currentFadingButtonColor = regularButtonColor;// look to the Coroutine FadeUI to see why we set these here
        currentFadingTextColor = regularTextColor;
        currentAlpha = regularAlphaAmount;

        if (UIText != null)
            UIText.color = regularTextColor;
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
            if (UIText != null)
                UIText.color = currentFadingTextColor;

            fadeCounter += fadeSpeed;

            if (fadeCounter > 1)
            {
                yield break;
            } else
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
}
