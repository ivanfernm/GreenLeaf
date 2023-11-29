using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using System.Linq;


public class AStar<T>
{
    public delegate bool Satisfies(T curr);
    public delegate List<T> GetNeighbours(T curr);
    public delegate float GetCost(T father, T child);
    public delegate float Heuristic(T curr);

    public List<T> Run(T start, Satisfies satisfies, GetNeighbours getNeighbours, GetCost getCost, Heuristic heuristic)
    {
        Dictionary<T, float> cost = new Dictionary<T, float>();
        Dictionary<T, T> parents = new Dictionary<T, T>();
        PriorityQueue<T> pending = new PriorityQueue<T>();
        HashSet<T> visited = new HashSet<T>();
        pending.Enqueue(start, 0);
        cost.Add(start, 0);
        parents.Add(start, start);

        while (!pending.IsEmpty)
        {
            T current = pending.Dequeue();
            if (satisfies(current))
            {
                return ConstructPath(current, parents);
            }
            visited.Add(current);
            List<T> neighbours = getNeighbours(current);

            for (int i = 0; i < neighbours.Count; i++)
            {
                T node = neighbours[i];
                if (visited.Contains(node)) continue;
                T parent = parents[current];
                float oldCost = cost.ContainsKey(node) ? cost[node] : float.MaxValue;
                float newCost = cost[parent] + getCost(parent, node);

                if (newCost < oldCost)
                {
                    if (cost.ContainsKey(node))
                    {
                        cost[node] = newCost;
                    }
                    else
                    {
                        cost.Add(node, newCost);
                    }
                    parents[node] = parent;
                    pending.Enqueue(node, newCost + heuristic(node));
                }
            }
        }

        return new List<T>();
    }

    List<T> ConstructPath(T end, Dictionary<T, T> parents)
    {
        var path = new List<T>();
        path.Add(end);

        while (!EqualityComparer<T>.Default.Equals(parents[path[path.Count - 1]], path[path.Count - 1]))
        {
            var lastNode = path[path.Count - 1];
            path.Add(parents[lastNode]);
        }
        path.Reverse();
        return path;
    }
}
