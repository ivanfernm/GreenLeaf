using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTerrain : MonoBehaviour
{
    public List<NavNodes> _nodes = new List<NavNodes>();
    [SerializeField] Mesh navMesh;
    [SerializeField] private bool gizmos;
    public List<GameObject> _spheres = new List<GameObject>();

    [ContextMenu("Bake Nav Mesh")]
    public void BakeNavMesh()
    {
        Init();
        DebugCreateSpheres();
    }

    [ContextMenu("DebugVertexPosition")]
    public void DebugVertexPosition()
    {
        foreach (var node in _nodes)
        {
            Debug.Log(node.GetVertexPosition());
        }
    }

    void Init()
    {
        if (_nodes == null)
        {
            _nodes = new List<NavNodes>();
        }
        else
        {
            _nodes.Clear();
        }

        //Va a tomar los datos del mesh y va a crear los nodos en cada uno de los vertices del mesh

        navMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;

        //por cada vertice en el mesh va a crear un nodo
        foreach (var vertex in navMesh.vertices)
        {
            var node = new NavNodes(vertex);
            _nodes.Add(node);
        }
    }

    [ContextMenu("debugCreateSpheres")] //esto es para que aparezca en el menu de la ventana de unity
    public void DebugCreateSpheres()
    {
       Clearspheres();
        foreach (var node in _nodes)
        {
            //Creo la sphera 
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Quad);
            //setteo al padre
            sphere.transform.parent = transform;
            //setteo la posicion
            var worldPos = sphere.transform.TransformPoint(transform.position + node.GetVertexPosition());
            sphere.transform.position = worldPos;
            node.SetWorldPosition(worldPos);
            //setteo el scale
            sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            //La agrego a la lista
            _spheres.Add(sphere);
        }
    }
    
    public void Clearspheres()
    {
        foreach (var sphere in _spheres)
        {
            DestroyImmediate(sphere);
        }
        _spheres.Clear();
    }

}

public class NavNodes
{
    private Vector3 _vertexPositon;
    private Vector3 _worldPosition;

    public NavNodes(Vector3 vPosition)
    {
        _vertexPositon = vPosition;
    }

    public Vector3 GetVertexPosition()
    {
        return _vertexPositon;
    }
    
    public void SetWorldPosition(Vector3 worldPosition)
    {
        _worldPosition = worldPosition;
    }
    
    public Vector3 GetWorldPosition()
    {
        return _worldPosition;
    }
    
}