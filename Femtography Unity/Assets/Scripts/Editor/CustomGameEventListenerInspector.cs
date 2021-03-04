using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomGameEventListenerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        GameEventListener thisTarget = (GameEventListener)target;


    }
}
