using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventListenerSorter))]
public class EventListenerSorterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EventListenerSorter eventListenerSorter = (EventListenerSorter)target;

        if (GUILayout.Button("Sort Event Listeners"))
        {
            eventListenerSorter.Sort();
        }
    }
}
