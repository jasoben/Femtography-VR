using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WaveMaker;

public class MeshVertexTracker : MonoBehaviour
{
    Vector3[] vertices, highestVerts, lowestVerts;
    public WaveMakerSurface waveMakerSurface;
    Mesh mesh;

    List<int> nearbyVertices = new List<int>();
    bool regionDefined;

    public float zPos;
    public float bufferSize;
    public float zMovementAmount;
    public GameObject recalibratingText;

    Matrix4x4 meshConversionMatrix; // We use this to convert the mesh verts from local space to world space

    public float amplitudeScale;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckInit());
        StartCoroutine(FindHighestVerts());
    }

    // We need to wait until the mesh object is initialized on the Wavemaker object before we define our 
    // region.

    IEnumerator CheckInit()
    {
        while(true)
        {
            if (!waveMakerSurface.Initialized)
            {
                Debug.Log("Waiting");
                yield return new WaitForEndOfFrame();
            }
            else
            {
                Debug.Log("Initialized");
                mesh = waveMakerSurface.Mesh_;
                meshConversionMatrix = waveMakerSurface.transform.localToWorldMatrix;
                regionDefined = DefineRegion();
                yield break;
            }
        }
    }

    // This defines the region of the mesh that we are looking at to register vertices (we don't want 
    // to look at the whole mesh because it would be too expensive)
    
    bool DefineRegion()
    {
        zPos = transform.position.z;

        nearbyVertices.Clear();

        mesh = waveMakerSurface.Mesh_; // The mesh is constantly updated, so we need to keep it updated here

        Vector3 worldPositionOfMeshVert ;
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            worldPositionOfMeshVert = meshConversionMatrix.MultiplyPoint3x4(mesh.vertices[i]);

            if (Mathf.Abs(zPos - worldPositionOfMeshVert.z) < bufferSize) // the buffersize refers to the "depth" of the region
                // that we want to use when looking for the mesh vertices.
            {
                Debug.Log(worldPositionOfMeshVert);
                nearbyVertices.Add(i);
            }
        }

        vertices = mesh.vertices;
        highestVerts = new Vector3[vertices.Length];
        lowestVerts = new Vector3[vertices.Length];

        recalibratingText.SetActive(false);

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Mathf.Abs(transform.position.z) > Mathf.Abs(zPos) + .1f ||
        //    Mathf.Abs(transform.position.z) < Mathf.Abs(zPos) - .1f) // If it moves out of range, we redefine the region
        //    // we wish to examine
        if (Input.GetKeyDown(KeyCode.R))
        {
            recalibratingText.SetActive(true);
            zPos = transform.position.z;
            if (regionDefined)
                Invoke("DefineRegion", .1f);
        }

        // This will move the "screen" forward or backwards depending on which direction we want to go
        if (Input.GetKey(KeyCode.F))
        {
            transform.position += new Vector3(0, 0, zMovementAmount);
        }
        if (Input.GetKey(KeyCode.V))
        {
            transform.position -= new Vector3(0, 0, zMovementAmount);
            if (transform.position.z < 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            }
        }
    }

    //Search for the peak vertices
    IEnumerator FindHighestVerts()
    {
        while (true)
        {
            if (regionDefined)
            {
                foreach (int nearbyVertex in nearbyVertices)
                {
                    if (vertices[nearbyVertex].y > highestVerts[nearbyVertex].y)
                    {
                        highestVerts[nearbyVertex] = vertices[nearbyVertex];
                    }
                    //if (vertices[nearbyVertex].y < lowestVerts[nearbyVertex].y)
                    //{
                    //    lowestVerts[nearbyVertex] = vertices[nearbyVertex];
                    //}
                }

                yield return new WaitForSecondsRealtime(.5f);

                mesh = waveMakerSurface.Mesh_;
                vertices = mesh.vertices;
            } else
                yield return new WaitForEndOfFrame();
        }
    }
    private void OnDrawGizmos()
    {
        if (regionDefined)
        {
            mesh = waveMakerSurface.Mesh_;
            vertices = mesh.vertices;

            Vector3 worldPosVert;
            Vector3 worldPosHighestVert;
            foreach (int vertexNumber in nearbyVertices)
            {
                worldPosVert = meshConversionMatrix.MultiplyPoint3x4(vertices[vertexNumber]);
                worldPosHighestVert = Vector3.Scale(meshConversionMatrix.MultiplyPoint3x4(highestVerts[vertexNumber]), 
                    new Vector3(1,amplitudeScale,1));
                
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(worldPosVert, .2f);

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(worldPosHighestVert, .2f);

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(lowestVerts[vertexNumber], .2f);
            }
        }
    }

    private void OnValidate()
    {
        zPos = transform.position.z;
        if (regionDefined)
            DefineRegion();
    }
}
