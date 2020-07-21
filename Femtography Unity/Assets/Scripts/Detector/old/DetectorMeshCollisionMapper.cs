using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorMeshCollisionMapper : MonoBehaviour
{
    public GameObject detector;
    public LayerMask rayMask;
    public Material[] materials = new Material[2];
    public Material deadMat, liveMat;
    Mesh hitMesh;
    List<int> hitTriangles = new List<int>();
    int[] triangles;

    public int radius;
    public int angle;
    // Start is called before the first frame update
    void Start()
    {
        materials[0] = deadMat;
        materials[1] = deadMat;
        detector = GameObject.FindGameObjectWithTag("detector");
        hitMesh = detector.GetComponent<MeshFilter>().sharedMesh;
        hitMesh.subMeshCount = 2;
        triangles = hitMesh.triangles;
    }

    // Update is called once per frame
    void Update()
    {

    }

    List<RaycastHit> FireRaysBasedOnDiameter(int radius)// We need to fire a group of rays to test for triangle indexes in the mesh collider
    {
        List<RaycastHit> hitPoints = new List<RaycastHit>();
        RaycastHit firstHitPoint;
        Physics.Raycast(transform.position, transform.forward, out firstHitPoint, 100, rayMask);// Fire from the center first
        hitPoints.Add(firstHitPoint);
        for (int i = 1; i < transform.lossyScale.x; i += radius)
        {
            for (int j = 0; j < 360; j += angle)
            {
                RaycastHit hitPoint;
                Vector3 startPos = transform.position 
                    + (Mathf.Cos(j) * i * transform.right.normalized) 
                    + (Mathf.Sin(j) * i * transform.up.normalized); // Pattern is a circle of "j" degrees at "i" radius

                Physics.Raycast(startPos, transform.forward, out hitPoint, 100, rayMask);
                hitPoints.Add(hitPoint);
            }
        }

        return hitPoints;
    }

    void HighlightTrianglesInMesh(List<RaycastHit> theseHits)
    {
        hitTriangles.Clear();
        foreach(RaycastHit thisHit in theseHits)
        {
            int triangle = thisHit.triangleIndex;
            //int otherTriangle;

            //if (triangle % 2 == 0)
            //    otherTriangle = triangle + 1;
            //else
            //    otherTriangle = triangle - 1;


            int[] newTriangles = {hitMesh.triangles[triangle * 3], hitMesh.triangles[triangle * 3 + 1], hitMesh.triangles[triangle * 3 + 2],
            /*hitMesh.triangles[otherTriangle * 3], hitMesh.triangles[otherTriangle * 3 + 1], hitMesh.triangles[otherTriangle * 3 + 2]*/};
            hitTriangles.AddRange(newTriangles);

        }

        hitMesh.SetTriangles(triangles, 0);
        hitMesh.SetTriangles(hitTriangles, 1);
        hitMesh.RecalculateNormals();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "detector")
        {
            Debug.Log("response");
            HighlightTrianglesInMesh(FireRaysBasedOnDiameter(radius));
            materials[1] = liveMat;
            detector.GetComponent<MeshRenderer>().materials = materials;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "detector")
        {
            materials[1] = deadMat;
            detector.GetComponent<MeshRenderer>().materials = materials;

        }    
    }
}
