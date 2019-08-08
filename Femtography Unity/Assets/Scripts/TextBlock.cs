using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class TextBlock : ScriptableObject
{
    [TextArea]
    public string text;
}
