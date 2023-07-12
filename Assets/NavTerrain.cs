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
    private Tuple<NavNodes, NavNodes, NavNodes, NavNodes> _nearByNodes;

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
        _nearByNodes = GetNeightbords(_navTerrain);

        Debug.Log("left" + _nearByNodes.Item1.GetWorldPosition());
        Debug.Log("right" + _nearByNodes.Item2.GetWorldPosition());
        Debug.Log("up" + _nearByNodes.Item3.GetWorldPosition());
        Debug.Log("down" + _nearByNodes.Item4.GetWorldPosition());
    }

    public Tuple<NavNodes, NavNodes, NavNodes, NavNodes> GetNeightbords(NavTerrain navTerrain)
    {
        var result = navTerrain._nodes.Aggregate(
            Tuple.Create<NavNodes, NavNodes, NavNodes, NavNodes>(null, null, null, null),
            (x, y) =>
            {
                var arrey = new List<NavNodes>();
                var left = navTerrain._nodes.Where(z => z.GetWorldPosition().x < y.GetWorldPosition().x);
                if (left.Any())
                {
                    var o = left.OrderByDescending(z => z.GetWorldPosition().x).First();
                    arrey.Add(o);
                }
                else
                {
                    arrey.Add(this);
                }

                var right = navTerrain._nodes.Where(z => z.GetWorldPosition().x > y.GetWorldPosition().x);
                if (right.Any())
                {
                    var o = right.OrderBy(z => z.GetWorldPosition().x).First();
                    arrey.Add(o);
                }
                else
                {
                    arrey.Add(this);
                }

                var up = navTerrain._nodes.Where(z => z.GetWorldPosition().y > y.GetWorldPosition().y);
                if (up.Any())
                {
                    var o = up.OrderByDescending(z => z.GetWorldPosition().y).First();
                    arrey.Add(o);
                }
                else
                {
                    arrey.Add(this);
                }

                var down = navTerrain._nodes.Where(z => z.GetWorldPosition().y < y.GetWorldPosition().y);
                if (down.Any())
                {
                    var o = down.OrderBy(z => z.GetWorldPosition().y).First();
                    arrey.Add(o);
                }
                else
                {
                    arrey.Add(this);
                }

                var tuple = Tuple.Create<NavNodes, NavNodes, NavNodes, NavNodes>(arrey[0], arrey[1], arrey[2],
                    arrey[3]);
                return tuple;
            });

        return result;
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