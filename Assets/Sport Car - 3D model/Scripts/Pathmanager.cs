using UnityEngine;
using System.Collections.Generic;

public class PathManager : MonoBehaviour
{
    public Transform startPoint; 
    public Transform endPoint; 
    public NodeConnector nodeConnector; 
    public CarController carController; 

    private List<Node> path; 

    void Start()
    {
        Node startNode = FindClosestNode(startPoint.position);
        Node endNode = FindClosestNode(endPoint.position);

        if (startNode == null || endNode == null)
        {
            Debug.Log("Start or end node is null");
            
            if (startNode == null)
                Debug.Log("Failed to find a start node close to: " + startPoint.position);
            if (endNode == null)
                Debug.Log("Failed to find an end node close to: " + endPoint.position);
            return;
        }


        path = AStar.FindPath(startNode, endNode);

        if (carController != null)
        {
            carController.SetPath(path); 
        }
    }

    Node FindClosestNode(Vector3 position)
    {
        Node closestNode = null;
        float minDistance = float.MaxValue;

        foreach (var nodePair in nodeConnector.nodeDictionary)
        {
            float distance = Vector3.Distance(position, nodePair.Value.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestNode = nodePair.Value;
            }
        }

        return closestNode;
    }

    void OnDrawGizmos()
    {
        if (path != null && path.Count > 1)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < path.Count - 1; i++)
            {
                Gizmos.DrawLine(path[i].position, path[i + 1].position);
            }
        }
    }
}



