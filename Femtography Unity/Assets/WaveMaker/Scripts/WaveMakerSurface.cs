using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace WaveMaker
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(BoxCollider))]
    public class WaveMakerSurface : MonoBehaviour
    {
        /*************************************************/
        #region PROPERTIES

        /// <summary>
        /// Size on X axis of the plane in Unity units
        /// </summary>
        public float Width
        {
            get { return _width; }
            set
            {
                _width = Mathf.Clamp(value, 0.001f, float.MaxValue);
                Initialize();
            }
        }

        /// <summary>
        /// Size on Z axis of the plane in Unity units
        /// </summary>
        public float Depth
        {
            get { return _depth; }
            set
            {
                _depth = Mathf.Clamp(value, 0.001f, float.MaxValue);
                Initialize();
            }
        }

        /// <summary>
        /// Size along the X local axis of a grid cell.
        /// </summary>
        public float SampleSizeX { get { return _sampleSizeX; } }

        /// <summary>
        /// Size along the Z local axis of a grid cell
        /// </summary>
        public float SampleSizeZ { get { return _sampleSizeZ; } }

        /// <summary>
        /// Number of vertices in the mesh on the x axis
        /// </summary>
        public int ResolutionX { get { return _resolutionX; } }

        /// <summary>
        /// Number of vertices in the mesh on the z axis
        /// </summary>
        public int ResolutionZ { get { return _resolutionZ; } }

        /// <summary>
        /// Number of vertices in the mesh on the X axis , including ghost cells on the extremes for extra calculations
        /// </summary>
        public int ResolutionXGhost { get { return _resolutionXGhost; } }

        /// <summary>
        /// Number of vertices in the mesh on the Z axis , including ghost cells on the extremes for extra calculations
        /// </summary>
        public int ResolutionZGhost { get { return _resolutionZGhost; } }

        /// <summary>
        /// Whether the surface is awake or is asleep, so it is not updating values. This happens when it doesn't need to simulate anymore
        /// </summary>
        public bool IsAwake { get { return _isAwake; } }

        public MeshRenderer Renderer
        {
            get
            {
                if (_meshRenderer == null)
                    _meshRenderer = GetComponent<MeshRenderer>();
                return _meshRenderer;
            }
        }

        public MeshFilter MeshFilter
        {
            get
            {
                if (_meshFilter == null)
                    _meshFilter = GetComponent<MeshFilter>();
                return _meshFilter;
            }
        }

        #endregion

        /*************************************************/

        #region PUBLIC MEMBERS

        public WaveMakerDescriptor Descriptor;

        [Tooltip("More sub executions of fixedUpdate means less efficiency, but allows for more Propagation Speed. Keep the value as near as 1 as possible.")]
        [Min(1)] //NOTE: Min does not work on 2018. Fixed on OnValidate
        public int substeps = 1;

        [Tooltip("Higher damping makes waves have a short life")]
        [Range(0f, 10f)]
        public float damping = 3f;

        [Tooltip("How fast waves propagate on the surface. WARNING: High values can make it more unstable")]
        [Min(0f)] //NOTE: Min does not work on 2018. Fixed on OnValidate
        public float propagationSpeed = 6;

        [Tooltip("Scales the vertical velocity of objects interacting that generates vertical waves")]
        [Min(0f)] //NOTE: Min does not work on 2018. Fixed in OnValidate
        public float verticalPushScale = 1f;

        [Tooltip("Scales the horizontal velocity of objects interacting that generates side waves")]
        [Min(0f)] //NOTE: Min does not work on 2018. Fixed in OnValidate
        public float horizontalPushScale = 1f;

        [Tooltip("Make waves smoother with this parameter")]
        [Range(0, 1)]
        public float waveSmoothness = 0;

        [Tooltip("Clamp the speed of any interactor that affects this surface to this value to avoid too fast objects to affect the surface too much")]
        [Min(0)] //NOTE: Min does not work on 2018. Fixed in OnValidate
        public float interactorSpeedClamp = 100f;

        [Tooltip("How much over the plane the collision will be tested")]
        [HideInInspector] [Range(0, 10)]
        public float collisionDetectionHeight = 0.1f;

        [Tooltip("Show more information on what is happening to this component")]
        public bool showLogMessages = false;

        [Tooltip("Red rays on intersection with interactors, yellow rays for a test")]
        public bool drawInteractionDebugRays = false;

        [Tooltip("Draw the mesh grid generated to test resolution and size")]
        public bool drawGrid = false;

        #endregion

        /*************************************************/

        #region PRIVATE MEMBERS

        bool _initialized = false;

        MeshRenderer _meshRenderer;
        MeshFilter _meshFilter;
        BoxCollider _collider;

        Mesh mesh;
        Vector3[] vertices;

        /// <summary>
        /// Contains the mesh grid heights, it has one more column on each extreme for ghost cells for open boundaries
        /// </summary>
        float[,] heights;
        float[,] velocities;
        float[,] accelerations;

        /// <summary>
        /// The reference to the fixed grid array on the descriptor. This is done due to efficiency cost of accessing via method
        /// </summary>
        bool[] FixedGridRef;

        /// <summary>
        /// Represents the number of vertices along local X axis
        /// </summary>
        [System.NonSerialized]
        [HideInInspector]
        int _resolutionX = 10;

        /// <summary>
        /// Represents the number of vertices along local Z axis
        /// </summary>
        [System.NonSerialized]
        [HideInInspector]
        int _resolutionZ = 10;

        /// <summary>
        /// Represents the number of vertices along local X axis including ghost cells
        /// </summary>
        [System.NonSerialized]
        [HideInInspector]
        int _resolutionXGhost = 12;

        /// <summary>
        /// Represents the number of vertices along local Z axis including ghost cells
        /// </summary>
        [System.NonSerialized]
        [HideInInspector]
        int _resolutionZGhost = 12;

        /// <summary>
        /// Size on X axis of the plane in Unity units
        /// </summary>
        [SerializeField]
        [HideInInspector]
        float _width = 10;

        /// <summary>
        /// Size on Z axis of the plane in Unity units
        /// </summary>
        [SerializeField]
        [HideInInspector]
        float _depth = 10;

        /// <summary>
        /// Size along the X local axis of a grid cell
        /// </summary>
        [SerializeField]
        float _sampleSizeX;

        /// <summary>
        /// Size along the Z local axis of a grid cell
        /// </summary>
        [SerializeField]
        float _sampleSizeZ;

        // At which minimum speed to a side (in local x or z coords) makes the wave propagate;
        private float speedThreshold = 0.001f;

        Vector3 _linearVelocity = Vector3.zero;
        Vector3 _angularVelocity = Vector3.zero;
        Vector3 _lastPosition = Vector3.zero;
        Quaternion _lastRotation = Quaternion.identity;

        List<WaveMakerInteractor> _interactorsDetected = new List<WaveMakerInteractor>();
        List<Collider> _interactorsDetectedColliders = new List<Collider>();

        //SLEEP params: How much this surface is still moving right now. Helps deciding when to turn it off
        bool _isAwake = false;
        float _cineticEnergy = 0;
        float _sleepThreshold = 0.001f;

        #endregion

        /*************************************************/

        #region DEFAULT METHODS

        private void Start()
        {
            if (!Application.isPlaying)
                return;

            if (Descriptor == null || !Descriptor.IsInitialized)
            {
                Debug.LogError("WaveMaker - (" + gameObject.name + ") cannot be initialized. No Descriptor is attached in that gameobject or could not be initialized.");
                _initialized = false;
                return;
            }

            Initialize();
        }

        void FixedUpdate()
        {
            if (!_initialized)
                return;

            UpdateAwakeStatus();
            UpdateCollidingObjectsInteraction();

            // Cinetic energy or action by any interactor can wake up the surface
            if (!_isAwake)
                return;

            // Repeat the execution
            for (int i = 0; i < substeps; i++)
            {
                Profiler.BeginSample("WaveMaker Substep");
                UpdateDataGrid();
                Profiler.EndSample();
            }
        }

        private void Update()
        {
            if (!_initialized)
                return;

            if (!_isAwake)
                return;

            UpdateMesh();
        }

        private void OnDestroy()
        {
            Uninitialize();
        }

        private void OnCollisionEnter(Collision collision)
        {
            DetectCollisionStart(collision.collider);
        }

        private void OnTriggerEnter(Collider other)
        {
            DetectCollisionStart(other);
        }

        private void DetectCollisionStart(Collider other)
        {
            var comp = other.GetComponent<WaveMakerInteractor>();
            if (comp == null)
                return;

            _interactorsDetectedColliders.Add(comp.GetComponent<Collider>());
            _interactorsDetected.Add(comp);
            comp.UpdateVelocities();
            UpdateVelocities();

            if (showLogMessages)
                Debug.Log("WaveMaker - Interactor detected: " + comp.gameObject.name + " . Interactors detected now : " + _interactorsDetected.Count);
        }


        private void OnCollisionExit(Collision collision)
        {
            DetectCollisionEnd(collision.collider);
        }

        private void OnTriggerExit(Collider other)
        {
            DetectCollisionEnd(other);
        }

        private void DetectCollisionEnd(Collider other)
        {
            var comp = other.GetComponent<WaveMakerInteractor>();
            if (comp == null)
                return;

            int id = _interactorsDetected.IndexOf(comp);
            _interactorsDetected.RemoveAt(id);
            _interactorsDetectedColliders.RemoveAt(id);

            if (showLogMessages)
                Debug.Log("WaveMaker - Stop detecting interactor : " + comp.gameObject.name + " . Interactors detected now : " + _interactorsDetected.Count);

        }

        //TODO: Setting a Min using an Attribute does not work on 2018.
        private void OnValidate()
        {
            if (substeps < 1)
                substeps = 1;

            if (propagationSpeed < 0)
                propagationSpeed = 0;

            if (verticalPushScale < 0)
                verticalPushScale = 0;

            if (horizontalPushScale < 0)
                horizontalPushScale = 0;

            if (interactorSpeedClamp < 0)
                interactorSpeedClamp = 0;
        }

        #endregion

        /***************************************/

        #region PRIVATE METHODS

        /// <summary>
        /// Checks and updates if this surface is awake or can be sleeping
        /// </summary>
        private void UpdateAwakeStatus()
        {
            _isAwake = _cineticEnergy > _sleepThreshold;
        }

        /// <summary>
        /// Check if any interactor is colliding and modifying this WaveMaker object
        /// </summary>
        private void UpdateCollidingObjectsInteraction()
        {
            if (_interactorsDetected.Count <= 0)
                return;

            UpdateVelocities();

            float fixedDeltaTime = Time.fixedDeltaTime;
            Matrix4x4 localToWorldMat = transform.localToWorldMatrix;
            Matrix4x4 worldToLocalMat = transform.worldToLocalMatrix;
            Quaternion rotation_w2l_surface = worldToLocalMat.rotation;

            for (int i = 0; i < _interactorsDetected.Count; i++)
            {
                var interactor = _interactorsDetected[i];
                interactor.UpdateVelocities();

                Matrix4x4 worldToLocalMatrixInteractor = interactor.transform.worldToLocalMatrix;

                var interactorCollider = _interactorsDetectedColliders[i];
                Bounds bounds_ls = WaveMakerUtils.TransformBounds(interactorCollider.bounds, transform.worldToLocalMatrix);

                GetNearestSample(bounds_ls.min.x, bounds_ls.min.z, out int sampleXMin_ls, out int sampleZMin_ls);
                GetNearestSample(bounds_ls.max.x, bounds_ls.max.z, out int sampleXMax_ls, out int sampleZMax_ls);

                // Fix floor applied by GetNearestSample
                if (bounds_ls.min.x > 0) sampleXMin_ls++;
                if (bounds_ls.min.z > 0) sampleZMin_ls++;

                Vector3 interactorPos = interactor.CenterOfMass;
                Vector3 surfacePos = transform.position;
                Vector3 samplePos_ws, samplePos_ls_Interactor, samplePos_ls_Surface;

                // Check collision of this object with the area it is affecting
                for (int sampleX = sampleXMin_ls; sampleX <= sampleXMax_ls; sampleX++)
                    for (int sampleZ = sampleZMin_ls; sampleZ <= sampleZMax_ls; sampleZ++)
                    {
                        if (FixedGridRef[sampleZ * _resolutionX + sampleX])
                            continue;

                        samplePos_ls_Surface = GetPositionFromSample(sampleX, sampleZ, false);
                        samplePos_ws = localToWorldMat.MultiplyPoint(samplePos_ls_Surface);

                        if (WaveMakerUtils.IsPointInsideCollider(interactorCollider, samplePos_ws))
                        {
                            samplePos_ls_Interactor = worldToLocalMatrixInteractor.MultiplyPoint(samplePos_ws);

                            // Change height using velocity of the colliding poing in this cell
                            Vector3 velocityAtPoint_interactor = WaveMakerUtils.VelocityAtPoint(samplePos_ws, interactorPos, interactor.AngularVelocity, interactor.LinearVelocity);
                            Vector3 velocityAtPoint_surface = WaveMakerUtils.VelocityAtPoint(samplePos_ws, surfacePos, _angularVelocity, _linearVelocity);

                            // Apply only rotation, not scale
                            Vector3 speed_ls = rotation_w2l_surface * (velocityAtPoint_interactor - velocityAtPoint_surface);

                            float speedMag = speed_ls.magnitude;

                            if (speedMag < _sleepThreshold)
                                continue;

                            if (speedMag > interactorSpeedClamp)
                            {
                                speed_ls.Normalize();
                                speed_ls *= interactorSpeedClamp;
                            }

                            SetHeight_FullCheck(sampleX, sampleZ, speed_ls.y * verticalPushScale * fixedDeltaTime, true);

                            // Propagate height to next cell depending on direction of the movement of each colliding point in this cell
                            // NOTE: Done by hand instead of using Mathf to improve efficiency
                            float speedXabs = speed_ls.x > 0 ? speed_ls.x : -speed_ls.x;
                            float speedZabs = speed_ls.z > 0 ? speed_ls.z : -speed_ls.z;
                            int offX = speed_ls.x > 0 ? 1 : -1;
                            int offZ = speed_ls.z > 0 ? 1 : -1;

                            //TODO: Necessary to scale by fixed delta for the threshold?
                            speed_ls.y = 0;
                            offX = speedXabs < speedThreshold * fixedDeltaTime ? 0 : offX;
                            offZ = speedZabs < speedThreshold * fixedDeltaTime ? 0 : offZ;

                            if (offX != 0 || offZ != 0)
                            {
                                int frontX = sampleX + offX;
                                int frontZ = sampleZ + offZ;

                                // Grow front
                                if (frontX >= 0 && frontX < _resolutionX && frontZ >= 0 && frontZ < _resolutionZ)
                                    if (!FixedGridRef[frontX * _resolutionX + frontZ])
                                        SetHeight_FullCheck(frontX, frontZ, speed_ls.magnitude * horizontalPushScale * fixedDeltaTime, true);

                                // Reduce back
                                int backX = sampleX - offX;
                                int backZ = sampleZ - offZ;

                                if (backX >= 0 && backX < _resolutionX && backZ >= 0 && backZ < _resolutionZ)
                                    if (!FixedGridRef[backX * _resolutionX + backZ])
                                        SetHeight_FullCheck(backX, backZ, -speed_ls.magnitude * horizontalPushScale * fixedDeltaTime, true);
                            }
                        }
                    }
            }
        }

        /// <summary>
        /// Generate internal data grids
        /// </summary>
        private void GenerateDataGrids()
        {
            // Heights has an extra ghost column for open boundaries
            heights = new float[_resolutionXGhost, _resolutionZGhost];
            velocities = new float[_resolutionX, _resolutionZ];
            accelerations = new float[_resolutionX, _resolutionZ];

            for (int x = 0; x < _resolutionXGhost; ++x)
                for (int z = 0; z < _resolutionZGhost; ++z)
                {
                    heights[x, z] = 0;

                    if (x < _resolutionX && z < _resolutionZ)
                    {
                        velocities[x, z] = 0;
                        accelerations[x, z] = 0;
                    }
                }
        }

        /// <summary>
        /// Generate visual mesh
        /// </summary>
        private void GenerateMesh()
        {
            if (_meshFilter == null)
                return;

            DestroyImmediate(mesh);
            mesh = new Mesh();

            int nCellsX = _resolutionX - 1;
            int nCellsZ = _resolutionZ - 1;

            // 10 resolution means 10 vertices = 10 samples and 9 mesh cells on that axis
            int nElems = _resolutionX * _resolutionZ;

            vertices = new Vector3[nElems];
            Vector3[] normals = new Vector3[nElems];
            Vector2[] uvs = new Vector2[nElems];

            // Two triangles between each pair of vertices.
            // 3 vertices each triangle. 
            // e.g: Resolution 4x4 would have 3x3 cells. 9 squares with 2 triangles each, with 3 vertices each
            int[] triangles = new int[2 * 3 * nCellsX * nCellsZ];

            int p0Index, p1Index, p2Index, p3Index;

            float uSection = 1.0f / nCellsX;
            float vSection = 1.0f / nCellsZ;
            _sampleSizeX = Width / nCellsX;
            _sampleSizeZ = Depth / nCellsZ;

            int triangleCount = 0;

            for (int z = 0; z < _resolutionZ; ++z)
                for (int x = 0; x < _resolutionX; ++x)
                {
                    p0Index = z * _resolutionX + x;

                    // Generate the new array data for this point
                    vertices[p0Index] = new Vector3(x * _sampleSizeX, 0, z * _sampleSizeZ);
                    normals[p0Index] = Vector3.up;
                    uvs[p0Index] = new Vector2(x * uSection, z * vSection);

                    // Generate triangles but not on the extreme sides
                    if (x != _resolutionX - 1 && z != _resolutionZ - 1)
                    {
                        // Calculate indices of this grid ( 2 triangles )
                        p1Index = p0Index + 1;
                        p2Index = p0Index + _resolutionX;
                        p3Index = p2Index + 1;

                        //    Z
                        //    |
                        //    |
                        //    p2 -- p3
                        //    |  /  |
                        //    p0 -- p1 --> X


                        /// 0 - 3 - 1
                        triangles[triangleCount++] = p0Index;
                        triangles[triangleCount++] = p3Index;
                        triangles[triangleCount++] = p1Index;

                        //  0 - 2 - 3
                        triangles[triangleCount++] = p0Index;
                        triangles[triangleCount++] = p2Index;
                        triangles[triangleCount++] = p3Index;
                    }
                }

            // Create the mesh and assign
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;

            // Triangles must be created afterwards
            mesh.triangles = triangles;
            _meshFilter.sharedMesh = mesh;

            //Generate heights in mesh
            UpdateMesh();
        }

        /// <summary>
        /// Simulates the heights on each sample effectively simulating the wave interaction. Should be executed every physics step.
        /// </summary>
        private void UpdateDataGrid()
        {
            if (!Application.isPlaying)
                return;

            _cineticEnergy = 0;

            float precalculation = propagationSpeed * propagationSpeed / (_sampleSizeX * _sampleSizeZ);
            float fixedDeltaTime = Time.fixedDeltaTime / substeps;
            float maxOffset = (1 - waveSmoothness) * Mathf.Min(SampleSizeX, SampleSizeZ);
            int sampleX, sampleZ;
            float nextPos = propagationSpeed * fixedDeltaTime;

            // Write on auxGrid. Skip the ghost extremes. heights have 2+ rows
            for (sampleX = 1; sampleX <= _resolutionX; sampleX++)
                for (sampleZ = 1; sampleZ <= _resolutionZ; sampleZ++)
                {
                    float averageNeighbourHeight = (heights[sampleX-1, sampleZ] + heights[sampleX+1, sampleZ] + heights[sampleX, sampleZ-1] + heights[sampleX, sampleZ+1]) / 4.0f;
                    accelerations[sampleX-1, sampleZ-1] = averageNeighbourHeight - heights[sampleX, sampleZ];
                }

            // Write on main grid what was calculated on auxgrid
            for (int ghostsampleX = 0; ghostsampleX < _resolutionXGhost; ghostsampleX++)
                for (int ghostsampleZ = 0; ghostsampleZ < _resolutionZGhost; ghostsampleZ++)
                {
                    // For normal cells
                    if (ghostsampleX > 0 && ghostsampleZ > 0 && ghostsampleX <= _resolutionX && ghostsampleZ <= _resolutionZ)
                    {
                        sampleX = ghostsampleX - 1;
                        sampleZ = ghostsampleZ - 1;

                        float acceleration = accelerations[sampleX, sampleZ];

                        // Smooth heights
                        float heightCorrection = 0;
                        if (acceleration > maxOffset)
                            heightCorrection += acceleration - maxOffset;
                        if (acceleration < -maxOffset)
                            heightCorrection += acceleration + maxOffset;
                        acceleration -= heightCorrection;

                        // Time * ( Acceleration - Damping Acceleration )
                        velocities[sampleX, sampleZ] += fixedDeltaTime * (precalculation * acceleration - velocities[sampleX, sampleZ] * damping);

                        // 1/2 vel*vel * mass. Consider mass to be 1
                        _cineticEnergy += velocities[sampleX, sampleZ] * velocities[sampleX, sampleZ] * 0.5f;

                        SetHeight_FullCheck(sampleX, sampleZ, fixedDeltaTime * velocities[sampleX, sampleZ] + heightCorrection, true);
                    }
                    
                    // For ghost cells on extremes. Corners are not used, are not really meaningful for this calculation
                    else
                    {
                        int neighbourX = ghostsampleX;
                        int neighbourZ = ghostsampleZ;
                        float sampleSize = _sampleSizeX;

                        if (ghostsampleX == 0) neighbourX++;
                        else if (ghostsampleX == _resolutionX + 1) neighbourX--;

                        if (ghostsampleZ == 0) { neighbourZ++; sampleSize = _sampleSizeZ; }
                        else if (ghostsampleZ == _resolutionZ + 1) {neighbourZ--; sampleSize = _sampleSizeZ;}

                        heights[ghostsampleX, ghostsampleZ] = (nextPos * heights[neighbourX, neighbourZ] + heights[ghostsampleX, ghostsampleZ] * sampleSize) / (sampleSize + nextPos);
                    }                    
                }
        }

        /// <summary>
        /// Calculate linear and angular velocity of this surface. This will be used to check relative movement between this surface and interactors.
        /// </summary>
        private void UpdateVelocities()
        {
            _linearVelocity = (transform.position - _lastPosition) / Time.fixedDeltaTime;
            _lastPosition = transform.position;

            _angularVelocity = WaveMakerUtils.GetAngularVelocity(_lastRotation, transform.rotation);
            _lastRotation = transform.rotation;
        }
        
        /// <summary>
        /// Copies calculated heights into mesh. This can happen just when drawing (Update)
        /// </summary>
        private void UpdateMesh()
        {
            if (!Application.isPlaying || mesh == null)
                return;

            //TODO: Copy or asignation? if copy... needed? Expensive. vertices = mesh.vertices;
            mesh.vertices = vertices;
            mesh.RecalculateNormals();

            //NOTE: mesh.RecalculateTangents(); is not called. It makes paint time multiply by 10 and is removed to prevent from activating it by error. 
            // Instead calculate your tangents in your shader cross(cross(currentTangent,normal), normal)
        }

        #endregion

        /**************************************************************/

        #region PUBLIC METHODS
            
        /// <summary>
        /// Generates the component data: gathers resolution data from the descriptor and generates the mesh and internal data, collider data...etc
        /// </summary>
        /// <returns>false if it couldn't initialize</returns>
        public bool Initialize()
        {
            if (Descriptor == null)
            {
                Debug.LogError("WaveMaker - (" + gameObject.name + ") no descriptor attached: Cannot initialize");
                _initialized = false;
                return false;
            }

            FixedGridRef = Descriptor.FixedGridRef;

            if (showLogMessages)
                Debug.Log("WaveMaker - (" + gameObject.name + ") initializing.");

            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _collider = GetComponent<BoxCollider>();

            if (GetComponents<Collider>().Length > 1)
            {
                Debug.LogWarning("WaveMaker - (" + gameObject.name + ") There must be only one BoxCollider. Disabling the rest");
                foreach (var coll in GetComponents<Collider>())
                    coll.enabled = false;

                _collider.enabled = true;
            }

            _resolutionX = Descriptor.ResolutionX;
            _resolutionZ = Descriptor.ResolutionZ;
            _resolutionXGhost = _resolutionX + 2;
            _resolutionZGhost = _resolutionZ + 2;
            _lastPosition = transform.position;
            _lastRotation = transform.rotation;

            GenerateDataGrids();
            GenerateMesh();

            _collider.center = new Vector3(_width / 2.0f, 0, _depth / 2.0f);
            _collider.size = new Vector3(_width, 0.01f, _depth);

            _initialized = true;
            return true;
        }

        /// <summary>
        /// Call this if you want to initialize again only if the resolution in the descriptor changed before.
        /// </summary>
        /// <returns>True if it could initialize</returns>
        public bool InitializeIfDescriptorChanged()
        {
            if (Descriptor == null)
                return false;

            if (_resolutionX != Descriptor.ResolutionX || _resolutionZ != Descriptor.ResolutionZ)
                return Initialize();

            return true;
        }

        /// <summary>
        /// Get the current height (on local space) of the given sample/cell. Extremes ghost cells are ignored
        /// </summary>
        /// <param name="sampleX">from 0 to resolution - 1. Not checked for efficiency reasons</param>
        /// <param name="sampleZ">from 0 to resolution - 1. Not checked for efficiency reasons</param>
        public float GetHeight(int sampleX, int sampleZ)
        {
            return heights[sampleX+1, sampleZ+1];
        }

        /// <summary>
        /// Get the current height (on local space) or the given sample/cell. Extreme ghost cells are considered so 1,1 will be the first mesh height
        /// </summary>
        /// <param name="sampleX">from 0 to resolution - 1. Not checked for efficiency reasons</param>
        /// <param name="sampleZ">from 0 to resolution - 1. Not checked for efficiency reasons</param>
        public float GetHeightIncludeGhostCells(int sampleX, int sampleZ)
        {
            return heights[sampleX, sampleZ];
        }
        
        /// <summary>
        /// Set height of a sample directly on non-ghost cells. This doesn't perform any check and doesn't awake the surface.
        /// Instead use the FULL CHECK version of this function
        /// </summary>
        /// <param name="offset">true if we are passing an offset instead of a given height</param>
        /// <returns>The final height set after this operation</returns>
        public float SetHeight(int sampleX, int sampleZ, float height, bool offset = false)
        {
            // Skip ghosts
            sampleX++;
            sampleZ++;

            if (offset)
                heights[sampleX, sampleZ] += height;
            else
                heights[sampleX, sampleZ] = height;

            vertices[(sampleZ-1) * _resolutionX + (sampleX-1)].y = heights[sampleX, sampleZ];

            return heights[sampleX, sampleZ];
        }

        /// <summary>
        /// Rise grid scaled by the parameters and with extreme check. Extremes ghost cells are ignored
        /// Ignores any change on a fixed cell as well as out of bounds.
        /// </summary>
        /// <param name="sampleX">from 0 to resolution - 1</param>
        /// <param name="sampleZ">from 0 to resolution - 1</param>
        /// <param name="offset">true if we are passing an offset from the current height instead of a height</param>
        public void SetHeight_FullCheck(int sampleX, int sampleZ, float height, bool offset = false)
        {
            // ignore out of bounds
            if (sampleX >= _resolutionX || sampleZ >= _resolutionZ || sampleX < 0 || sampleZ < 0)
                return;

            // ignore fixed
            if (FixedGridRef[sampleZ * _resolutionX + sampleX])
                return;

            height = SetHeight(sampleX, sampleZ, height, offset);

            // Wake surface if change is enough to produce visible waves
            if (height > _sleepThreshold || height < _sleepThreshold)
                _isAwake = true;
        }

        /// <summary>
        /// Get position in local or world space of the current sample/cell center. Y coordinate will be 0 by default.
        /// </summary>
        /// <param name="worldSpace">True if we want the position given in world space. Needs additional computation, by default it is local space</param>
        /// <param name="includeHeight">The y coordinate will be also gathered. This needs additional computation. by default not included.</param>
        /// <param name="includeGhostCells">Ghost cells are added to the extremes to perform additional calculations. by default they are not included</param>
        /// <exception cref="AgumentException">If index out of range for the selected parameters</exception>
        public Vector3 GetPositionFromSample(int sampleX, int sampleZ, bool worldSpace = false, bool includeHeight = false, bool includeGhostCells = false)
        {
            if (!includeGhostCells && (sampleX >= _resolutionX || sampleZ >= _resolutionZ || sampleX < 0 || sampleZ < 0))
                throw new System.ArgumentException("Sample index is out of range : " + sampleX + " - " + sampleZ);

            if (includeGhostCells && (sampleX >= ResolutionXGhost || sampleZ >= ResolutionZGhost || sampleX < 0 || sampleZ < 0))
                throw new System.ArgumentException("Sample index is out of range (ghost cells) : " + sampleX + " - " + sampleZ);

            Vector3 pos = new Vector3(sampleX * _sampleSizeX, 0, sampleZ * _sampleSizeZ);
            
            if (includeGhostCells)
            {
                pos.x -= _sampleSizeX;
                pos.z -= _sampleSizeZ;

                if (includeHeight)
                    pos.y = GetHeightIncludeGhostCells(sampleX, sampleZ);
            }
            else if (includeHeight)
                pos.y = GetHeight(sampleX, sampleZ);

            if (worldSpace)
                pos = transform.TransformPoint(pos);

            return pos;
        }

        /// <summary>It returns the cell/sample coordinates for the given position in local space. 
        /// If the position is away from the bounding box, it will return the nearest one anyway.</summary>
        /// <param name="posX">A position in X local space</param>
        /// <param name="posZ">A position in Z local space</param>
        public void GetNearestSample(float posX, float posZ, out int sampleX, out int sampleZ)
        {
            if (posX < 0) posX = 0;
            else if (posX > _width) posX = _width;
            sampleX = Mathf.FloorToInt(posX / _sampleSizeX);

            if (posZ < 0) posZ = 0;
            else if (posZ > _depth) posZ = _depth;
            sampleZ = Mathf.FloorToInt(posZ / _sampleSizeZ);
        }

        /// <summary>
        /// Add fixed cells to the current descriptor data with the collisions with objects of the given layer.
        /// The Collision Detection Height is used as a threshold of collision detection.
        /// </summary>
        public void FixCollisions(int layer)
        {
            Quaternion rotation = transform.rotation;
            Vector3 halfExtents = new Vector3(_sampleSizeX/2.0f, collisionDetectionHeight * 2 , _sampleSizeZ/2.0f);
            LayerMask layerMask = 1 << layer;
            bool isSameLayer = layer == gameObject.layer;

            // For each sample on the patch
            for (int x = 0; x < _resolutionX; x++)
                for (int z = 0; z < _resolutionZ; z++)
                {
                    // Create a small box around the sample to collide with objects
                    Vector3 center = GetPositionFromSample(x, z, true);
                    if (Physics.OverlapBox(center, halfExtents, rotation, layerMask).Length > (isSameLayer? 1 : 0))
                        Descriptor.SetFixed(x, z, true);
                }
        }

        /// <summary>
        /// Using the current parameters, calculate if the surface should be stable or not. Unstability means the surface effect doesn't fade but grows.
        /// </summary>
        /// <returns>True if it should be stable.</returns>
        public bool CheckStabilityCondition()
        {
            return (Time.fixedDeltaTime/substeps) < Mathf.Min(_sampleSizeX, _sampleSizeZ) / propagationSpeed;
        }

        /// <summary>
        /// Deletes information stored on this object
        /// </summary>
        public void Uninitialize()
        {
            // TODO: Returns a warning during OnValidate, is this necessary? I need to destroy this
            if (_meshFilter != null)
                _meshFilter.sharedMesh = null;

            if (mesh != null)
                DestroyImmediate(mesh);

            _initialized = false;

            if (showLogMessages)
                Debug.Log("WaveMaker - (" + gameObject.name + ") uninitialized");
        }

        #endregion

    }

}