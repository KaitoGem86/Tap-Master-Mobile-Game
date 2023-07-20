using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NomarlizeVertixOfBlock : MonoBehaviour
{
    Vector3[] vertices;
    // Start is called before the first frame update
    void Start()
    {
        vertices = this.GetComponent<MeshFilter>().mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            float x = (float)Math.Round(vertices[i].x * 2) / 2;
            float y = (float)Math.Round(vertices[i].y * 2) / 2;
            float z = (float)Math.Round(vertices[i].z * 2) / 2;
            vertices[i] = new Vector3(x, y, z);
        }
        this.GetComponent<MeshFilter>().mesh.SetVertices(vertices);
        this.GetComponent<MeshCollider>().sharedMesh = this.GetComponent<MeshFilter>().sharedMesh;
    }
}
