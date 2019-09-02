using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVertices : MonoBehaviour
{
    void Start(){
        MeshFilter waterMesh = this.gameObject.GetComponent<MeshFilter>();
        waterMesh.mesh = CreateWaterMesh();

        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
        renderer.material.shader = Shader.Find("Unlit/WaterShader");

    }
    Mesh CreateWaterMesh(){

        Mesh m = new Mesh();
        m.name = "Water";

        // add vertices
        m.vertices = new[] {
            new Vector3(-1.0f, 0.0f, -1.0f), // Top
            new Vector3(-1.0f, 0.0f, 1.0f),
            new Vector3(1.0f, 0.0f, 1.0f),
            new Vector3(-1.0f, 0.0f, -1.0f),
            new Vector3(1.0f, 0.0f, 1.0f),
            new Vector3(1.0f, 0.0f, -1.0f),

        };
        // add texture
             // Define the vertex colours
        m.colors = new[] {
            Color.red, // Top
            Color.red,
            Color.red,
            Color.red,
            Color.red,
            Color.red
        };

        // triangle thing modified from workshop
        int[] triangles = new int[m.vertices.Length];
        for (int i = 0; i < m.vertices.Length; i++){


            // triangles[i] = m.vertices.Length - 1 - i; //inside cube
            triangles[i] = i; //outside cub
        }
        m.triangles = triangles;

        return m;

    }

}
