using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestMeshCollision : MonoBehaviour
{
    public GameObject hemisphere;
    public LayerMask rayMask;
    public int triangle;
    public Vector3 thisVec;
    public Material[] materials = new Material[2];
    public Material deadMat, liveMat;
    Mesh hitMesh;
    int[] hitTriangles;
    // Start is called before the first frame update
    void Start()
    {
        materials[1] = deadMat;
    }

    // Update is called once per frame
    void Update()
    {
        hitMesh = hemisphere.GetComponent<MeshFilter>().sharedMesh;
        hitMesh.subMeshCount = 2;
        RaycastHit hitPoint;
        Physics.Raycast(transform.position, -transform.up, out hitPoint, 100, rayMask);

        triangle = hitPoint.triangleIndex;
        int otherTriangle;

        if (triangle % 2 == 0)
            otherTriangle = triangle + 1;
        else
            otherTriangle = triangle - 1;

        int[] triangles = hitMesh.triangles;

        hitTriangles = new int[] { hitMesh.triangles[triangle * 3], hitMesh.triangles[triangle * 3 + 1], hitMesh.triangles[triangle * 3 + 2],
        hitMesh.triangles[otherTriangle * 3], hitMesh.triangles[otherTriangle * 3 + 1], hitMesh.triangles[otherTriangle * 3 + 2]};

        hitMesh.SetTriangles(triangles, 0);
        hitMesh.SetTriangles(hitTriangles, 1);
        hitMesh.RecalculateNormals();

        //TODO duplicate for multiple rays in larger objects (proton)
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("response");
        materials[1] = liveMat;
        hemisphere.GetComponent<MeshRenderer>().materials = materials;
    }
    private void OnTriggerExit(Collider other)
    {
        materials[1] = deadMat;
        hemisphere.GetComponent<MeshRenderer>().materials = materials;
    }
}
