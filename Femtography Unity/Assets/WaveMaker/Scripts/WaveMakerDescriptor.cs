using System;
using UnityEngine;
using Unity.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WaveMaker
{
    [HelpURL("http://wavemaker.lidia-martinez.com/")]
    [CreateAssetMenu(fileName = "WaveMakerDescriptor", menuName = "WaveMaker Descriptor", order = 10)]
    public class WaveMakerDescriptor : ScriptableObject
    {
#if UNITY_2018 || (MATHEMATICS_INSTALLED && BURST_INSTALLED && COLLECTIONS_INSTALLED)

        public event EventHandler OnResolutionChanged;
        public event EventHandler OnFixedGridChanged;
        public event EventHandler OnDestroyed;

        public IntegerPair Resolution { get; private set; }

        /// <summary>Number of vertices/samples along the X axis </summary>
        public int ResolutionX {
            get => _resolution.x;
            set 
            {
                if (_resolution.x != value)
                    SetResolution(value, ResolutionZ);
            }
        }

        /// <summary>Number of vertices/samples along the Z axis</summary>
        public int ResolutionZ {
            get => _resolution.z;
            set
            {
                if (_resolution.z != value)
                    SetResolution(ResolutionX, value);
            }
        }

        [HideInInspector]
        public Color defaultColor = Color.white;

        [HideInInspector]
        public Color fixedColor = Color.black;
        
        public bool IsInitialized => _isInitialized;
        public static int MaxVertices => _maxVertices;
        public static int MinResolution => _minResolution;

        [SerializeField]
        bool[] fixedGrid;

        public ref NativeArray<int> FixedGridRef => ref nativeFixedGrid;
        NativeArray<int> nativeFixedGrid;

        bool _isInitialized = false;

        [SerializeField]
        IntegerPair _resolution = new IntegerPair(50, 50);
        IntegerPair _oldResolution;
        const int _maxVertices = 65536;
        const int _minResolution = 3;

        private void Awake()
        {
            _isInitialized = false;
        }

        private void OnEnable()
        {
            _oldResolution = _resolution;
            
            if (fixedGrid == null)
                fixedGrid = new bool[ResolutionX * ResolutionZ];

            if (!nativeFixedGrid.IsCreated)
                nativeFixedGrid = new NativeArray<int>(ResolutionX * ResolutionZ, Allocator.Persistent);
            
            UpdateFixedNativeArray();

            _isInitialized = true;
        }

        /// <summary>
        /// Use this function to fix and unfix samples on the grid.
        /// </summary>
        /// <param name="x">0 to ResolutionX - 1</param>
        /// <param name="z">0 to ResolutionZ - 1</param>
        /// <param name="isFixed">New fixed status</param>
        public void SetFixed(int x, int z, bool isFixed)
        {
            if (x < 0 || x >= ResolutionX || z < 0 || z >= ResolutionZ)
            {
                Debug.LogError("WaveMaker - Cannot set the fixed status to the given sample. It is out of bounds. " + x + " - " + z);
                return;
            }

            SetFixed(Utils.FromSampleIndicesToIndex(Resolution, in x, in z), isFixed);
        }

        public void SetFixed(int index, bool isFixed)
        {
            if (index >= fixedGrid.Length || index < 0)
            {
                Debug.LogError(string.Format("WaveMaker - Cannot set the fixed status to the given sample index {0}. It is out of bounds.", index));
                return;
            }

            fixedGrid[index] = isFixed;
            nativeFixedGrid[index] = isFixed ? 1 : 0;
            SetModifiedAndDirty();
        }

        ///<summary>If you are using this very often, then it is recommended to grab the reference of the fixeGrid array using the property</summary>
        /// <returns>True if the status of the given sample is fixed or not.</returns>
        public bool IsFixed(int index)
        {
            if (index < 0 || index >= fixedGrid.Length)
            {
                Debug.LogError("WaveMaker - Cannot get the fixed status to the given sample. It is out of bounds.");
                return true;
            }

            return fixedGrid[index];
        }

        /// <summary>
        /// Change resolution of the descriptor. This will make the whole grid regenerate
        /// </summary>
        public void SetResolution(int newResolutionX, int newResolutionZ)
        {
            if (newResolutionX * newResolutionZ > _maxVertices)
            {
                if (newResolutionX > newResolutionZ)
                    newResolutionX = newResolutionZ / _maxVertices;
                
                if (newResolutionZ > newResolutionX)
                    newResolutionZ = newResolutionX / _maxVertices;

                Debug.LogError("WaveMaker - Descriptor resolution cannot generate a mesh with more than (" + _maxVertices + "). Clamping biggest resolution.");
            }

            if (newResolutionX < _minResolution || newResolutionZ < _minResolution)
            {
                newResolutionX = newResolutionX > _minResolution? _minResolution: newResolutionX;
                newResolutionZ = newResolutionZ > _minResolution? _minResolution: newResolutionZ;
                Debug.LogError("WaveMaker - Descriptor resolution cannot be less than " + _minResolution + "). Clamping.");
            }

            if (_resolution.x == newResolutionX && _resolution.z == newResolutionZ)
                return; 

            _oldResolution = _resolution;
            _resolution = new IntegerPair(newResolutionX, newResolutionZ);

            UpdateFixedGridSizes();
            OnResolutionChanged?.Invoke(this, null);
        }

        /// <summary>
        /// It will set to fixed all border samples
        /// </summary>
        public void FixBorders()
        {
            for (int x = 0; x < ResolutionX; x++)
                for (int z = 0; z < ResolutionZ; z++)
                    if (x == 0 || z == 0 || x == ResolutionX - 1 || z == ResolutionZ - 1)
                    {
                        fixedGrid[ResolutionX * z + x] = true;
                        nativeFixedGrid[ResolutionX * z + x] = 1;
                    }

            SetModifiedAndDirty();
        }

        /// <summary>
        /// Set all samples to fixed or unfixed status
        /// </summary>
        public void SetAllFixStatus(bool newValue = false)
        {
            for (int i = 0; i < fixedGrid.Length; i++)
            {
                fixedGrid[i] = newValue;
                nativeFixedGrid[i] = newValue ? 1 : 0;
            }
            SetModifiedAndDirty();
        }

        /// <summary>
        /// will update the fixed grid changing the resolution. 
        /// </summary>
        /// <param name="copyPreviousStatus">Current values will be kept if it grows or is reduced, adding unfixed values if growing</param>
        private void UpdateFixedGridSizes()
        {
            if (_oldResolution.x == ResolutionX && _oldResolution.z == ResolutionZ)
                return;

            bool[] fixedGridAux = fixedGrid;
            fixedGrid = new bool[ResolutionX * ResolutionZ];

            if (nativeFixedGrid.IsCreated)
                nativeFixedGrid.Dispose();

            nativeFixedGrid = new NativeArray<int>(ResolutionX * ResolutionZ, Allocator.Persistent);

            // Copy all values from the old one to the new one
            for (int z = 0; z < ResolutionZ; z++)
                for (int x = 0; x < ResolutionX; x++)
                {
                    int newIndex = Utils.FromSampleIndicesToIndex(_resolution, x, z);
                    int oldIndex = Utils.FromIndexToScaledIndex(newIndex, _resolution, _oldResolution);
                    fixedGrid[newIndex] = fixedGridAux[oldIndex];
                    nativeFixedGrid[newIndex] = fixedGridAux[oldIndex] ? 1 : 0;
                }

            //TODO: Before, there was no "OnFixedGridModified" emmited. Does it make a difference?
            SetModifiedAndDirty();
        }

        private void SetModifiedAndDirty()
        {
            OnFixedGridChanged?.Invoke(this, null);

            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        private void UpdateFixedNativeArray()
        {
            for (int i = 0; i < fixedGrid.Length; i++)
                nativeFixedGrid[i] = fixedGrid[i] ? 1 : 0;
        }

        internal void ApplyNativeArrayModifications(IntegerPair areaSize, IntegerPair areaOffset)
        {
            for (int z = areaOffset.z; z <= areaOffset.z + areaSize.z; ++z)
                for (int x = areaOffset.x; x <= areaOffset.x + areaSize.x; ++x)
                {
                    if (x >= ResolutionX || z >= ResolutionZ)
                        continue;

                    var i = Utils.FromSampleIndicesToIndex(_resolution, x, z);
                    fixedGrid[i] = nativeFixedGrid[i] == 1;
                }
            SetModifiedAndDirty();
        }

        private void OnDestroy()
        {
            //TODO: NOT CALLED! Using AssetModificationProcessor instead
            OnDestroyed?.Invoke(this, null);
        }
#endif
    }

#if UNITY_EDITOR && (UNITY_2018 || (MATHEMATICS_INSTALLED && BURST_INSTALLED && COLLECTIONS_INSTALLED))
    public class WaveMakerDescriptorDeleteDetector : UnityEditor.AssetModificationProcessor
        {
            static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions opt)
            {
                if (AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(WaveMakerDescriptor))
                {
                    foreach (var surf in GameObject.FindObjectsOfType<WaveMakerSurface>())
                    {
                        if (surf.Descriptor != null && path == AssetDatabase.GetAssetPath(surf.Descriptor.GetInstanceID()))
                            surf.Descriptor = null;
                    }
                }
                return AssetDeleteResult.DidNotDelete;
            }
        }
#endif
}

