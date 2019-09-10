using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVertices : MonoBehaviour
{
    private float size = 30f;
    private int nDivisions = 128;


    
    Vector3[] vertices;
    int[] triangles;
    Mesh mesh;

    // public Shader waterShader;
    public SunScript pointLight;

    // Use this for initialization
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        vertices = new Vector3[(nDivisions + 1) * (nDivisions + 1)];

        // use this to make sure to centre the square
        float halfSize = size * 0.5f;
        float divisionSize = size / nDivisions;

        for (int z = 0; z < nDivisions + 1; z++) 
        {
            for (int x = 0; x < nDivisions + 1; x++)
            {
                // zth row, xth column
                vertices[x*(nDivisions + 1) + z] = 
                    new Vector3(-halfSize + x*divisionSize, Random.Range(-0.001f,0.001f), halfSize - z*divisionSize);
            }
        }

        BuildTriangles();
        DrawMesh();
    }

    // Called each frame
    void Update()
    {
        // Get renderer component (in order to pass params to shader)
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        // Pass updated light positions to shader
        renderer.material.SetColor("_PointLightColor", this.pointLight.color);
        renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
    }

    void BuildTriangles ()
    {
        int vert = 0;
        int tris = 0;
        triangles = new int[nDivisions * nDivisions * 6];

        for (int z = 0; z < nDivisions; z++) 
        {
            for (int x = 0; x < nDivisions; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + nDivisions + 1;
                triangles[tris + 2] = vert + 1;

                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + nDivisions + 1;
                triangles[tris + 5] = vert + nDivisions + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void DrawMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

}