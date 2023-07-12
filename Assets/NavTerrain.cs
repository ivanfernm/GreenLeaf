using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        _nodes[20].SetNearByNodes();
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
            var node = new NavNodes(vertex, this);
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
    public NavTerrain _navTerrain;
    private Vector3 _vertexPositon;
    private Vector3 _worldPosition;
    private List<NavNodes> _nearByNodes = new List<NavNodes>();
    public NavNodes(Vector3 vPosition, NavTerrain navTerrain)
    {
        _vertexPositon = vPosition;
        _navTerrain = navTerrain;
    }

    public Vector3 GetVertexPosition()
    {
        return _vertexPositon;
    }

    public void SetNearByNodes()
    {
        //por cada nodo en la lista de nodos
        foreach (var node in _navTerrain._nodes)
        {
            //si el nodo es el mismo que el que estoy iterando, no hago nada
            if (node == this)
            {
                continue;
            }
            //optimise the code bellow
            var Left = _navTerrain._nodes.Where(x => x.GetWorldPosition().x < _worldPosition.x);
            if (Left.Any())
            {
              var a = Left.OrderByDescending(x => x.GetWorldPosition().x).First();
              _nearByNodes.Add(a);
            }

            var right  = _navTerrain._nodes.Where(x => x.GetWorldPosition().x > _worldPosition.x);
            if (right.Any())
            {
               var a = right.OrderByDescending(x => x.GetWorldPosition().x).First();
               _nearByNodes.Add(a);
            }

            var up = _navTerrain._nodes.Where(x => x.GetWorldPosition().y > _worldPosition.y);
            if (up.Any())
            {
                var a = up.OrderByDescending(x => x.GetWorldPosition().y).First();
                _nearByNodes.Add(a);
            }

            var down = _navTerrain._nodes.Where(x => x.GetWorldPosition().y < _worldPosition.y);
            if (down.Any())
            {
                var a = down.OrderByDescending(x => x.GetWorldPosition().y).First();
                _nearByNodes.Add(a);
            }

            
        }
        foreach (var VARIABLE in _nearByNodes)
        {
            Debug.Log(VARIABLE.GetWorldPosition());
        }
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