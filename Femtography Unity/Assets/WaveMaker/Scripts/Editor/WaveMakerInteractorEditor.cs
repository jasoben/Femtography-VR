using UnityEngine;
using UnityEditor;
using System;

namespace WaveMaker
{
    [CustomEditor(typeof(WaveMakerInteractor))]
    public class WaveMakerInteractorEditor : Editor
    {
        GUIStyle _warningStyle = new GUIStyle();
        GUIStyle _boxStyle = new GUIStyle();
        WaveMakerInteractor _waveMakerInteractorObj;
        Collider _collider;
        Rigidbody _rb;
        
        private void OnEnable()
        {
            _waveMakerInteractorObj = (WaveMakerInteractor)target;
            _collider = _waveMakerInteractorObj.GetComponent<Collider>();
            _rb = _waveMakerInteractorObj.GetComponent<Rigidbody>();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            if (_collider == null)
                EditorGUILayout.HelpBox("Assign a Collider component to this interactor", MessageType.Warning);

            if (_rb == null)
                EditorGUILayout.HelpBox("Assign a RigidBody component to this interactor", MessageType.Warning);

            EditorGUILayout.HelpBox("To soften the effect of speed of this interactor activate and increase dampening. Show velocities in the scene view to check the effect.", MessageType.Info);

            DrawDefaultInspector();
        }
    }
}
