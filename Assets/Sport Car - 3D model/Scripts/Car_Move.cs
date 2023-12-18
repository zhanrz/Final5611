
using UnityEngine;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    public float speed = 1f;
    public float turnSpeed = 1f;
    private List<Node> path;
    private int currentPathIndex;

    private bool isAvoidingObstacle = false;
    private float avoidDistance = 1f;
    private Vector3 avoidStartPosition;
    private Vector3 avoidDirection;

    public void SetPath(List<Node> newPath)
    {
        path = newPath;
        currentPathIndex = 0;
        if (path != null)
        {
            Debug.Log($"Path set with {path.Count} nodes");
        }
        else
        {
            Debug.Log("Path is null");
        }
    }

    void Update()
    {
        if (!isAvoidingObstacle)
        {
            if (path != null && currentPathIndex < path.Count)
            {
                Node targetNode = path[currentPathIndex];
                Vector3 targetPosition = targetNode.position;
                MoveTowards(targetPosition);
            }

            RaycastHit hit;
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            float detectionDistance = 1.0f;


            Debug.DrawLine(transform.position, transform.position + forward * detectionDistance, Color.red);

            if (Physics.Raycast(transform.position, forward, out hit, detectionDistance))
            {
                if (hit.collider.gameObject.name == "vehicle Bwheels.003")
                {
                    StartAvoidingObstacle(hit);
                }
            }
        }
        else
        {
            AvoidObstacle();
        }
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.forward = Vector3.Slerp(transform.forward, direction, turnSpeed * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentPathIndex++;
        }
    }

    private void StartAvoidingObstacle(RaycastHit hit)
    {
        isAvoidingObstacle = true;
        avoidStartPosition = transform.position;
        avoidDirection = Vector3.Cross(hit.normal, Vector3.up).normalized;

        if (Vector3.Dot(avoidDirection, transform.right) < 0)
        {
            avoidDirection = -avoidDirection;
        }
    }

    private void AvoidObstacle()
    {
        if (Vector3.Distance(transform.position, avoidStartPosition) <= avoidDistance)
        {

            Quaternion lookRotation = Quaternion.LookRotation(avoidDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime * 2);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {

            isAvoidingObstacle = false;


            if (Vector3.Distance(transform.position, path[currentPathIndex].position) < 0.1f)
            {
                currentPathIndex++;
            }

            if (currentPathIndex < path.Count)
            {
                Vector3 targetDirection = (path[currentPathIndex].position - transform.position).normalized;
                transform.forward = Vector3.Slerp(transform.forward, targetDirection, turnSpeed * Time.deltaTime);
            }
            else
            {
                Debug.Log("Reached end of the path");
            }
        }
    }


}



