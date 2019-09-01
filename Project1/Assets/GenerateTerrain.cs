﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Guided by Ather Omar: https:// https://www.youtube.com/watch?v=1HV8GbFnCik
public class GenerateTerrain : MonoBehaviour
{
    public float size = 30f;
    public int nDivisions = 64;
    public float maxHeight = 5f;

    Vector3[] vertices;
    int[] triangles;
    Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateVertices();
        DrawMesh();
    }

    void GenerateVertices()
    {
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
                    new Vector3(-halfSize + x*divisionSize, 0, halfSize - z*divisionSize);
            }
        }

        BuildTriangles();

        // init corners' heights
        vertices[0].y = Random.Range(0, maxHeight);
        vertices[nDivisions].y = Random.Range(0, maxHeight);
        vertices[vertices.Length - 1 - nDivisions].y = Random.Range(-maxHeight, maxHeight);
        vertices[vertices.Length - 1].y = Random.Range(-maxHeight, maxHeight);


        int niters = (int)Mathf.Log(nDivisions, 2);
        int nSquares = 1;
        int squareSize = nDivisions;
        for (int i = 0; i < niters; i++) {
            int row = 0;

            for (int j = 0; j < nSquares; j++) {
                int col = 0;

                for (int k = 0; k < nSquares; k++) {
                    DiamondSquare(row, col, squareSize);
                    col += squareSize;
                }
                row += squareSize;
            }
            nSquares *= 2;
            squareSize /= 2;
            maxHeight *= 0.5f;
        }
    }

    // TODO: optimise this to build triangles in the nested forloop above
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

    void DiamondSquare(int row, int col, int squareSize)
    {
        int halfSize = (int)(squareSize * 0.5f);
        int topLeft = row * (nDivisions + 1) + col;
        int botLeft = (row + squareSize) * (nDivisions + 1) + col;
        int mid = (int)(row + halfSize) * (nDivisions + 1) + (int)(col + halfSize);

        // Diamond
        vertices[mid].y = (vertices[topLeft].y + vertices[topLeft + squareSize].y
                          + vertices[botLeft].y + vertices[botLeft + squareSize].y) * 0.25f +
                                                                                 Random.Range(-maxHeight, maxHeight);
        // Square
        vertices[topLeft + halfSize].y = (vertices[topLeft].y + vertices[topLeft + squareSize].y + vertices[mid].y)/3 + Random.Range(-maxHeight, maxHeight);
        vertices[mid - halfSize].y = (vertices[topLeft].y + vertices[botLeft].y + vertices[mid].y)/3 + Random.Range(-maxHeight, maxHeight);
        vertices[mid + halfSize].y = (vertices[topLeft + squareSize].y + vertices[botLeft + squareSize].y + vertices[mid].y) / 3 + Random.Range(-maxHeight, maxHeight);
        vertices[botLeft + halfSize].y = (vertices[botLeft].y + vertices[botLeft + squareSize].y + vertices[mid].y) / 3 + Random.Range(-maxHeight, maxHeight);
    }

    // taken from Brackeys :https://www.youtube.com/watch?v=64NblGkAabk
    void DrawMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}