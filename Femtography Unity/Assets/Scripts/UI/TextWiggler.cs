using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWiggler : MonoBehaviour
{
    // This wiggles UI text around a bit to preserve the appearance of the liquid display

    float wiggleAmountX = .000005f, wiggleAmountY = .000005f, totalWiggleSpanY = .0005f, totalWiggleSpanX = .0007f, currentPositionX, currentPositionY;
    // Start is called before the first frame update
    void Start()
    {
        currentPositionX = Random.Range(-.01f, .01f);
        currentPositionY = Random.Range(-.01f, .01f);
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
            if (wiggleAmountX > 0)
                wiggleAmountX = Random.Range(.000003f, .000007f);
        }
        if (Mathf.Abs(currentPositionY) > totalWiggleSpanY)
        {
            wiggleAmountY = -wiggleAmountY;
            if (wiggleAmountY > 0)
                wiggleAmountY = Random.Range(.000003f, .000007f);
        }
    }
}
