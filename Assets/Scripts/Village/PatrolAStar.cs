using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class PatrolWithAStar : MonoBehaviour
{
    public Transform[] waypoints;
    public AStar aStar;
    public float moveSpeed = 3f;

    private List<Node> currentPath = new List<Node>();
    private int currentWaypointIndex = 0;
    private int currentPathIndex = 0;

    void Update()
    {
        if (currentPath == null || currentPathIndex >= currentPath.Count)
        {
            FindNextPath();
        }

        MoveAlongPath();
    }

    void FindNextPath()
    {
        Vector3 currentPos = transform.position;
        Vector3 nextWaypointPos = waypoints[currentWaypointIndex].position;

        currentPath = aStar.FindPath(currentPos, nextWaypointPos);
        currentPathIndex = 0;

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    void MoveAlongPath()
    {

        Debug.Log("Current Path: " + currentPath);
        Debug.Log("Current Path Index: " + currentPath);

        if (currentPath == null || currentPathIndex >= currentPath.Count) return;

        Debug.Log("Move");

        Node targetNode = currentPath[currentPathIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetNode.worldPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetNode.worldPosition) < 0.1f)
        {
            currentPathIndex++;
        }
    }
}
