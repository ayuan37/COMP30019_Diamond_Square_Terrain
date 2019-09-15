using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVertices : MonoBehaviour
{
    private float size = 30f;
    private int nDivisions = 128;    
    Vector3[] vertices;
    Vector2[] uvs;
    int[] triangles;

    Mesh mesh;

    public SunScript pointLight;

    // Use this for initialization
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        vertices = new Vector3[(nDivisions + 1) * (nDivisions + 1)];

        // use this to make sure to centre the square
        float halfSize = size * 0.5f;
        float divisionSize = size / nDivisions;
        float y = Random.Range(-0.005f,0.005f);

        for (int z = 0; z < nDivisions + 1; z++) 
        {
            for (int x = 0; x < nDivisions + 1; x++)
            {
                // zth row, xth column
                vertices[x*(nDivisions + 1) + z] = 
                    new Vector3(-halfSize + x*divisionSize, y, halfSize - z*divisionSize);
            }    
        }
        MapUvs();
        BuildTriangles();
        DrawMesh();

        MeshCollider collider = this.gameObject.GetComponent<MeshCollider>();
        collider.sharedMesh = mesh;

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

    // Build triangles from the vertices to create realistic water
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

    // Map texture to the vertices
    void MapUvs(){
        uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x/nDivisions, vertices[i].z/nDivisions);
        }
    }

    // Draw the mesh of the water
    void DrawMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

}