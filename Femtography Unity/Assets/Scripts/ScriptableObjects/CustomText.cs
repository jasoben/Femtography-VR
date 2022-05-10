using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Custom Text")]
public class CustomText : ScriptableObject
{
    public List<string> titles;

    [TextArea]
    public List<string> customTexts;

}
