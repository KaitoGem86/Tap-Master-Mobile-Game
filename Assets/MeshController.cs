using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : MonoBehaviour
{
    [SerializeField] private Mesh _meshRenderer;
    [SerializeField] private Material _standard;
    // Start is called before the first frame update
    void Start()
    {
        // MeshFilter _meshFilter = GetComponent<MeshFilter>();
        // Mesh _mesh = _meshFilter.mesh;

        // for (int i = 0; i < _mesh.subMeshCount; i++)
        // {
        //     var indices = _mesh.GetIndices(i);
        //     var mesh = new Mesh();
        //     mesh.vertices = _mesh.vertices;
        //     mesh.uv = _mesh.uv;
        //     mesh.normals = _mesh.normals;
        //     mesh.SetIndices(indices, _mesh.GetTopology(i), 0);

        //     var gameObject = new GameObject("SubMesh" + i);
        //     var meshFilter = gameObject.AddComponent<MeshFilter>();
        //     var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        //     meshFilter.mesh = mesh;
        //     meshRenderer.material = _standard; // You can replace "Standard" with the name of the shader you want to use
        // }
        // Debug.Log(_meshRenderer.vertices.Length);
        // foreach (var vertex in _meshRenderer.vertices)
        // {
        //     Debug.Log(vertex);
        // }
        //_meshRenderer.GetVertices();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
