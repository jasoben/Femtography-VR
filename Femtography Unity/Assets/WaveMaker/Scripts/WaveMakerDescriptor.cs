
#if UNITY_EDITOR
    using UnityEditor;
#endif

using UnityEngine;

namespace WaveMaker
{
    [CreateAssetMenu(fileName = "WaveMakerDescriptor", menuName = "WaveMaker Descriptor", order = 10)]
    public class WaveMakerDescriptor : ScriptableObject
    {
        #region MEMBERS

        /// <summary>
        /// Number of vertices along the X axis. Each vertex is a samples, so the number of samples on X
        /// </summary>
        [Header("Plane Resolution")]
        public int ResolutionX = 50;

        /// <summary>
        /// Number of vertices along the Z axis. Each vertex is a sample, so the number of samples on Z
        /// </summary>
        public int ResolutionZ = 50;

        [HideInInspector]
        public Color defaultColor = Color.white;

        [HideInInspector]
        public Color fixedColor = Color.black;

        [SerializeField]
        bool[] fixedGrid;

        /// <summary>
        /// Max resolution is the resolution that generates the maximum number of vertices of a mesh in Unity: 256 x 256 --> 65.536 vertices
        /// </summary>
        public int MaxResolution { get { return _maxResolution; } }
        int _maxResolution = 256;

        public bool IsInitialized { get { return _isInitialized; } }
        bool _isInitialized = false;

        public ref bool[] FixedGridRef { get { return ref fixedGrid; } }

        int oldResolutionX, oldResolutionZ;

        #endregion

        /************************************************/

        #region METHODS

        private void Awake()
        {
            _isInitialized = false;
        }

        private void OnEnable()
        {
            // If it is the first time we execute this
            if (fixedGrid == null)
            {
                fixedGrid = new bool[ResolutionX * ResolutionZ];
                UpdateFixedGrid(false);
            }

            oldResolutionX = ResolutionX;
            oldResolutionZ = ResolutionZ;

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
                // Cannot change fixed status on border
                if (x == 0 || z == 0 || x == ResolutionX - 1 || z == ResolutionZ - 1)
                    return;

                Debug.LogWarning("WaveMaker - Cannot set the fixed status to the given sample. It is out of bounds. " + x + " - " + z);
                return;
            }

            fixedGrid[ResolutionX * z + x] = isFixed;

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        ///<summary>If you are using this very much, then it is recommended to grab the reference of the fixeGrid array using the property</summary>
        /// <param name="x">0 to ResolutionX - 1</param>
        /// <param name="z">0 to ResolutionZ - 1</param>
        /// <returns>True if the status of the given sample is fixed or not.</returns>
        public bool IsFixed(int x, int z)
        {
            if (x < 0 || x >= ResolutionX || z < 0 || z >= ResolutionZ)
            {
                Debug.LogWarning("WaveMaker - Cannot get the fixed status to the given sample. It is out of bounds. " + x + " - " + z);
                return true;
            }

            return fixedGrid[ResolutionX * z + x];
        }

        /// <summary>
        /// Change resolution of the descriptor. This will make the whole grid regenerate
        /// </summary>
        public void SetResolution(int newResolutionX, int newResolutionZ)
        {
            if (newResolutionX < 2 || newResolutionZ < 2 || newResolutionX > _maxResolution || newResolutionZ > _maxResolution)
            {
                newResolutionX = Mathf.Clamp(newResolutionX, 2, _maxResolution);
                newResolutionZ = Mathf.Clamp(newResolutionZ, 2, _maxResolution);
                Debug.LogError("WaveMaker - Descriptor resolution cannot be out of the range (2-" + _maxResolution + "). Clamping.");
            }

            oldResolutionX = ResolutionX;
            oldResolutionZ = ResolutionZ;

            ResolutionX = newResolutionX;
            ResolutionZ = newResolutionZ;
            UpdateFixedGrid();
        }

        /// <summary>
        /// It will set to fixed all border samples
        /// </summary>
        public void FixBorders()
        {
            for (int x = 0; x < ResolutionX; x++)
                for (int z = 0; z < ResolutionZ; z++)
                    if ( x == 0 || z == 0 || x == ResolutionX-1 || z == ResolutionZ-1)
                        fixedGrid[ResolutionX * z + x] = true;

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// will update the fixed grid changing the resolution. 
        /// </summary>
        /// <param name="copyPreviousStatus">Current values will be kept if it grows or is reduced, adding unfixed values if growing</param>
        public void UpdateFixedGrid(bool copyPreviousStatus = true)
        {
            if (fixedGrid == null)
                copyPreviousStatus = false;

            // Create a new fixed grid with the new size
            bool[] fixedGridAux = new bool[ResolutionX * ResolutionZ];

            // Copy all values from the old one to the new one
            for (int x = 0; x < ResolutionX; x++)
                for (int z = 0; z < ResolutionZ; z++)
                {
                    int index = z * ResolutionX + x;
                    fixedGridAux[index] = false;

                    // Copy old samples
                    if (copyPreviousStatus && x < oldResolutionX && z < oldResolutionZ)
                        fixedGridAux[index] = fixedGrid[z * oldResolutionX + x];
                }


            fixedGrid = fixedGridAux;

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        
        /// <summary>
        /// Set all samples to fixed or unfixed status
        /// </summary>
        public void SetAllFixStatus(bool newValue = false)
        {
            for (int i = 0; i < fixedGrid.Length; i++)
                fixedGrid[i] = newValue;
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        #endregion
    }
}


