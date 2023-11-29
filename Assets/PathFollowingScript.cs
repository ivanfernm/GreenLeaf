using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PathFollowingScript : MonoBehaviour
{
    public GameObject target;
    public float speed = 1f;
    private List<Vector3> path;
    private int currentPathIndex = 0;
    private AStar<Vector3> aStar;

    void Start()
    {
        aStar = new AStar<Vector3>();
        // Define your start, end, satisfies, getNeighbours, getCost, heuristic here
        Vector3 start = NodeManager.Instance.GetClosestNodeAndPosition(transform.position).Item2;
        Vector3 end =target.transform.position;
        var a = NodeManager.Instance.nodesInTheScene.Select(x => x.transform.position).ToList();
        path = aStar.Run(start, 
            satisfies: node => node == end, 
            getNeighbours: GetNeighbours,
            getCost: GetCost, 
            heuristic: node => Vector3.Distance(node, end));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = GetMousePosition();
            Vector3 start = NodeManager.Instance.GetClosestNodeAndPosition(transform.position).Item2;
            Vector3 end = NodeManager.Instance.GetClosestNodeAndPosition(mousePosition).Item2;
            path = aStar.Run(start, 
                               satisfies: node => node == end, 
                                              getNeighbours: GetNeighbours,
                                                             getCost: GetCost, 
                                                                            heuristic: node => Vector3.Distance(node, end));
            currentPathIndex = 0;
        }

        
        if (path == null || path.Count == 0) return;

        Vector3 targetPosition = path[currentPathIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            currentPathIndex++;
            if (currentPathIndex >= path.Count)
            {
                path = null;
            }
        }
    }

    List<Vector3> GetNeighbours(Vector3 node)
    {
        var a = NodeManager.Instance.nodesInTheScene;
        var b = a.Select(x => x.transform.position).ToList();   
        return b;
    }

    float GetCost(Vector3 father, Vector3 child)
    {

        // Implement your logic to get cost from father to child here
        return Vector3.Distance(father, child);
    }


    //return the position of the mouse in the world where de mouse raicast hit
    Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit.point;
    }

}
