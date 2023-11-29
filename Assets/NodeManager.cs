using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class NodeManager : MonoBehaviour
{
    public List<Node> nodesInTheScene = new List<Node>();

    //singelton
    public static NodeManager Instance;


    private void Awake()
    {
        Instance = this;
    }
    //based in an gameObject position return a tuple whit the closest node, and the postion of the node
    public (Node, Vector3) GetClosestNodeAndPosition(Vector3 position)
    {
        var closestNode = nodesInTheScene.OrderBy(x => Vector3.Distance(x.transform.position, position)).FirstOrDefault();
        return (closestNode, closestNode.transform.position);
    }

    //look fot all the nodes in the scene
    [ContextMenu("GetActiveNodes")]
    public void GetActiveNodes()
    {
        nodesInTheScene.Clear();
        nodesInTheScene.AddRange(FindObjectsOfType<Node>()); 

        //set ll the nodes in the scene to this node manager
        foreach (Node node in nodesInTheScene)
        {
            node.NodeManager = this;
        }
 
    }

    //set the neightbors of all the nodes in the scene, based in the position each node need to hace a Top, Bottom, Left and Right neightbor
    [ContextMenu("SetNeightbours")]
    public void SetNeightbors()
    {
        
        foreach(Node node in nodesInTheScene)
        {
            node.neightbors.Clear();
            //voy a chequear si hay un nodo arriba, abajo, a la izquierda y a la derecha

            var currentPos = node.transform.position;

            foreach(Node otherNode in nodesInTheScene)
            {
               if(otherNode.transform.position.z > currentPos.z)
                {
                     var topNode = nodesInTheScene.Where(x => x.transform.position.z > currentPos.z).OrderBy(x => Vector3.Distance(x.transform.position, currentPos)).FirstOrDefault();
                     node.neightbors.Add(topNode);
                }
             
               if(otherNode.transform.position.z < currentPos.z)
                {
                        var bottomNode = nodesInTheScene.Where(x => x.transform.position.z < currentPos.z).OrderBy(x => Vector3.Distance(x.transform.position, currentPos)).FirstOrDefault();
                        node.neightbors.Add(bottomNode);
                 }
              

               if(otherNode.transform.position.x < currentPos.x)
               {
                         var leftNode = nodesInTheScene.Where(x => x.transform.position.x < currentPos.x).OrderBy(x => Vector3.Distance(x.transform.position, currentPos)).FirstOrDefault();
                         node.neightbors.Add(leftNode);
               }
               if(otherNode.transform.position.x > currentPos.x)
                {
                             var rightNode = nodesInTheScene.Where(x => x.transform.position.x > currentPos.x).OrderBy(x => Vector3.Distance(x.transform.position, currentPos)).FirstOrDefault();
                             node.neightbors.Add(rightNode);
                }
               
                //remove duplicates
                node.neightbors = node.neightbors.Distinct().ToList();



            }
        }   
    }
}
