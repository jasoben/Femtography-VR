using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWiggler : MonoBehaviour
{
    // This wiggles UI around a bit to preserve the appearance of the liquid display

    float wiggleAmountX = .00001f, wiggleAmountY = .00001f, totalWiggleSpanY = .005f, totalWiggleSpanX = .007f, currentPositionX, currentPositionY;
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
                wiggleAmountX = Random.Range(.000005f, .00001f);
        }
        if (Mathf.Abs(currentPositionY) > totalWiggleSpanY)
        {
            wiggleAmountY = -wiggleAmountY;
            if (wiggleAmountY > 0)
                wiggleAmountY = Random.Range(.000005f, .00001f);
        }
    }
}
