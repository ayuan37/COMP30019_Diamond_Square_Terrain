using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVertices : MonoBehaviour
{
    // public Shader waterShader;
    public SunScript pointLight;

    // Use this for initialization
    void Start()
    {
        // Add a MeshFilter component to this entity. This essentially comprises of a
        // mesh definition, which in this example is a collection of vertices, colours 
        // and triangles (groups of three vertices). 

        // MeshFilter waterMesh = this.gameObject.AddComponent<MeshFilter>();
        // waterMesh.mesh = CreateWaterMesh();

        // Add a MeshRenderer component. This component actually renders the mesh that
        // is defined by the MeshFilter component.
        // MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        // renderer.material.shader = waterShader;
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

    // Mesh CreateWaterMesh(){

    //     Mesh m = new Mesh();
    //     m.name = "Water";
    //     float size = 5;

    //     // add vertices
    //     m.vertices = new[] {

    //         new Vector3(1.0f, 0.0f, -1.0f) * size, // Top
    //         new Vector3(-1.0f, 0.0f, -1.0f) * size,
    //         new Vector3(-1.0f, 0.0f, 1.0f) * size,
    //         new Vector3(1.0f, 0.0f, -1.0f) * size,
    //         new Vector3(-1.0f, 0.0f, 1.0f)* size,
    //         new Vector3(1.0f, 0.0f, 1.0f)* size,

    //     };

    //     Vector3 topNormal = new Vector3(0.0f, 1.0f, 0.0f);

    //     m.normals = new[] {
    //         topNormal,
    //     };

    //     // triangle thing modified from workshop
    //     int[] triangles = new int[m.vertices.Length];
    //     for (int i = 0; i < m.vertices.Length; i++){


    //         // triangles[i] = m.vertices.Length - 1 - i; //inside cube
    //         triangles[i] = i; //outside cub
    //     }
    //     m.triangles = triangles;

    //     return m;

    // }

}
