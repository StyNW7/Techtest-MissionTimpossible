//using System.Collections.Generic;
//using UnityEngine;

//public class Pathfinding : MonoBehaviour
//{
//    public GridSystem grid;

//    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
//    {
//        Node startNode = grid.GetNodeFromWorldPoint(startPos);
//        Node targetNode = grid.GetNodeFromWorldPoint(targetPos);

//        if (startNode == null || targetNode == null)
//        {
//            Debug.LogWarning("Start or target node is null. Pathfinding aborted.");
//            return null;
//        }

//        List<Node> openSet = new List<Node>();
//        HashSet<Node> closedSet = new HashSet<Node>();
//        openSet.Add(startNode);

//        while (openSet.Count > 0)
//        {
//            Node currentNode = openSet[0];
//            for (int i = 1; i < openSet.Count; i++)
//            {
//                if (openSet[i].fCost < currentNode.fCost ||
//                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
//                {
//                    currentNode = openSet[i];
//                }
//            }

//            openSet.Remove(currentNode);
//            closedSet.Add(currentNode);

//            if (currentNode == targetNode)
//            {
//                Debug.Log("Path found! Retracing path...");
//                return RetracePath(startNode, targetNode);
//            }

//            foreach (Node neighbor in grid.GetNeighbors(currentNode))
//            {
//                if (!neighbor.isWalkable || closedSet.Contains(neighbor)) continue;

//                int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
//                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
//                {
//                    neighbor.gCost = newCostToNeighbor;
//                    neighbor.hCost = GetDistance(neighbor, targetNode);
//                    neighbor.parent = currentNode;

//                    if (!openSet.Contains(neighbor))
//                        openSet.Add(neighbor);
//                }
//            }
//        }

//        Debug.LogWarning("Path not found!");
//        return null;
//    }

//    List<Node> RetracePath(Node startNode, Node endNode)
//    {
//        List<Node> path = new List<Node>();
//        Node currentNode = endNode;

//        while (currentNode != startNode)
//        {
//            path.Add(currentNode);
//            currentNode = currentNode.parent;
//        }

//        path.Reverse();
//        Debug.Log($"Path length: {path.Count}");
//        return path;
//    }

//    int GetDistance(Node a, Node b)
//    {
//        int dstX = Mathf.Abs(a.gridX - b.gridX);
//        int dstY = Mathf.Abs(a.gridY - b.gridY);

//        if (dstX > dstY)
//            return 14 * dstY + 10 * (dstX - dstY);
//        return 14 * dstX + 10 * (dstY - dstX);
//    }
//}
