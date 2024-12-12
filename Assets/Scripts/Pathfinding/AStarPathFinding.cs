using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Pathfinding3 : MonoBehaviour
{
    public Transform seeker;
    public GridManager3 grid;

    public List<Node3> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node3 startNode = grid.NodeFromWorldPoint(startPos);
        Node3 targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node3> openSet = new List<Node3>();
        HashSet<Node3> closedSet = new HashSet<Node3>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node3 currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                Debug.Log("Lagi retrace pathh");
                return RetracePath(startNode, targetNode);
            }

            foreach (Node3 neighbor in grid.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        Debug.Log("No Path");
        return null; // Jika tidak ada jalur
    }

    private List<Node3> RetracePath(Node3 startNode, Node3 endNode)
    {
        List<Node3> path = new List<Node3>();
        Node3 currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        Debug.Log("Retrace path dikembalikan: " + path);
        return path;
    }

    private int GetDistance(Node3 nodeA, Node3 nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        Debug.Log("Distance X: " + dstX);
        return (dstX > dstY) ? 14 * dstY + 10 * (dstX - dstY) : 14 * dstX + 10 * (dstY - dstX);
    }
}