using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateNewTextBar : MonoBehaviour
{
    public GameObject newTextBar;
    public float growthRate;
    // Start is called before the first frame update
    void Start()
    {
        newTextBar.transform.localScale = new Vector3(0, .143f, 1);
        newTextBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateBar()
    {
        newTextBar.SetActive(true);
        StartCoroutine(activateBar());
    }

    private IEnumerator activateBar()
    {
        bool growing = true;
        while(true)
        {
            float newX = newTextBar.transform.localScale.x;
            newTextBar.transform.localScale = new Vector3(newX + growthRate, .143f, 1);
            Debug.Log(growing.ToString());
                Debug.Log(newX.ToString());
            if (newX > 1 && growing)
            {
                growing = false;
                growthRate = -growthRate;
            }
            else if (newX < 0)
            {
                newTextBar.transform.localScale = new Vector3(0, .143f, 1);
                growthRate = -growthRate;
                newTextBar.SetActive(false);
                Debug.Log("Done");
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
