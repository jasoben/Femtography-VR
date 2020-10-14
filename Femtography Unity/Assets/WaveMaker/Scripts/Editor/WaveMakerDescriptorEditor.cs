using UnityEngine;
using UnityEditor;

namespace WaveMaker
{
    [CustomEditor(typeof(WaveMakerDescriptor))]
    public class WaveMakerDescriptorEditor : Editor
    {
        int newWidth, newDepth;
        int oldWidth, oldDepth;
        WaveMakerDescriptor descriptor;
        string infomsg;

        private void Awake()
        {
            descriptor = (WaveMakerDescriptor)target;
            infomsg = "Resolution must be between 2 and " + descriptor.MaxResolution;
        }

        private void OnEnable()
        {
            descriptor = (WaveMakerDescriptor)target;
            newWidth = descriptor.ResolutionX;
            newDepth = descriptor.ResolutionZ;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Wave Maker - Descriptor", WaveMakerCommonEditorResources.GetAssetNameStyle());
            EditorGUILayout.Space();

            DrawResolutionGUI();
            EditorGUILayout.Space();
            DrawFixingGUI();
            EditorGUILayout.Space();

            EditorGUILayout.HelpBox(infomsg, MessageType.Info);

            serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

        private void DrawResolutionGUI()
        {
            EditorGUI.BeginChangeCheck();

            newWidth = EditorGUILayout.DelayedIntField("Width Resolution", newWidth);
            newDepth = EditorGUILayout.DelayedIntField("Depth Resolution", newDepth);
            if (EditorGUI.EndChangeCheck())
            {
                descriptor.SetResolution(newWidth, newDepth);
                newWidth = descriptor.ResolutionX;
                newDepth = descriptor.ResolutionZ;
            }
        }

        private void DrawFixingGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Fix All"))
                descriptor.SetAllFixStatus(true);

            if (GUILayout.Button("Unfix All"))
                descriptor.SetAllFixStatus(false);

            if (GUILayout.Button("Fix Borders"))
                descriptor.FixBorders();

            GUILayout.EndHorizontal();
        }
    }

    [CustomPreview(typeof(WaveMakerDescriptor))]
    public class WaveMakerDescriptorPreview : ObjectPreview
    {
        Texture2D _previewTexture;
        WaveMakerDescriptor descriptor;

        public override bool HasPreviewGUI()
        {
            return true;
        }

        private void CreateTexture()
        {
             descriptor = (WaveMakerDescriptor)target;

            _previewTexture = new Texture2D(descriptor.ResolutionX, descriptor.ResolutionZ, TextureFormat.RGBAHalf, false);
            _previewTexture.wrapMode = TextureWrapMode.Clamp;
        }

        private void UpdateTexture()
        {
            // Apply properties as colors
            for (int x = 0; x < descriptor.ResolutionX; x++)
                for (int z = 0; z < descriptor.ResolutionZ; z++)
                {
                    Color newColor = descriptor.IsFixed(x, z) ? descriptor.fixedColor : descriptor.defaultColor;
                    _previewTexture.SetPixel(x, z, newColor);
                }

            _previewTexture.Apply();
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            descriptor = (WaveMakerDescriptor)target;

            // Update texture data if resolution changed
            if (_previewTexture == null || descriptor.ResolutionX != _previewTexture.width || descriptor.ResolutionZ != _previewTexture.height)
                CreateTexture();

            UpdateTexture();

            GUI.DrawTexture(r, _previewTexture, ScaleMode.ScaleToFit, false, 1);
        }


    }


}
