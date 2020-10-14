using UnityEngine;
using UnityEditor;

namespace WaveMaker
{
    [CustomEditor(typeof(WaveMakerSurface))]
    public class WaveMakerSurfaceEditor : Editor
    {
        #region MEMBERS

        float _auxWidth, _auxDepth;

        WaveMakerSurface _waveMakerObj;
        Collider _waveMakerCollider;

        // Properties
        SerializedProperty substeps;
        SerializedProperty damping;
        SerializedProperty propagationSpeed;
        SerializedProperty verticalPushScale;
        SerializedProperty waveSmoothness;
        SerializedProperty horizontalPushScale;
        SerializedProperty collisionDetectionHeight;
        SerializedProperty interactorSpeedClamp;

        // Debug
        SerializedProperty showLogMessages;
        SerializedProperty drawGrid;

        bool showFixedSamples = false;

        // Paint Mode
        bool paintModeEnabled = false;
        static PaintMode paintMode = PaintMode.FIX;
        [MinAttribute(0)] static float pencilRadius = 1;
        static Color paintAreaColor = Color.red;

        int fixDetectionLayer = 0;
        
        Color32[] _fixedSampleColors;
        
        public enum PaintMode { NONE, FIX, UNFIX }

        Mesh _paintModeMesh;

        /// <summary>
        /// Material used for the paint mode. It contains a custom shader that draws colors on mesh
        /// </summary>
        Material MaterialPaintMode { get => _materialPaintMode; set => _materialPaintMode = value; }
        Material _materialPaintMode;

        bool _keyPressed = false;

        #endregion

        /***********************************************/

        #region METHODS

        private void OnEnable()
        {
            _keyPressed = false;

            _waveMakerObj = (WaveMakerSurface)target;
            _waveMakerCollider = _waveMakerObj.GetComponent<Collider>();
            _waveMakerObj.InitializeIfDescriptorChanged();

            _auxWidth = _waveMakerObj.Width;
            _auxDepth = _waveMakerObj.Depth;

            substeps = serializedObject.FindProperty("substeps");
            damping = serializedObject.FindProperty("damping");
            propagationSpeed = serializedObject.FindProperty("propagationSpeed");
            verticalPushScale = serializedObject.FindProperty("verticalPushScale");
            horizontalPushScale = serializedObject.FindProperty("horizontalPushScale");
            waveSmoothness = serializedObject.FindProperty("waveSmoothness");
            collisionDetectionHeight = serializedObject.FindProperty("collisionDetectionHeight");
            interactorSpeedClamp = serializedObject.FindProperty("interactorSpeedClamp");

            showLogMessages = serializedObject.FindProperty("showLogMessages");
            drawGrid = serializedObject.FindProperty("drawGrid");

            InitializePaintModeAssets();
        }

        private void InitializePaintModeAssets()
        {
            if (Application.isPlaying)
                return;

            if(_waveMakerObj.Descriptor != null)
            {
                if (_paintModeMesh != null)
                    DestroyImmediate(_paintModeMesh);

                _paintModeMesh = new Mesh();
                _paintModeMesh.vertices = _waveMakerObj.MeshFilter.sharedMesh.vertices;
                _paintModeMesh.triangles = _waveMakerObj.MeshFilter.sharedMesh.triangles;
                _paintModeMesh.uv = _waveMakerObj.MeshFilter.sharedMesh.uv;
                _paintModeMesh.normals = _waveMakerObj.MeshFilter.sharedMesh.normals;
                _paintModeMesh.tangents = _waveMakerObj.MeshFilter.sharedMesh.tangents;

                //TODO: Assign default material first. find a way to find a material and shader not by name
                if (MaterialPaintMode == null)
                {
                    Shader shader = Shader.Find("WaveMaker/WaveMakerFixedPreviewShader");

                    if (shader != null)
                        MaterialPaintMode = new Material(shader);
                    else
                    {
                        Debug.LogError("WaveMaker - 'WaveMakerFixedPreviewShader' not found. Please fix the name or reinstall WaveMaker");
                        MaterialPaintMode = new Material(Shader.Find("Diffuse"));
                    }
                }

                ReloadFixedSampleColors();
            }
        }

        private void OnDisable()
        {
            DisablePaintMode();
            UninitializePaintModeAssets();
        }
        
        private void UninitializePaintModeAssets()
        {
            DestroyImmediate(MaterialPaintMode);
            DestroyImmediate(_paintModeMesh);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            DrawDescriptorSelectionGUI();

            EditorGUILayout.Space();

            // Draw nothing more if no descriptor available
            if (_waveMakerObj.Descriptor == null)
                return;

            DrawSizeChangeGUI();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.LabelField("Parameters", WaveMakerCommonEditorResources.GetTitleStyle());

            EditorGUIUtility.labelWidth = 150;
            EditorGUILayout.PropertyField(substeps);
            EditorGUILayout.PropertyField(propagationSpeed);

            EditorGUIUtility.labelWidth = 0;
            EditorGUILayout.PropertyField(damping);
            EditorGUIUtility.labelWidth = 150;
                        
            EditorGUIUtility.labelWidth = 0;
            EditorGUILayout.PropertyField(waveSmoothness);
            EditorGUIUtility.labelWidth = 150;

            EditorGUILayout.PropertyField(verticalPushScale);
            EditorGUILayout.PropertyField(horizontalPushScale);
            EditorGUILayout.PropertyField(interactorSpeedClamp);
            EditorGUIUtility.labelWidth = 0;

            DrawStabilityWarningGUI();

            EditorGUILayout.EndVertical();
            
            if (!Application.isPlaying)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                    DrawFixingGUI();
                    DrawAutoFixGUI();
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();

                DrawPaintModeGUI();
            }

            DrawDebugGUI();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawStabilityWarningGUI()
        {
            // Check Stability condition
            if (!_waveMakerObj.CheckStabilityCondition())
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Warning! The surface can be unstable with these settings. Reduce descriptor resolution or reduce propagation speed. The last option is to increase substeps but it will be take more time to compute.", MessageType.Warning);
            }
        }

        private void DrawSizeRatioWarningGUI()
        {
            // Check size ratio
            if (_waveMakerObj.SampleSizeX / _waveMakerObj.SampleSizeZ > 2 || _waveMakerObj.SampleSizeZ / _waveMakerObj.SampleSizeX > 2)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Warning! Cells must be approximately square. Activate the grid view to find a proper combination of resolution and size.", MessageType.Warning);
            }
                
        }

        private void DrawDescriptorSelectionGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Descriptor", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(70));
            EditorGUI.BeginChangeCheck();
            _waveMakerObj.Descriptor = (WaveMakerDescriptor)EditorGUILayout.ObjectField(_waveMakerObj.Descriptor, typeof(WaveMakerDescriptor), false, GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck())
            {
                if (_waveMakerObj.Descriptor != null)
                {
                    UninitializePaintModeAssets();
                    _waveMakerObj.Initialize();
                    InitializePaintModeAssets();
                }
                else
                {
                    _waveMakerObj.Uninitialize();
                    UninitializePaintModeAssets();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSizeChangeGUI()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Mesh Size (Local space)", WaveMakerCommonEditorResources.GetTitleStyle());

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Width", WaveMakerCommonEditorResources.GetCenteredStyle(), GUILayout.MinWidth(20));
            _auxWidth = EditorGUILayout.DelayedFloatField(_auxWidth, GUILayout.ExpandWidth(true), GUILayout.MinWidth(20));
            EditorGUILayout.LabelField("Depth", WaveMakerCommonEditorResources.GetCenteredStyle(), GUILayout.MinWidth(20));
            _auxDepth = EditorGUILayout.DelayedFloatField(_auxDepth, GUILayout.ExpandWidth(true), GUILayout.MinWidth(20));
            EditorGUILayout.Space();

            if (EditorGUI.EndChangeCheck())
            {
                _waveMakerObj.Width = _auxWidth;
                _waveMakerObj.Depth = _auxDepth;
            }

            EditorGUILayout.EndHorizontal();
            DrawSizeRatioWarningGUI();
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        private void DrawFixingGUI()
        {
            EditorGUILayout.LabelField("Fixed Cells", WaveMakerCommonEditorResources.GetTitleStyle());

            EditorGUILayout.Space();

            showFixedSamples = EditorGUILayout.Toggle("Show Fixed Cells", showFixedSamples);

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Fix All"))
            {
                _waveMakerObj.Descriptor.SetAllFixStatus(true);
                ReloadFixedSampleColors();
            }
            if (GUILayout.Button("Unix All"))
            {
                _waveMakerObj.Descriptor.SetAllFixStatus(false);
                ReloadFixedSampleColors();
            }
            if (GUILayout.Button("Fix Borders"))
            {
                _waveMakerObj.Descriptor.FixBorders();
                ReloadFixedSampleColors();
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private void DrawAutoFixGUI()
        {
            // Automatic fixing
            EditorGUILayout.LabelField("Automatic detection", WaveMakerCommonEditorResources.GetTitleStyle());
            EditorGUILayout.PropertyField(collisionDetectionHeight, new GUIContent("Distance Detected"));
            fixDetectionLayer = EditorGUILayout.LayerField(new GUIContent("Layer Detected"), fixDetectionLayer);
            if (GUILayout.Button("Fix Collisions with Cells"))
            {
                _waveMakerObj.FixCollisions(fixDetectionLayer);
                ReloadFixedSampleColors();
            }
            EditorGUILayout.Space();
        }
        
        private void DrawPaintModeGUI()
        {
            EditorGUILayout.LabelField("Property Paint Mode", WaveMakerCommonEditorResources.GetTitleStyle());
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            paintModeEnabled = EditorGUILayout.Toggle(paintModeEnabled, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(15));
            EditorGUILayout.LabelField("Enabled", GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck())
            {
                if (paintModeEnabled)
                    EnablePaintMode();
                else
                    DisablePaintMode();
            }

            if (paintModeEnabled)
            {
                EditorGUILayout.BeginVertical(WaveMakerCommonEditorResources.GetBoxStyle());
                paintMode = (PaintMode)EditorGUILayout.EnumPopup(new GUIContent("Mode"), paintMode);
                pencilRadius = EditorGUILayout.FloatField(new GUIContent("Pencil Radius"), pencilRadius);
                paintAreaColor = EditorGUILayout.ColorField(new GUIContent("Pencil Color"), paintAreaColor);
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();
        }

        private void DrawDebugGUI()
        {
            EditorGUILayout.LabelField("Debug Tools", WaveMakerCommonEditorResources.GetTitleStyle());
            EditorGUILayout.PropertyField(showLogMessages);
            EditorGUILayout.PropertyField(drawGrid);
        }
               
        private void OnSceneGUI()
        {
            if (_waveMakerObj == null)
                return;

            if (!Application.isPlaying && paintModeEnabled)
            {
                if (Event.current.type == EventType.KeyDown)
                    _keyPressed = true;
                else if (Event.current.type == EventType.KeyUp)
                    _keyPressed = false;

                //TODO: Maybe do just once
                // Don't allow selecting another object
                if (!_keyPressed)
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

                // Get ray from the camera at the mouse position
                var mousePos = Event.current.mousePosition;

                mousePos.y = Camera.current.pixelHeight - mousePos.y;
                var ray = Camera.current.ScreenPointToRay(mousePos);

                // Hit with this wave maker object
                if (_waveMakerCollider.Raycast(ray, out RaycastHit hit, 500))
                {
                    var upVector = _waveMakerCollider.transform.TransformVector(Vector3.up);
                    Handles.color = paintAreaColor;
                    Handles.DrawWireDisc(hit.point, upVector, pencilRadius);
                    SceneView.RepaintAll();

                    if (!_keyPressed && (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag))
                        PaintArea(hit.point);
                }
            }

            if (paintModeEnabled || showFixedSamples)
                Graphics.DrawMesh(_paintModeMesh, _waveMakerObj.transform.localToWorldMatrix, _materialPaintMode, 0);
                        
            if (_waveMakerObj.drawGrid)
            {
                int width = _waveMakerObj.ResolutionXGhost;
                int depth = _waveMakerObj.ResolutionZGhost;
                
                for (int z = 0; z < _waveMakerObj.ResolutionZGhost; z++)
                {
                    Vector3 right = _waveMakerObj.GetPositionFromSample(0, z, true, true, true);
                    for (int x = 0; x < _waveMakerObj.ResolutionXGhost; x++)
                    {
                        Vector3 origin = right;
                        
                        if (x < _waveMakerObj.ResolutionXGhost-1)
                        {
                            right = _waveMakerObj.GetPositionFromSample(x + 1, z, true, true, true);
                            Debug.DrawLine(origin, right, Color.red);
                        }

                        if (z < _waveMakerObj.ResolutionZGhost-1)
                        {
                            Vector3 top = _waveMakerObj.GetPositionFromSample(x, z + 1, true, true, true);
                            Debug.DrawLine(origin, top, Color.red);
                        }
                    }
                }   
            }
        }

        private void ReloadFixedSampleColors()
        {
            var desc = _waveMakerObj.Descriptor;
            _fixedSampleColors = new Color32[desc.ResolutionX * desc.ResolutionZ];
            for (int x = 0; x < desc.ResolutionX; x++)
                for (int z = 0; z < desc.ResolutionZ; z++)
                    _fixedSampleColors[desc.ResolutionX * z + x] = desc.IsFixed(x, z) ? desc.fixedColor : desc.defaultColor;
        
            if (_paintModeMesh != null)
                _paintModeMesh.colors32 = _fixedSampleColors;
        }

        public void EnablePaintMode()
        {
            if (Application.isPlaying)
                return;

            paintModeEnabled = true;
            Tools.current = Tool.None;

            ReloadFixedSampleColors();
        }

        public void DisablePaintMode()
        {
            if (Application.isPlaying)
                return;

            paintModeEnabled = false;
            Tools.current = Tool.Move;

            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }

        /// <param name="hitPoint_ws">A position in worldspace where to paint</param>
        public void PaintArea(Vector3 hitPoint_ws)
        {
            if (Application.isPlaying || !_waveMakerObj.Descriptor.IsInitialized)
                return;

            Vector3 hitPoint_ls = _waveMakerObj.transform.worldToLocalMatrix.MultiplyPoint(hitPoint_ws);
            _waveMakerObj.GetNearestSample(hitPoint_ls.x, hitPoint_ls.z, out int centerX, out int centerZ);

            // Calculate number of cells from radius
            int cellRadius = Mathf.Max(1, Mathf.FloorToInt(pencilRadius / _waveMakerObj.SampleSizeX));
            bool colorsModified = false;

            for (int x = centerX - cellRadius; x <= centerX + cellRadius; x++)
            {
                if (x < 0 || x >= _waveMakerObj.Descriptor.ResolutionX)
                    continue;

                for (int z = centerZ - cellRadius; z <= centerZ + cellRadius; z++)
                {
                    if (z < 0 || z >= _waveMakerObj.Descriptor.ResolutionZ)
                        continue;

                    Vector3 pos_ls = _waveMakerObj.GetPositionFromSample(x, z);

                    // Ignore out of the circle area
                    if ((hitPoint_ls - pos_ls).magnitude > pencilRadius)
                        continue;

                    switch (paintMode)
                    {
                        case PaintMode.FIX:
                            _waveMakerObj.Descriptor.SetFixed(x, z, true);
                            _fixedSampleColors[_waveMakerObj.Descriptor.ResolutionX * z + x] = _waveMakerObj.Descriptor.fixedColor;
                            colorsModified = true;
                            break;

                        case PaintMode.UNFIX:
                            _waveMakerObj.Descriptor.SetFixed(x, z, false);
                            _fixedSampleColors[_waveMakerObj.Descriptor.ResolutionX * z + x] = _waveMakerObj.Descriptor.defaultColor;
                            colorsModified = true;
                            break;

                        default:
                            break;
                    }
                }
            }

            if (colorsModified)
                _paintModeMesh.colors32 = _fixedSampleColors;
        }

    }
    #endregion
}
