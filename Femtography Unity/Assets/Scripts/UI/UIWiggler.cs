using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWiggler : MonoBehaviour
{
    // This wiggles UI around a bit to preserve the appearance of the liquid display

    public Vector3 wiggleSpan, wiggleAmount, wiggleRange;
    Vector3 startPosition;
    public float wiggleCoefficient;
    // Start is called before the first frame update
    void Start()
    {
        wiggleRange = wiggleRange * wiggleCoefficient;
        wiggleAmount = wiggleAmount * wiggleCoefficient;
        wiggleSpan = wiggleSpan * wiggleCoefficient;

        Vector3 randomStartPosition = new Vector3(Random.Range(-wiggleRange.x, wiggleRange.x), Random.Range(-wiggleRange.y, wiggleRange.y),
            Random.Range(-wiggleRange.z, wiggleRange.z));
        transform.position = transform.position + randomStartPosition;
        startPosition = randomStartPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(wiggleAmount.x, wiggleAmount.y, wiggleAmount.z);
    }
}
