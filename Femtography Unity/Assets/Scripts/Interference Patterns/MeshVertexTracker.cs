using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WaveMaker;

public class MeshVertexTracker : MonoBehaviour
{
    Vector3[] vertices, highestVerts;
    WaveMakerSurface waveMakerSurface;
    Mesh mesh;

    List<int> nearbyVertices = new List<int>();
    bool regionDefined;

    public float zPos;
    public float bufferSize;
    // Start is called before the first frame update
    void Start()
    {
        waveMakerSurface = GetComponent<WaveMakerSurface>();
        StartCoroutine(CheckInit());
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
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            if (Mathf.Abs(zPos - mesh.vertices[i].z) < bufferSize)
            {
                nearbyVertices.Add(i);
            }
        }
        
        highestVerts = new Vector3[nearbyVertices.Count];

        for (int i = 0; i < nearbyVertices.Count; i++)
        {
            highestVerts[i] = vertices[nearbyVertices[i]];
        }

        return true;
    }
    // Update is called once per frame
    void Update()
    {
        
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
            }

            for (int i = 0; i < highestVerts.Length; i++)
            {
                if (highestVerts[i].y < vertices[nearbyVertices[i]].y)
                {
                    highestVerts[i] = vertices[nearbyVertices[i]];
                }
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(highestVerts[i], .2f);
            }
        }
    }

    private void OnValidate()
    {
        DefineRegion();
    }
}
