using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWiggler : MonoBehaviour
{
    // This wiggles UI text around a bit to preserve the appearance of the liquid display

    float wiggleAmountX = .005f, wiggleAmountY = .005f, totalWiggleSpanY = .5f, totalWiggleSpanX = .7f, 
    currentPositionX = 0, currentPositionY = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(wiggleAmountX, wiggleAmountX, 0);
        currentPositionX += wiggleAmountX;
        currentPositionY += wiggleAmountY;
        
        if (Mathf.Abs(currentPositionX) > totalWiggleSpanX)
        {
            wiggleAmountX = -wiggleAmountX;
        }
        if (Mathf.Abs(currentPositionY) > totalWiggleSpanY)
        {
            wiggleAmountY = -wiggleAmountY;
        }
    }
}
