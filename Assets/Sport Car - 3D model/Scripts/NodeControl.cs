using UnityEngine;
using System.Collections.Generic;

public class NodeConnector : MonoBehaviour
{
    public GameObject[] nodes; 
    public LayerMask obstacleLayer; 
    public Dictionary<GameObject, Node> nodeDictionary = new Dictionary<GameObject, Node>();

    void Awake()
    {
        CreateNodes();
        ConnectNodes();
    }

    //void CreateNodes()
    //{
    //    foreach (GameObject nodeObject in nodes)
    //    {
    //        Node newNode = new Node(nodeObject.transform.position);
    //        nodeDictionary[nodeObject] = newNode;
    //    }
    //}

    void CreateNodes()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            Vector3 nodeDirection;
            if (i < nodes.Length - 1)
            {
        
                nodeDirection = (nodes[i + 1].transform.position - nodes[i].transform.position).normalized;
            }
            else
            {
            
                nodeDirection = (nodes[i].transform.position - nodes[i - 1].transform.position).normalized;
            }

            Node newNode = new Node(nodes[i].transform.position, nodeDirection);
            nodeDictionary[nodes[i]] = newNode;
        }
    }


    void ConnectNodes()
    {
        foreach (GameObject nodeA in nodes)
        {
            Node nodeAData = nodeDictionary[nodeA];
            foreach (GameObject nodeB in nodes)
            {
                if (nodeA != nodeB)
                {
                    Vector3 offset = new Vector3(0.1f, 0.1f, 0.1f); 
                    Vector3 direction = nodeB.transform.position - nodeA.transform.position;
                    if (!Physics.Raycast(nodeA.transform.position + offset, direction, direction.magnitude, obstacleLayer))
                    {
                        Node nodeBData = nodeDictionary[nodeB];
                        nodeAData.neighbors.Add(nodeBData);
                    }
                }
            }
        }
    }

    
    public Node GetNodeFromGameObject(GameObject gameObject)
    {
        if (nodeDictionary.ContainsKey(gameObject))
        {
            return nodeDictionary[gameObject];
        }
        return null;
    }


    void OnDrawGizmos()
    {
        if (nodeDictionary.Count > 0)
        {
            foreach (var nodePair in nodeDictionary)
            {
                Node node = nodePair.Value;
                Gizmos.color = Color.green;
                foreach (var neighbor in node.neighbors)
                {
                    Gizmos.DrawLine(node.position, neighbor.position);
                }
            }
        }
    }
}

