using System.Collections.Generic;
using UnityEngine;


public class Node
{
    public Vector3 position; 
    public List<Node> neighbors;
    public Vector3 direction; 

    public float gCost; 
    public float hCost; 
    public float fCost { get { return gCost + hCost; } } 
    public Node parent; 

    public Node(Vector3 _position, Vector3 _direction)
    {
        position = _position;
        direction = _direction;
        neighbors = new List<Node>();
    }
}

public static class AStar
{
    public static List<Node> FindPath(Node startNode, Node targetNode)
    {
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in currentNode.neighbors)
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor) + CalculateDirectionFactor(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetHeuristic(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null;
    }

    private static List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    private static float GetDistance(Node nodeA, Node nodeB)
    {
        return Vector3.Distance(nodeA.position, nodeB.position);
    }

    private static float GetHeuristic(Node nodeA, Node nodeB)
    {
        return Vector3.Distance(nodeA.position, nodeB.position); 
    }

    private static float CalculateDirectionFactor(Node nodeA, Node nodeB)
    {
        
        float angle = Vector3.Angle(nodeA.direction, (nodeB.position - nodeA.position).normalized);
        return angle; 
    }
}

