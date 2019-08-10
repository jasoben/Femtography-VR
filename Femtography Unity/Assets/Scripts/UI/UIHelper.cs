using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
    public GameObject helperText;

    // Start is called before the first frame update
    void Start()
    {
        helperText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowHelperText()
    {
        helperText.SetActive(true);
    }
    public void HiderHelperText()
    {
        helperText.SetActive(false);
    }
}
