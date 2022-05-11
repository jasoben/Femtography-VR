using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using Unity.Jobs;
using Unity.Profiling;

#if UNITY_2018 || (MATHEMATICS_INSTALLED && BURST_INSTALLED && COLLECTIONS_INSTALLED)
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using System.Collections.Generic;
using Unity.Mathematics;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WaveMaker
{
    [HelpURL("http://wavemaker.lidia-martinez.com/")]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [ExecuteInEditMode]
    public class WaveMakerSurface : MonoBehaviour
    {

#if UNITY_2018 || (MATHEMATICS_INSTALLED && BURST_INSTALLED && COLLECTIONS_INSTALLED)

        public UnityEvent OnInitialized;
        public UnityEvent OnUninitialized;

        /// <summary> Notifies only changes from awake to asleep. Intermediate states are not sent to avoid noise </summary>
        public UnityEvent OnAwakeStatusChanged;

        #region PROPERTIES

        public bool IsInitialized => _initialized;
        public bool IsAwake => _awakeStatus == AwakeStatus.AWAKE || _awakeStatus == AwakeStatus.GETTING_ASLEEP;
        public AwakeStatus Status => _awakeStatus;

        public WaveMakerDescriptor Descriptor
        {
            get => _descriptor;
            set
            {
                if (value == _descriptor)
                    return;

                // Remove attachment with previous descriptor if any
                Uninitialize();
                _descriptor = value;
                Initialize();
            }
        }

        public CustomMeshManager MeshManager => _meshManager;

        /// <summary> Size on X and Z axis of the plane in Unity units in local space. Transform scale not applied. Can be used in runtime.</summary>
        public Vector2 Size_ls { get => new Vector2(_size_ls.x, _size_ls.y); set => SetSize(value.x, value.y); }

        /// <summary> Size along the X and Z local axis of a sample or grid cell. Transform scale not applied.</summary>
        public Vector2 SampleSize_ls => new Vector2(_sampleSize_ls.x, _sampleSize_ls.y);

        /// <summary> Number of vertices in the mesh on the local x axis </summary>
        public int ResolutionX => _resolution.x;

        /// <summary> Number of vertices in the mesh on the local z axis </summary>
        public int ResolutionZ => _resolution.z;

        /// <summary> The final occupancy on each sample on the grid in local space (y scale not applied) Be sure to call this on "LateUpdate" to get final results </summary>
        public ref NativeArray<float> Occupancy => ref _occupancy;

        #endregion

        /*************************************************/

        #region PUBLIC MEMBERS

        public enum InteractionType
        {
            VelocityBased,
            OccupancyBased
        }

        public enum AwakeStatus
        {
            ASLEEP,
            GETTING_ASLEEP,
            AWAKE
        }

        public struct BuoyantForceData
        {
            public float4 hitPos_ws;
            public float4 force_ws;
        }

        public struct RigidBodyData
        {
            public bool isKinematic;
            public float mass;
            public float4 centerOfMass_ws;
            public float3 linearVelocity;
            public float3 angularVelocity;
            public float4x4 inverseInertiaTensor;
        }

        public struct VelocityData
        {
            public float4 position;
            public float3 linearVelocity;
            public float3 angularVelocity;
        }

        [Tooltip("More sub executions of fixedUpdate means less efficiency, but allows for more Propagation Speed. Keep the value as near as 1 as possible.")]
        [Min(1)] //NOTE: Min does not work on 2018. Fixed on OnValidate
        public int substeps = 1;

        [Tooltip("Higher damping makes waves have a short life")]
        [Range(0f, 10f)]
        public float damping = 2f;

        [Tooltip("How fast waves propagate on the surface. WARNING: High values can make it more unstable")]
        [Min(0f)] //NOTE: Min does not work on 2018. Fixed on OnValidate
        public float propagationSpeed = 6;

        [Tooltip("Make waves smoother with this parameter")]
        [Range(0, 1)]
        public float waveSmoothness = 0;

        [Tooltip("Tweaks the speed of the whole simulation. Use only to create non-realistic simulations. Default is 1.")]
        [Range(0, 2)]
        public float speedTweak = 1;

        [Tooltip("How far colliders are detected over the surface (+Y). Useful for terrains.")]
        [HideInInspector]
        [Range(0, 10)]
        public float upwardsDetectionDistance = 0.5f;

        [Tooltip("How far colliders are detected under the surface (-Y)")]
        [HideInInspector]
        [Range(0, 10)]
        public float downwardsDetectionDistance = 0.5f;

        [Tooltip("Show more information on what is happening to this component")]
        public bool showDetailedLogMessages = false;

        [Tooltip("Use Velocity Based for a faster simulation with no floating forces. Buoyancy based is slower but has additional features available.")]
        public InteractionType interactionType = InteractionType.VelocityBased;

        // VELOCITY BASED

        [Tooltip("Scales the vertical velocity of objects interacting that generates vertical waves")]
        [Min(0f)] //NOTE: Min does not work on 2018. Fixed in OnValidate
        public float verticalPushScale = 0.5f;

        [Tooltip("Scales the horizontal velocity of objects interacting that generates side waves")]
        [Min(0f)] //NOTE: Min does not work on 2018. Fixed in OnValidate
        public float horizontalPushScale = 0.3f;

        [Tooltip("Clamp the speed of any interactor that affects this surface to this value to avoid too fast objects to affect the surface too much")]
        [Min(0)] //NOTE: Min does not work on 2018. Fixed in OnValidate
        public float interactorMaximumSpeedClamp = 100f;

        [Tooltip("Ignore the effect of any interactor moving slower than this speed")]
        [Min(0)] //NOTE: Min does not work on 2018. Fixed in OnValidate
        public float interactorMinimumSpeedClamp = 0.001f;


        // BUOYANCY BASED

        [Tooltip("Disable simulation when you only want the objects to float but not modify the surface. Faster")]
        public bool simulate = true;

        [Tooltip("Whether the surface is modified by the interactors when they move around it")]
        public bool getAffectedByInteractors = true;

        [Tooltip("Enable to activate vertical floating forces on the interactors")]
        public bool buoyancy = true;

        [Tooltip("Enable to activate horizontal drifting forces on the interactors")]
        public bool horizontalBuoyancy = true;

        [Tooltip("How deep underneath the surface interactors are detected")]
        [Min(0)] //NOTE: Min does not work on 2018. Fixed in OnValidate
        public float detectionDepth = 5f;

        [Tooltip("Density of the surface material. Use around 1 or 2 to make it work with Unity default mass values. Set to 1000 if you want to use real mass values.")]
        [Min(0)] //NOTE: Min does not work on 2018. Fixed in OnValidate
        public float density = 1f;

        [Tooltip("Avoid infinite jumps by increasing damping to reduce strength of buoyant forces on fast objects. Find the value that is the most stable for your case.")]
        [Min(0)] //NOTE: Min does not work on 2018. Fixed in OnValidate
        public float buoyancyDamping = 0.05f;

        [Tooltip("Show the wireframe in Scene View to check the cell sizes. WARNING: Adjust to the minimum. The bigger the slower. The surface interaction will be noisy if the size of your interactors surpases this. Enable warning logs to see errors!")]
        [Min(1)] //NOTE: Min does not work on 2018. Fixed in OnValidate
        public int nMaxCellsPerInteractor = 100;

        [Tooltip("How many interactors are detected. WARNING: Adjust to the minimum. The bigger the slower. Enable warning logs to see errors")]
        [Min(1)] //NOTE: Min does not work on 2018. Fixed in OnValidate
        public int nMaxInteractorsDetected = 1;

        [Tooltip("Override the amount of change an object entering the volume generates. No change (default) is 1")]
        [Range(0, 30)]
        public float effectScale = 1;

        [Tooltip("By default, the surface will display logs when settings are not correct while simulating, for example, when not enough cells are set for the interactors, more interactors than allowed are detected, etc.")]
        public bool showSettingWarningLogs = true;

        #endregion

        /*************************************************/

        #region PRIVATE MEMBERS

        ProfilerMarker simulationStepMarker = new ProfilerMarker("WaveMaker.SimulationStep");
        ProfilerMarker occupancyDataInitMarker = new ProfilerMarker("WaveMaker.OcuppancyDataInit");
        ProfilerMarker calculateActiveAreaMarker = new ProfilerMarker("WaveMaker.CalculateActiveArea");
        ProfilerMarker occupancyMarker = new ProfilerMarker("WaveMaker.Occupancy");
        ProfilerMarker surfaceAffectingInteractorsMarker = new ProfilerMarker("WaveMaker.Surface->Interactors");
        ProfilerMarker interactorAffectingSurfaceMarker = new ProfilerMarker("WaveMaker.Interactors->Surface");
        ProfilerMarker gradientsNormalsMarker = new ProfilerMarker("WaveMaker.GradientsNormals");

        bool _initialized = false;
        bool _simDataInitialized = false;
        AwakeStatus _awakeStatus = AwakeStatus.ASLEEP;

        CustomMeshManager _meshManager;
        BoxCollider _collider;
        Transform _transform;
        bool _fixedUpdateExecuted;

        [HideInInspector][SerializeField]
        WaveMakerDescriptor _descriptor;

        internal float4x4 _w2lTransformMatrix;
        internal float4x4 _l2wTransformMatrix;
        quaternion _w2lRotation; // Only updated on velocity based simulation
        float2 _sampleSize_ws;
        float3 _scale_l2w;

        float4 _position;
        float3 _linearVelocity;
        float3 _angularVelocity;

        /// <summary> Which height changes below this are ignored </summary>
        float _heightChangeThreshold = 0.0001f;

        /// <summary> Which cinetic energy values below this are considered 0 (value scaled by 100.000 to store a 64bit integer)</summary>
        float _cineticEnergyThreshold = 100f; 

        /// <summary> Number of fixed updates executed with near to zero cinetic energy </summary>
        internal int _asleepCounter = 0;

        /// <summary> Value from which the cinetic energy can make the surface to be asleep to avoid doing it when surface is flat</summary>
        internal int _asleepCounterLimit = 50;

        //TODO Workaround to add additional ray hits due to the possibility of having more colliders hit that are not interactors and we should ignore them.
        internal int _nAdditionalHits = 10;

        /// <summary> A single cinetic energy value. Needed as a native container to be able to write on jobs. Float stored as a 64bit integer scaled by 100.000 </summary>
        internal NativeArray<long> _cineticEnergy;
        internal NativeArray<long> _awakeFromInteractor;

        /// <summary>
        /// Local space size of the surface in Unity units. Transform scale not applied
        /// </summary>
        [SerializeField]
        [HideInInspector]
        internal float2 _size_ls = new float2(10, 10);

        /// <summary>
        /// Local space size of a simple sample or cell in the grid in Unity Units. Transform scale not applied.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        internal float2 _sampleSize_ls = new float2(10, 10);

        /// <summary>
        /// Represents the number of vertices along local X and Z axis. Each vertex is a sample
        /// </summary>
        internal IntegerPair _resolution = new IntegerPair(0, 0);

        /// <summary>
        /// Represents the number of vertices along local X and Z axis including 
        /// ghost cells, added for simulation calculations
        /// </summary>
        internal IntegerPair _resolutionGhost = new IntegerPair(12, 12);

        /// <summary>
        /// Contains the mesh grid heights, it has one or more columns and 
        /// rows of ghost cells around
        /// </summary>
        internal NativeArray<float> _heights;

        internal NativeArray<float> _velocities;
        internal NativeArray<float> _accelerations;
        internal NativeArray<float4> _gradients;
        internal NativeArray<Vector3> _normals;
        internal NativeArray<float4> _relativeVelocities;

        /// <summary>
        /// Contains a block of items per object the surface can detect (set in 
        /// the settings). The number of items each block contains is maximum 
        /// number of cells allowed per object (set in the settings).
        /// Each item stores an interaction data (a cell id and info about the 
        /// cell occupied)
        /// </summary>
        internal NativeArray<InteractionData> _interactionData;

        /// <summary>
        /// Used to find the starting point for each object on the interaction data array
        /// </summary>
        internal NativeArray<int> _hitsPerObject;

        /// <summary>
        /// the occupied height on each cell by interactors in local space (no scale on y applied)
        /// </summary>
        internal NativeArray<float> _occupancy;

        /// <summary>
        /// One occupancy value per cell. Backup from the previous iteration.
        /// </summary>
        NativeArray<float> _occupancyPrevious;

        internal List<WaveMakerInteractor> _interactorsDetected;
        internal List<Rigidbody> _rigidbodies;

        /// <summary>Used to correlate InstanceIDs of the Colliders detected and the local index assigned</summary>
        internal NativeHashMap<int, int> _colliderIdsToIndices;

        /// <summary>Used to correlate collider local index and the rigidbody local index assigned</summary>
        internal NativeHashMap<int, int> _colliderToRbIndices;

        internal NativeList<NativeCollider> _interactorColliders;
        internal NativeList<RigidBodyData> _rigidBodyData;
        internal NativeList<VelocityData> _velocityData;

        /// <summary> Use by the Surface Debugger in case we want to draw the interal forces applied. The creation and deletion is managed by the debugger </summary>
        internal bool exportForces = false;
        internal NativeArray<BuoyantForceData> _buoyantForces;

        #endregion

        /*************************************************/

        #region PUBLIC METHODS


        /// <summary>
        /// Generates the component data: gathers resolution data from the descriptor and generates the mesh and internal data, collider data...etc
        /// If already initialized, will uninitialize and repeat the initialization.
        /// </summary>
        /// <returns>false if it couldn't initialize</returns>
        public bool Initialize()
        {
            Uninitialize();

            if (Descriptor == null)
                return false;

            _transform = transform;
            _descriptor.OnResolutionChanged += OnDescriptorResolutionChanged;
            _descriptor.OnDestroyed += OnDescriptorDestroyed;
            CleanupCollidersAndAddOne();
            UpdateStoredSurfaceData();
            InitializeSimulationData();
            _meshManager = new CustomMeshManager(GetComponent<MeshFilter>(), _resolution, _size_ls);

            _initialized = true;

            if (showDetailedLogMessages)
                Utils.Log("Initialized", gameObject);

            OnInitialized?.Invoke();
            return true;
        }

        public void UpdateCollider()
        {
            if (!Application.isPlaying)
                return;

            if (_collider == null)
            {
                Utils.LogError("Collider not available, cannot update it.", gameObject);
                return;
            }

            if (interactionType == InteractionType.OccupancyBased)
            {
                _collider.center = new Vector3(_size_ls.x * 0.5f, - detectionDepth * 0.5f, _size_ls.y * 0.5f);
                _collider.size = new Vector3(_size_ls.x + 2 * _sampleSize_ls.x, detectionDepth, _size_ls.y + 2 * _sampleSize_ls.y);
            }
            else
            {
                _collider.center = new Vector3(_size_ls.x * 0.5f, 0, _size_ls.y * 0.5f);
                _collider.size = new Vector3(_size_ls.x, 0.001f, _size_ls.y);
            }
        }

        /// <summary>Get the current height (on local space) of the given sample/cell. If a lot of heights are needed, better get the heights array directly.</summary>
        /// <param name="sampleIndex">Use Utils class to get sample indices from x,y indices on the array. Not checked for efficiency reasons</param>
        public float GetHeight(int sampleIndex)
        {
            sampleIndex = Utils.FromNoGhostIndexToGhostIndex(sampleIndex, in _resolution, in _resolutionGhost);
            return _heights[sampleIndex];
        }

        /// <summary>
        /// Instantly change the height of a given sample in the surface by the given offset value 
        /// (negative reduces the height, positive raises the height). Cannot affect fixed cells.
        /// Used for custom modification of the surface. Use with caution. Awakes surface.
        /// </summary>
        public void SetHeightOffset(int sampleIndex, float offset)
        {
            if (Descriptor.FixedGridRef[sampleIndex] == 1)
                return;

            sampleIndex = Utils.FromNoGhostIndexToGhostIndex(sampleIndex, in _resolution, in _resolutionGhost);
            _heights[sampleIndex] += offset;
            _awakeStatus = AwakeStatus.AWAKE;
        }

        /// <summary>
        /// Get position in local or world space of the current sample/cell center. Y coordinate will be 0 by default.
        /// </summary>
        /// <param name="worldSpace">True if we want the position given in world space. Needs additional computation, by default it is local space</param>
        /// <param name="includeHeight">The y coordinate will be also gathered. This needs additional computation. by default not included.</param>
        /// <param name="includeGhostCells">Ghost cells are added to the extremes to perform additional calculations. by default they are not included</param>
        /// <exception cref="ArgumentException">If index out of range for the selected parameters</exception>
        public float4 GetPositionFromSample(int sampleIndex, bool worldSpace = false, bool includeHeight = false, bool includeGhostCells = false)
        {
            if (sampleIndex < 0 || sampleIndex >= _heights.Length)
                throw new ArgumentException(string.Format("Sample {0} index {1} is out of range.", includeGhostCells ? "ghost" : "", sampleIndex));

            Utils.FromIndexToSampleIndices(sampleIndex, includeGhostCells ? _resolutionGhost : _resolution, out int sampleX, out int sampleZ);

            var pos = new float4(sampleX * _sampleSize_ls.x, 0, sampleZ * _sampleSize_ls.y, 0);

            if (includeGhostCells)
            {
                pos.x -= _sampleSize_ls.x * (_resolutionGhost.x - _resolution.x) / 2;
                pos.z -= _sampleSize_ls.y * (_resolutionGhost.z - _resolution.z) / 2;
            }

            if (includeHeight)
            {
                if (!includeGhostCells)
                    sampleIndex = Utils.FromNoGhostIndexToGhostIndex(sampleIndex, in _resolution, in _resolutionGhost);

                pos.y = _heights[sampleIndex];
            }

            if (worldSpace)
                pos = new float4(math.mul(_l2wTransformMatrix, new float4(pos.xyz, 1)).xyz, 0);

            return pos;
        }

        public float4 GetBottomPositionFromSample(int sampleIndex, bool worldSpace = false)
        {
            // Local pos
            var result = GetPositionFromSample(sampleIndex);
            result.y -= detectionDepth;

            if (worldSpace)
                result = new float4(math.mul(_l2wTransformMatrix, new float4(result.xyz, 1)).xyz, 0);

            return result;
        }


        /// <summary>
        /// Using the current parameters, calculate if the surface should be stable or not. Unstability means the surface effect doesn't fade but grows.
        /// </summary>
        /// <returns>True if it should be stable.</returns>
        public bool CheckStabilityCondition()
        {
            return (Time.fixedDeltaTime / substeps) < Mathf.Min(_sampleSize_ls.x, _sampleSize_ls.y) / propagationSpeed;
        }

        /// <summary>
        /// Add fixed cells to the current descriptor data with the collisions with objects of the given layer.
        /// The Collision Detection Height is used as a threshold of collision detection.
        /// </summary>
        public void FixCollisions(int layer)
        {
            if (Application.isPlaying)
                return;

            UpdateStoredSurfaceData();
            Quaternion rotation = _transform.rotation;
            var scale = _transform.localScale;
            float halfExtentsY = (upwardsDetectionDistance + downwardsDetectionDistance) * 0.5f;
            Vector3 halfExtents = new Vector3(_sampleSize_ls.x * scale.x * 0.5f, halfExtentsY, _sampleSize_ls.y * scale.z * 0.5f);
            LayerMask layerMask = 1 << layer;
            bool isSameLayer = layer == gameObject.layer;
            int nCols = GetComponents<Collider>().Length;
            float heightOffset = upwardsDetectionDistance - halfExtentsY;

            // For each sample on the patch
            for (int i = 0; i < Descriptor.FixedGridRef.Length; i++)
            {
                // Create a small box around the sample to collide with objects
                var center = GetPositionFromSample(i, true);
                var offsetCenter = new Vector3(center.x, center.y + heightOffset, center.z);
                if (Physics.OverlapBox(offsetCenter, halfExtents, rotation, layerMask).Length > (isSameLayer ? nCols : 0))
                    Descriptor.SetFixed(i, true);
            }
        }

        /// <summary>
        /// Deletes information stored on this object
        /// </summary>
        public void Uninitialize()
        {
            if (!_initialized)
                return;

            if (_simDataInitialized)
            {
                foreach (var interactor in _interactorsDetected)
                    interactor.OnDisabledOrDestroyed -= OnInteractorDisabledOrDestroyed;

                if (_heights.IsCreated)
                    _heights.Dispose();
                if (_velocities.IsCreated)
                    _velocities.Dispose();
                if (_accelerations.IsCreated)
                    _accelerations.Dispose();
                if (_colliderIdsToIndices.IsCreated)
                    _colliderIdsToIndices.Dispose();
                if (_colliderToRbIndices.IsCreated)
                    _colliderToRbIndices.Dispose();
                if (_awakeFromInteractor.IsCreated)
                    _awakeFromInteractor.Dispose();
                if (_cineticEnergy.IsCreated)
                    _cineticEnergy.Dispose();
                if (_gradients.IsCreated)
                    _gradients.Dispose();
                if (_normals.IsCreated)
                    _normals.Dispose();

                if (_interactionData.IsCreated)
                    _interactionData.Dispose();
                if (_rigidBodyData.IsCreated)
                    _rigidBodyData.Dispose();
                if (_velocityData.IsCreated)
                    _velocityData.Dispose();
                if (_hitsPerObject.IsCreated)
                    _hitsPerObject.Dispose();
                if (_occupancy.IsCreated)
                    _occupancy.Dispose();
                if (_occupancyPrevious.IsCreated)
                    _occupancyPrevious.Dispose();

                if (_relativeVelocities.IsCreated)
                    _relativeVelocities.Dispose();
                if (_interactorColliders.IsCreated)
                    _interactorColliders.Dispose();

                if (_buoyantForces.IsCreated)
                    _buoyantForces.Dispose();

                _simDataInitialized = false;
            }

            if (Application.isPlaying)
                Destroy(_collider);

            if (Descriptor != null)
            {
                _descriptor.OnResolutionChanged -= OnDescriptorResolutionChanged;
                _descriptor.OnDestroyed -= OnDescriptorDestroyed;
            }

            _meshManager?.Dispose();

            _initialized = false;

            if (showDetailedLogMessages)
                Utils.Log("Uninitialized", gameObject);

            OnUninitialized?.Invoke();
        }

        #endregion

        /*************************************************/

        #region PRIVATE METHODS

        private void Awake()
        {
            // Called on surface creation
            if (!Application.isPlaying)
                return;

            if (Descriptor == null || !Descriptor.IsInitialized)
            {
                Utils.LogError("Cannot be initialized. No Descriptor is attached in that gameobject or it could not be initialized.", gameObject);
                gameObject.SetActive(false);
                return;
            }

            Initialize();
        }

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Uninitialize();
        }

        private void Reset()
        {
            Uninitialize();
            _size_ls = new float2(10, 10);
        }

        void FixedUpdate()
        {
            if (!_initialized)
                return;

            UpdateStoredSurfaceData();
            UpdateInteractorColliders();

            JobHandle handle = default;

            if (IsAwake && simulate)
                SimulateSurface(ref handle);

            JobHandle.ScheduleBatchedJobs();

            if (interactionType == InteractionType.OccupancyBased)
            {
                if (getAffectedByInteractors || buoyancy)
                    UpdateOccupancy(ref handle);

                if (simulate & getAffectedByInteractors)
                    UpdateHeightsFromInteraction(ref handle);

                if (IsAwake && simulate)
                    UpdateGradientsAndNormals(ref handle);

                if (buoyancy)
                    ApplyBuoyantForces(ref handle);
            }
            else if (interactionType == InteractionType.VelocityBased)
            {
                VelocityBasedInteraction(ref handle);

                if (IsAwake && simulate)
                    UpdateGradientsAndNormals(ref handle);
            }

            handle.Complete();

            if (interactionType == InteractionType.OccupancyBased)
            {
                if (buoyancy)
                {
                    // Applyforces to actual rigidbodies
                    for (int i = 0; i < _rigidbodies.Count; i++)
                    {
                        _rigidbodies[i].velocity = _rigidBodyData[i].linearVelocity;
                        _rigidbodies[i].angularVelocity = _rigidBodyData[i].angularVelocity;
                    }
                }

                if (showSettingWarningLogs)
                    ShowLimitWarnings();
            }

            UpdateAwakeStatus();
            _fixedUpdateExecuted = true;
        }

        private void Update()
        {
            if (!_initialized)
                return;

            //TODO: Copy only when changed.
            if (IsAwake && _fixedUpdateExecuted)
            {
                _fixedUpdateExecuted = false;
                _meshManager.CopyHeightsAndNormals(in _heights, in _normals, in _resolution, in _resolutionGhost);
            }
        }

#if !UNITY_2019_1_OR_NEWER
        //NOTE: Setting a Min using an Attribute does not work in Unity v2018.
        private void OnValidate()
        {
            if (substeps < 1)
                substeps = 1;

            if (propagationSpeed < 0)
                propagationSpeed = 0;

            if (detectionDepth < 0)
                detectionDepth = 0;

            if (density < 0)
                density = 0;

            if (buoyancyDamping < 0)
                buoyancyDamping = 0;

            if (nMaxCellsPerInteractor < 1)
                nMaxCellsPerInteractor = 1;

            if (nMaxInteractorsDetected < 1)
                nMaxInteractorsDetected = 1;

            if (verticalPushScale < 0)
                verticalPushScale = 0;

            if (horizontalPushScale < 0)
                horizontalPushScale = 0;

            if (interactorMaximumSpeedClamp < 0)
                interactorMaximumSpeedClamp = 0;

            if (interactorMinimumSpeedClamp < 0)
                interactorMinimumSpeedClamp = 0;

            if (buoyancyDamping < 0)
                buoyancyDamping = 0;
        }
#endif

        // TODO Use a job to initialize all
        internal void InitializeSimulationData()
        {
            _simDataInitialized = false;

            _resolution = new IntegerPair(Descriptor.ResolutionX, Descriptor.ResolutionZ);
            //NOTE: For now 2 ghost cells (1 cell on the border) is enough
            _resolutionGhost = new IntegerPair(_resolution.x + 2, _resolution.z + 2);
            _sampleSize_ls = new float2(_size_ls.x / (_resolution.x - 1), _size_ls.y / (_resolution.z - 1));
            // IMPORTANT NOTE: Recalculating Resolution from the size and sample size will return errors in precision
            // and the wrong resolution in certain cases. Never calculate resolution by hand.

            //Note: Heights has an extra ghost column for open boundaries
            _heights = new NativeArray<float>(_resolutionGhost.x * _resolutionGhost.z, Allocator.Persistent);

            _velocities = new NativeArray<float>(_resolution.x * _resolution.z, Allocator.Persistent);
            _accelerations = new NativeArray<float>(_resolution.x * _resolution.z, Allocator.Persistent);
            _awakeStatus = AwakeStatus.ASLEEP;
            _cineticEnergy = new NativeArray<long>(1, Allocator.Persistent);
            _cineticEnergy[0] = 0;
            _awakeFromInteractor = new NativeArray<long>(1, Allocator.Persistent);
            _awakeFromInteractor[0] = 0;
            _gradients = new NativeArray<float4>(_resolution.x * _resolution.z, Allocator.Persistent);
            _normals = new NativeArray<Vector3>(_resolution.x * _resolution.z, Allocator.Persistent);

            _interactorsDetected = new List<WaveMakerInteractor>(nMaxInteractorsDetected);
            _rigidbodies = new List<Rigidbody>(nMaxInteractorsDetected);
            _colliderIdsToIndices = new NativeHashMap<int, int>(nMaxInteractorsDetected, Allocator.Persistent);
            _colliderToRbIndices = new NativeHashMap<int, int>(nMaxInteractorsDetected, Allocator.Persistent);
            _interactorColliders = new NativeList<NativeCollider>(nMaxInteractorsDetected, Allocator.Persistent);
            _rigidBodyData = new NativeList<RigidBodyData>(nMaxInteractorsDetected, Allocator.Persistent);
            _velocityData = new NativeList<VelocityData>(nMaxInteractorsDetected, Allocator.Persistent);

            //NOTE: Initialized by the debugger. Otherwise it is empty.
            _buoyantForces = new NativeArray<BuoyantForceData>(0, Allocator.Persistent);

            // Heights array is bigger due to the ghost samples available
            for (int i = 0; i < _heights.Length; ++i)
                _heights[i] = 0;

            // Rest of the arrays
            for (int i = 0; i < _velocities.Length; ++i)
            {
                _velocities[i] = 0;
                _accelerations[i] = 0;
                _gradients[i] = float4.zero;
                _normals[i] = Vector3.up;

                if (_occupancy.IsCreated)
                    _occupancy[i] = 0;

                if (_occupancyPrevious.IsCreated)
                    _occupancyPrevious[i] = 0;

                if (_relativeVelocities.IsCreated)
                    _relativeVelocities[i] = float4.zero;
            }

            if (interactionType == InteractionType.OccupancyBased)
            {
                if (Application.isPlaying)
                {
                    _occupancy = new NativeArray<float>(_resolution.x * _resolution.z, Allocator.Persistent);
                    _occupancyPrevious = new NativeArray<float>(_resolution.x * _resolution.z, Allocator.Persistent);
                }

                InteractionDataArray.CreateAndInitialize(nMaxInteractorsDetected, nMaxCellsPerInteractor, out _interactionData);

                // TODO: Necessary to initialize?
                _hitsPerObject = new NativeArray<int>(nMaxInteractorsDetected, Allocator.Persistent);
                for (int i = 0; i < _hitsPerObject.Length; i++)
                    _hitsPerObject[i] = 0;
            }
            else if (interactionType == InteractionType.VelocityBased)
                _relativeVelocities = new NativeArray<float4>(_resolution.x * _resolution.z, Allocator.Persistent);

            _simDataInitialized = true;
        }
        
        /// <summary> Size of the surface in local space. Regenerates mesh and collider </summary>
        private void SetSize(float width, float depth)
        {
            width = math.clamp(width, 0.001f, float.MaxValue);
            depth = math.clamp(depth, 0.001f, float.MaxValue);
            _size_ls = new float2(width, depth);
            _sampleSize_ls = new float2(_size_ls.x / (_resolution.x - 1), _size_ls.y / (_resolution.z - 1));
            _meshManager.UpdateMesh(in _resolution, in _size_ls);
            UpdateCollider();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool IsAwakeDueToSimulation()
        {
            return _cineticEnergy[0] > _cineticEnergyThreshold;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool IsAwakeDueToInteraction()
        {
            return _awakeFromInteractor[0] == 1;
        }

        private void CleanupCollidersAndAddOne()
        {
            if (!Application.isPlaying)
                return;

            var colliders = GetComponents<Collider>();
            if (colliders.Length > 1)
            {
                Utils.LogError("Please remove all colliders attached to the WaveMaker Surface. It is no longer needed. The collider is created in play mode. Dissabling other colliders.", gameObject);

                foreach (var col in colliders)
                    col.enabled = false;
            }
            _collider = gameObject.AddComponent<BoxCollider>();
            _collider.isTrigger = true;
            
            UpdateCollider();
        }
        
        private void OnDescriptorResolutionChanged(object sender, EventArgs e)
        {
            Initialize();
        }

        private void OnDescriptorDestroyed(object sender, EventArgs e)
        {
            Descriptor = null;
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (!Application.isPlaying || !_initialized)
                return;
            
            var interactor = WaveMakerInteractor.GetRelatedInteractor(otherCollider);

            //NOTE: Triggers can sometimes generate double and triple events for the same objets.
            if (interactor == null || _colliderIdsToIndices.TryGetValue(interactor.NativeCollider.instanceId, out int _))
                return;

            AddInteractor(interactor);
        }

        /// <summary>
        /// Use this method to make the surface be aware of the given interactor in case you don't want it to
        /// be detected via OnTriggerEnter (Collider with RigidBody attached to the hierarchy)
        /// </summary>
        public void AddInteractor(WaveMakerInteractor interactor)
        {
            if (!_initialized || !interactor.Initialized)
                return;

            if (interactionType == InteractionType.OccupancyBased && _interactorsDetected.Count >= nMaxInteractorsDetected)
            {
                if (showSettingWarningLogs)
                    Utils.LogWarning(string.Format(
                            "Setting Warning Log: Cannot detect more than the specified {0} interactors. Stop simulation and increase the number to the minimum needed. " +
                            "The bigger the value, the slower the simulation. Visit the official website to see examples for each parameter.", 
                             nMaxInteractorsDetected), gameObject);
                return;
            }

#if NATIVEHASHMAP_COUNT
            if (!_colliderIdsToIndices.TryAdd(interactor.NativeCollider.instanceId, _colliderIdsToIndices.Count()))
#else
            if (!_colliderIdsToIndices.TryAdd(interactor.NativeCollider.instanceId, _colliderIdsToIndices.Length))
#endif
            {
                Utils.LogError("Cannot add interactor to the list of interactors. Please contact asset creator.", gameObject);
                return;
            }

            interactor.OnDisabledOrDestroyed += OnInteractorDisabledOrDestroyed;
            _interactorsDetected.Add(interactor);
            _interactorColliders.Add(interactor.NativeCollider);

            if (interactionType == InteractionType.VelocityBased)
                interactor.UpdateVelocities(); // To avoid a big jump on velocities since last time it touched the surface
            else if (interactionType == InteractionType.OccupancyBased)
                TryAddInteractorRigidBody(interactor);

            if (showDetailedLogMessages)
                Utils.Log(string.Format("Interactor detected: {0}. Interactors detected now : {1}.", 
                                interactor.gameObject.name, _interactorsDetected.Count), gameObject);
        }

        internal void TryAddInteractorRigidBody(WaveMakerInteractor interactor)
        {
            if (interactor.RigidBody == null)
                return;

            // Add rigidbody if not already there
            int rbIndex = _rigidbodies.FindIndex(rb => rb.GetInstanceID() == interactor.RigidBody.GetInstanceID());
            if (rbIndex == -1)
            {
                rbIndex = _rigidbodies.Count;
                _rigidbodies.Add(interactor.RigidBody);
            }

            // Add relationship between the local indices of the collider and the rb
            if (!_colliderIdsToIndices.TryGetValue(interactor.NativeCollider.instanceId, out int colliderIndex))
            {
                Utils.LogError(string.Format("Cannot find collider in the colliders ids relationhsip for interactor {0}. Please contact asset creator.", interactor.name), gameObject);
                return;
            }

            if (!_colliderToRbIndices.TryAdd(colliderIndex, rbIndex))
            {
                Utils.LogError(string.Format("Cannot add collider-rigidbody indices relationship with interactor {0}. Please contact asset creator.", interactor.name), gameObject);
                return;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!Application.isPlaying || !_initialized)
                return;

            var interactor = WaveMakerInteractor.GetRelatedInteractor(other);
            if (interactor != null)
                RemoveInteractor(interactor);
        }

        private void OnInteractorDisabledOrDestroyed(object obj, EventArgs _)
        {
            RemoveInteractor(obj as WaveMakerInteractor);
        }

        /// <summary>
        /// Use this method to remove the interactor that is detected by the surface. Use this in case you don't want the interactor
        /// to be detected via OnTriggerExit (collider with rigidbody attached to the hierarchy)
        /// </summary>
        public void RemoveInteractor(WaveMakerInteractor interactor)
        {
            if (!_initialized)
                return;

            int instanceID = interactor.NativeCollider.instanceId;
            if (!_colliderIdsToIndices.TryGetValue(instanceID, out int removedId))
                return;

            if (interactionType == InteractionType.OccupancyBased)
                TryRemoveInteractorRigidBody(interactor);

            _interactorsDetected.RemoveAtSwapBack(removedId);
            _colliderIdsToIndices.Remove(instanceID);
            _interactorColliders.RemoveAtSwapBack(removedId);

            // If swapped last with other position in the arrays, remove old index in the dict and add with new index
            if (removedId != _interactorsDetected.Count)
            {
                int swappedColliderId = _interactorsDetected[removedId].NativeCollider.instanceId;
                int oldIndex = _colliderIdsToIndices[swappedColliderId];
                _colliderIdsToIndices.Remove(swappedColliderId);
                _colliderIdsToIndices.TryAdd(swappedColliderId, removedId);

                // Collider indices have changed, update rigidbody relationships
                if (_colliderToRbIndices.TryGetValue(oldIndex, out int rbIndex))
                {
                    _colliderToRbIndices.Remove(oldIndex);
                    _colliderToRbIndices.TryAdd(removedId, rbIndex);
                }
            }

            interactor.OnDisabledOrDestroyed -= OnInteractorDisabledOrDestroyed;

            if (Application.isPlaying && _interactorsDetected.Count == 0)
            {
                if (interactionType == InteractionType.OccupancyBased)
                    ResetOccupancyBasedDataForNewStep();
                else if (interactionType == InteractionType.VelocityBased)
                    ResetVelocityBasedDataForNewStep();
            }

            if (showDetailedLogMessages)
                Utils.Log(string.Format("Interactor removed {0}. Interactors detected now: {1}", 
                    interactor.name, _interactorsDetected.Count), gameObject);
        }

        //TODO: Create wrapper classes to simplify management of these arrays
        internal void TryRemoveInteractorRigidBody(WaveMakerInteractor interactor)
        {
            if (!_colliderIdsToIndices.TryGetValue(interactor.NativeCollider.instanceId, out int interactorIndex))
            {
                Utils.LogError("This interactor has been removed before removing the rigidbody, it is not in the interactors list anymore. Please, contact the asset creator!", gameObject);
                return;
            }

            // NOTE: Additional checks before hand don't work in situations where objects are destroyed, rbs before interactors, etc.
            if (!_colliderToRbIndices.TryGetValue(interactorIndex, out int rbIndex) || rbIndex == -1)
                return;

            _colliderToRbIndices.Remove(interactorIndex);
            
            // Find if any other collider is related to this rb
            bool found = false;
            for (int i = 0; i < _interactorsDetected.Count; i++)
            {
                if (_colliderToRbIndices.TryGetValue(i, out int index) && rbIndex == index)
                    found = true;
            }

            // delete rb if no other collider is related. 
            if (!found)
            {
                int rbIndexPrevious = _rigidbodies.Count - 1;
                _rigidbodies.RemoveAtSwapBack(rbIndex);

                // One rb has changed index, update related colliders relationship
                for (int i = 0; i < _interactorsDetected.Count; i++)
                {
                    if (_colliderToRbIndices.TryGetValue(i, out int index) && rbIndexPrevious == index)
                    {
                        // TODO: Assigning directly to [i] doesn't work in Unity 2018. Fix when deprecating v2018.
                        _colliderToRbIndices.Remove(i);
                        _colliderToRbIndices.TryAdd(i, rbIndex);
                    }
                }
            }   
        }

        private unsafe void ResetSimulationData()
        {
            UnsafeUtility.MemClear(_heights.GetUnsafePtr(), sizeof(float) * _heights.Length);
            UnsafeUtility.MemClear(_velocities.GetUnsafePtr(), sizeof(float) * _velocities.Length);
            UnsafeUtility.MemClear(_accelerations.GetUnsafePtr(), sizeof(float) * _accelerations.Length);
        }

        internal void UpdateStoredSurfaceData()
        {
            Matrix4x4 w2lMat = _transform.worldToLocalMatrix;
            Matrix4x4 l2wMat = _transform.localToWorldMatrix;
            _w2lTransformMatrix = w2lMat;
            _l2wTransformMatrix = l2wMat;

            var scale = _transform.lossyScale;
            _sampleSize_ws = new float2(_sampleSize_ls.x * scale.x, _sampleSize_ls.y * scale.z);
            _scale_l2w = new float3(scale);

            if (Application.isPlaying && interactionType == InteractionType.VelocityBased)
            {
                var surfOldPos = _position;
                _position = new float4(_transform.position, 0);

                var surfOldRot = math.inverse(_w2lRotation); //NOTE: Gather old value before updating
                var surfCurrentRot = l2wMat.rotation;
                _w2lRotation = w2lMat.rotation;

                _linearVelocity = ((_position - surfOldPos) / Time.fixedDeltaTime).xyz;
                _angularVelocity = Utils.GetAngularVelocity(surfOldRot, surfCurrentRot);
            }
        }

        private void UpdateInteractorColliders()
        {                
            for (int i = 0; i < _interactorsDetected.Count; i++)
            {
                _interactorsDetected[i].UpdateNativeCollider();
                _interactorColliders[i] = _interactorsDetected[i].NativeCollider;
            }
        }

        private void SimulateSurface(ref JobHandle inOutHandle)
        {
            for (int i = 0; i < substeps; i++)
                SimulateStep(ref inOutHandle);
        }

        private void SimulateStep(ref JobHandle inOutHandle)
        {
            simulationStepMarker.Begin();

            var accelerationJob = new AccelerationsJob
            {
                heights = _heights,
                accelerations = _accelerations,
                fixedSamples = Descriptor.FixedGridRef,
                resolution = _resolution,
                ghostResolution = _resolutionGhost,
                cineticEnergy = _cineticEnergy
            };
            inOutHandle = accelerationJob.Schedule(_accelerations.Length, 64, inOutHandle);

            float precalculation = propagationSpeed * propagationSpeed / (_sampleSize_ls.x * _sampleSize_ls.y);
            float maxOffset = (1 - waveSmoothness) * (_sampleSize_ls.x < _sampleSize_ls.y ? _sampleSize_ls.x : _sampleSize_ls.y);
            float substepFixedDeltaTime = Time.fixedDeltaTime / substeps;
            var heightsAndVelsJob = new HeightsAndVelocitiesJob
            {
                heights = _heights,
                velocities = _velocities,
                accelerations = _accelerations,
                fixedSamples = Descriptor.FixedGridRef,
                resolution = _resolution,
                ghostResolution = _resolutionGhost,
                maxOffset = maxOffset,
                fixedDeltaTime = substepFixedDeltaTime,
                precalculation = precalculation,
                damping = damping,
                propagationSpeed = propagationSpeed,
                sampleSize = _sampleSize_ls,
                speedTweak = speedTweak,
                cineticEnergy = _cineticEnergy
            };
            inOutHandle = heightsAndVelsJob.Schedule(_heights.Length, 32, inOutHandle);

            simulationStepMarker.End();
        }

        private void UpdateOccupancy(ref JobHandle inOutHandle)
        {
            if (_interactorsDetected.Count <= 0)
                return;

            ResetOccupancyBasedDataForNewStep();
            CalculateMinimumSharedAreaOfInteractors(out IntegerPair areaResolution, out int xOffset, out int zOffset);
            CalculateOccupancy(ref inOutHandle, areaResolution, xOffset, zOffset);  
        }

        private void ResetOccupancyBasedDataForNewStep()
        {
            occupancyDataInitMarker.Begin();

            InteractionDataArray.Reset(ref _interactionData, nMaxCellsPerInteractor);

            unsafe
            {
                UnsafeUtility.MemClear(_occupancyPrevious.GetUnsafePtr(), sizeof(float) * _occupancyPrevious.Length);
                UnsafeUtility.MemClear(_hitsPerObject.GetUnsafePtr(), sizeof(int) * _hitsPerObject.Length);

                if (exportForces)
                    UnsafeUtility.MemClear(_buoyantForces.GetUnsafePtr(), sizeof(BuoyantForceData) * _buoyantForces.Length);
            }

            // Swap arrays
            var aux = _occupancyPrevious;
            _occupancyPrevious = _occupancy;
            _occupancy = aux;

            _rigidBodyData.ResizeUninitialized(_interactorsDetected.Count);

            for (int i = 0; i < _rigidbodies.Count; i++)
            {
                var rb = _rigidbodies[i];
                var data = new RigidBodyData();

                data.isKinematic = rb.isKinematic;
                data.mass = rb.mass;
                data.centerOfMass_ws = new float4(rb.worldCenterOfMass, 0);
                data.linearVelocity = rb.velocity;
                data.angularVelocity = rb.angularVelocity;

                // Calculate the inverse intertia tensor (equivalent to mass in rotation)
                // It is a diagonal matrix because it is expressed in the space generated by the principal
                // axes of rotation (inertiaTensorRotation)
                var inertiaTensor = new float3(
                    (rb.constraints & RigidbodyConstraints.FreezeRotationX) != 0 ? 0 : 1 / rb.inertiaTensor.x,
                    (rb.constraints & RigidbodyConstraints.FreezeRotationY) != 0 ? 0 : 1 / rb.inertiaTensor.y,
                    (rb.constraints & RigidbodyConstraints.FreezeRotationZ) != 0 ? 0 : 1 / rb.inertiaTensor.z);

                // calculate full world space inertia matrix
                var rotationMat = float4x4.TRS(float3.zero, rb.rotation * rb.inertiaTensorRotation, new float3(1, 1, 1));
                data.inverseInertiaTensor = rotationMat * float4x4.Scale(inertiaTensor) * math.transpose(rotationMat);

                _rigidBodyData[i] = data;
            }

            occupancyDataInitMarker.End();
        }

        //TODO: Reduce the amount of areas in blobs. It is very easy to get to the worst case scenario with this improvement, anyway.
        internal void CalculateMinimumSharedAreaOfInteractors(out IntegerPair areaResolution, out int xOffset, out int zOffset)
        {
            calculateActiveAreaMarker.Begin();

            int sampleXMin = _resolution.x;
            int sampleZMin = _resolution.z;
            int sampleXMax = 0;
            int sampleZMax = 0;

            foreach (var interactor in _interactorsDetected)
            {
                Utils.GetColliderProjectedAreaOnSurface(this, interactor.NativeCollider, out int sampleMin_ls, out int sampleMax_ls);
                Utils.FromIndexToSampleIndices(in sampleMin_ls, in _resolution, out int outSampleXMin, out int outSampleZMin);
                Utils.FromIndexToSampleIndices(in sampleMax_ls, in _resolution, out int outSampleXMax, out int outSampleZMax);

                sampleXMin = math.min(sampleXMin, outSampleXMin);
                sampleZMin = math.min(sampleZMin, outSampleZMin);

                sampleXMax = math.max(sampleXMax, outSampleXMax);
                sampleZMax = math.max(sampleZMax, outSampleZMax);
            }

            xOffset = sampleXMin;
            zOffset = sampleZMin;
            areaResolution = new IntegerPair(math.max(sampleXMax - sampleXMin + 1, 0), math.max(sampleZMax - sampleZMin + 1, 0));

            calculateActiveAreaMarker.End();
        }

        private void CalculateOccupancy(ref JobHandle inOutHandle, IntegerPair areaResolution, int xOffset, int zOffset)
        {
            occupancyMarker.Begin();

            Utils.IncreaseAreaBy(_resolution, ref areaResolution, ref xOffset, ref zOffset, 1);

            var occupancyJob = new RaymarchOccupancy
            {
                _l2wTransformMatrix = _l2wTransformMatrix,
                hitsPerRigidbody = _hitsPerObject,
                interactionData = _interactionData,
                nMaxCellsPerInteractor = nMaxCellsPerInteractor,
                occupancy = _occupancy,
                resolution = _resolution,
                sampleSize_ls = _sampleSize_ls,
                detectionDepth_ls = detectionDepth,
                depthScale_l2w = _scale_l2w.y,
                depthScale_w2l = 1 / _scale_l2w.y,
                upDirection_ws = new float4(_transform.up, 0),
                buoyancyEnabled = buoyancy,
                areaResolution = areaResolution,
                offset = new IntegerPair(xOffset, zOffset),
                colliders = _interactorColliders,
                affectSurface = getAffectedByInteractors,
                colliderToRbIndices = _colliderToRbIndices,
                smoothedBorder_ws = math.max(_sampleSize_ws.x, _sampleSize_ws.y)
            };

            int nRaycasts = areaResolution.x * areaResolution.z;
            inOutHandle = occupancyJob.Schedule(nRaycasts, 16, inOutHandle);

            //TODO: Better or maybe best a simple Job that runs a loop?
            var finishOccupancyJob = new FinishOccupancyJob
            {
                hitsPerObject = _hitsPerObject,
                interactionData = _interactionData,
                nMaxCellsPerInteractor = nMaxCellsPerInteractor
            };
            inOutHandle = finishOccupancyJob.Schedule(_hitsPerObject.Length, 1, inOutHandle);

            occupancyMarker.End();
        }

        private void ApplyBuoyantForces(ref JobHandle inOutHandle)
        {
            if (_interactorsDetected.Count <= 0)
                return;

            surfaceAffectingInteractorsMarker.Begin();

            var job = new ApplyBuoyantForcesJob()
            {
                interactionData = _interactionData,
                rigidBodyData = _rigidBodyData.AsArray(),
                gradients = _gradients,
                resolution = _resolution,
                nMaxCellsPerInteractor = nMaxCellsPerInteractor,
                sampleSize_ls = _sampleSize_ls,
                upDirection_ws = new float4(_transform.up, 0),
                buoyancyDamping = buoyancyDamping,
                density = density,
                detectionDepth_ls = detectionDepth,
                scale_l2w = _scale_l2w,
                horizontalBuoyancy = horizontalBuoyancy,
                l2wTransformMatrix = _l2wTransformMatrix,
                fixedDeltaTime = Time.fixedDeltaTime,
                buoyantForces = _buoyantForces,
                colliderToRbIndices = _colliderToRbIndices,
                exportForces = exportForces
            };
            inOutHandle = job.Schedule(_interactorsDetected.Count, 32, inOutHandle);
            surfaceAffectingInteractorsMarker.End();
        }

        private void UpdateHeightsFromInteraction(ref JobHandle inOutHandle)
        {
            if (_interactorsDetected.Count == 0)
                return;

            interactorAffectingSurfaceMarker.Begin();
            var job = new OccupancyEffectJob()
            {
                resolution = _resolution,
                ghostResolution = _resolutionGhost,
                heights = _heights,
                occupancy = _occupancy,
                occupancyPrevious = _occupancyPrevious,
                fixedSamples = Descriptor.FixedGridRef,
                effectScale = effectScale,
                sleepThreshold = _heightChangeThreshold,
                isAwake = _awakeFromInteractor
            };
            inOutHandle = job.Schedule(_occupancy.Length, 64, inOutHandle);
            interactorAffectingSurfaceMarker.End();
        }

        private void UpdateGradientsAndNormals(ref JobHandle inOutHandle)
        {
            gradientsNormalsMarker.Begin();
            var job = new GradientsAndNormalsJob()
            {
                resolution = _resolution,
                ghostResolution = _resolutionGhost,
                gradients = _gradients,
                normals = _normals,
                heights = _heights,
                sampleSize = _sampleSize_ls
            };

            inOutHandle = job.Schedule(_gradients.Length, 128, inOutHandle);
            gradientsNormalsMarker.End();
        }

        private void UpdateAwakeStatus()
        {
            var wasAwoken = IsAwake;

            _awakeStatus = AwakeStatus.AWAKE;
            
            if (!IsAwakeDueToInteraction() && !IsAwakeDueToSimulation())
            {
                //TODO: use time instead of counting fixedUpdates()
                // Only sleep if no activation and with no cinetic energy for a while.
                if (_asleepCounter >= _asleepCounterLimit)
                    _awakeStatus = AwakeStatus.ASLEEP;
                else
                {
                    _asleepCounter++;
                    _awakeStatus = AwakeStatus.GETTING_ASLEEP;
                }
            }
            else
            {
                _asleepCounter--;
                _asleepCounter = _asleepCounter < 0 ? 0 : _asleepCounter;
            }

            _awakeFromInteractor[0] = 0;

            if (IsAwake != wasAwoken)
            {
                OnAwakeStatusChanged.Invoke();

                if (!IsAwake)
                    ResetSimulationData();
            }
        }

        private void VelocityBasedInteraction(ref JobHandle inOutHandle)
        {
            if (_interactorsDetected.Count == 0)
                return;

            interactorAffectingSurfaceMarker.Begin();

            ResetVelocityBasedDataForNewStep();

            CalculateMinimumSharedAreaOfInteractors(out IntegerPair areaResolution, out int xOffset, out int zOffset);

            var velsJob = new SampleVelocitiesJob()
            {
                velocities = _relativeVelocities,
                fixedSamples = Descriptor.FixedGridRef,
                colliders = _interactorColliders.AsArray(),
                interactorsData = _velocityData.AsArray(),
                l2wTransform = _l2wTransformMatrix,
                surfacePosition = _position,
                surfaceLinearVelocity = _linearVelocity,
                surfaceAngularVelocity = _angularVelocity,
                w2lRotation = _w2lRotation,
                resolution = _resolution,
                sampleSize_ls = _sampleSize_ls,
                areaResolution = areaResolution,
                xOffset = xOffset,
                zOffset = zOffset,
                interactorMaximumSpeedClamp = interactorMaximumSpeedClamp,
                interactorMinimumSpeedClamp = interactorMinimumSpeedClamp,
            };
            inOutHandle = velsJob.Schedule(areaResolution.x * areaResolution.z, 32, inOutHandle);

            Utils.IncreaseAreaBy(in _resolution, ref areaResolution, ref xOffset, ref zOffset, 1);

            var heightsJob = new HeightsFromVelocitiesJob()
            {
                heights = _heights,
                velocities = _relativeVelocities,
                fixedSamples = Descriptor.FixedGridRef,
                fixedDeltaTime = Time.fixedDeltaTime,
                horizontalPushScale = horizontalPushScale,
                verticalPushScale = verticalPushScale,
                resolution = _resolution,
                resolutionGhost = _resolutionGhost,
                areaResolution = areaResolution,
                xOffset = xOffset,
                zOffset = zOffset,
                isAwake = _awakeFromInteractor
            };
            inOutHandle = heightsJob.Schedule(areaResolution.x * areaResolution.z, 32, inOutHandle);

            interactorAffectingSurfaceMarker.End();
        }

        private void ResetVelocityBasedDataForNewStep()
        {
            unsafe
            {
                UnsafeUtility.MemClear(_relativeVelocities.GetUnsafePtr(), sizeof(float4) * _relativeVelocities.Length);
            }

            _velocityData.ResizeUninitialized(_interactorsDetected.Count);
            
            for (int i = 0; i < _interactorsDetected.Count; i++)
            {
                _interactorsDetected[i].UpdateVelocities();
                var velData = new VelocityData();
                velData.linearVelocity = _interactorsDetected[i].LinearVelocity;
                velData.angularVelocity = _interactorsDetected[i].AngularVelocity;
                velData.position = new float4(_interactorsDetected[i].transform.position, 0);
                _velocityData[i] = velData;
            }
        }

        private void ShowLimitWarnings()
        {
            if (interactionType != InteractionType.OccupancyBased)
                return;

            for (int i = 0; i < _interactorsDetected.Count; i++)
            {
                if (InteractionDataArray.HasReachedCellLimit(in _interactionData, nMaxCellsPerInteractor, i))
                    Utils.LogWarning(
                        string.Format("Setting Warning Log: The interactor {0} has reached the limit of allowed cells per interactor in the surface. Stop simulation and increase the value to the minimum possible without warnings in order to avoid simulation inesatibilities. Please visit the official website to see examples for each setting.", 
                        _interactorsDetected[i].name), gameObject
                        );
            }
        }

        /// <summary>
        /// Used by the debugger to enable the export of the buoyant forces generated in order to display them in the scene view
        /// WARNING: Disable by default. This is used only for debugging purposes
        /// </summary>
        internal void EnableExportBuoyantForces()
        {
            if (_buoyantForces != null && _buoyantForces.IsCreated)
                _buoyantForces.Dispose();

            _buoyantForces = new NativeArray<BuoyantForceData>(_resolution.x * _resolution.z, Allocator.Persistent);
            exportForces = true;
        }

        internal void DisableExportBuoyantForces()
        {
            exportForces = false;
            if (_buoyantForces != null && _buoyantForces.IsCreated)
                _buoyantForces.Dispose();
        }

        #endregion


#endif

    }

}