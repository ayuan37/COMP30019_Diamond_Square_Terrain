using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    public int size = 10;
    Vector3[] vertices;
    int vertexCount = 0;
    int[] triangles;
    Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        DiamondSquare();
        DrawMesh();
    }

    void DiamondSquare()
    {
        vertices = new Vector3[(size + 1) * (size + 1)];

        for (int z = 0; z < size + 1; z++) 
        {
            for (int x = 0; x < size + 1; x++)
            {
                vertices[vertexCount] = new Vector3(x, 0, z);
                vertexCount++;
                
            }
        }

        BuildTriangles();

    }

    void BuildTriangles ()
    {
        int vert = 0;
        int tris = 0;
        triangles = new int[size * size * 6];

        for (int z = 0; z<size; z++) 
        {
            for (int x = 0; x<size; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + size + 1;
                triangles[tris + 2] = vert + 1;

                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + size + 1;
                triangles[tris + 5] = vert + size + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    // taken from Brackeys :https://www.youtube.com/watch?v=64NblGkAabk
    void DrawMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
