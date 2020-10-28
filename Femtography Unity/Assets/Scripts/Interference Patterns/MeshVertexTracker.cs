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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckInit());
        StartCoroutine(FindHighestVerts());
    }

    IEnumerator CheckInit()
    {
        while(true)
        {
            if (!waveMakerSurface.Initialize())
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                mesh = waveMakerSurface.Mesh_;
                regionDefined = DefineRegion();
                yield break;
            }
        }
    }

    bool DefineRegion()
    {
        zPos = transform.position.z;

        nearbyVertices.Clear();

        mesh = waveMakerSurface.Mesh_; // The mesh is constantly updated, so we need to keep it updated here

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            if (Mathf.Abs(zPos - mesh.vertices[i].z) < bufferSize)
            {
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
                    if (vertices[nearbyVertex].y < lowestVerts[nearbyVertex].y)
                    {
                        lowestVerts[nearbyVertex] = vertices[nearbyVertex];
                    }
                }


                yield return new WaitForSecondsRealtime(.5f);

                mesh = waveMakerSurface.Mesh_;
                vertices = mesh.vertices;

            }
        }
    }
    private void OnDrawGizmos()
    {
        if (regionDefined)
        {
            mesh = waveMakerSurface.Mesh_;
            vertices = mesh.vertices;
            foreach (int vertexNumber in nearbyVertices)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(vertices[vertexNumber], .2f);

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(highestVerts[vertexNumber], .2f);

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
