using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShrinkField : MonoBehaviour
{
    public float shrinkSpeed;

    public GameObject growRoom, shrinkText;
    GameObject player;

    public Text superScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<MeshRenderer>().enabled = true;
        player = other.gameObject;
        player.SetActive(false);
        growRoom.SetActive(true);
        shrinkText.SetActive(true);

        StartCoroutine(ShrinkCharacter());
    }

    IEnumerator ShrinkCharacter()
    {
        float shrinkage = 0;

        Vector3 cameraStart = growRoom.transform.TransformPoint(new Vector3(0, .5f, 0));
        Vector3 cameraEnd = growRoom.transform.TransformPoint(new Vector3(0, -.5f, 0));

        float startSizeSuperScript = 0;
        float endSizeSuperScript = -10;

        while (true)
        {
            shrinkage += shrinkSpeed;

            Camera.main.transform.position = Vector3.Lerp(cameraStart, cameraEnd, shrinkage);

            superScript.text = Mathf.Lerp(startSizeSuperScript, endSizeSuperScript, shrinkage).ToString("#.##");

            if (shrinkage > 1)
                yield break;

            yield return new WaitForEndOfFrame();
        }

    }
}
