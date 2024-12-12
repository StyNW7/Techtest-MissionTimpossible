//using System.Collections.Generic;
//using UnityEngine;

//public class GridSystem : MonoBehaviour
//{
//    public int gridWidth; // Lebar grid dalam jumlah node
//    public int gridHeight; // Tinggi grid dalam jumlah node
//    public float nodeSize; // Ukuran setiap node (jarak antar node)
//    public LayerMask unwalkableMask; // Mask untuk mendeteksi area tidak dapat dilalui

//    private Node[,] grid; // Penyimpanan grid node
//    private float gridWorldWidth;
//    private float gridWorldHeight;

//    void Start()
//    {
//        Debug.Log("Creating the grid...");
//        gridWorldWidth = gridWidth * nodeSize;
//        gridWorldHeight = gridHeight * nodeSize;
//        CreateGrid();
//        Debug.Log("Grid creation completed.");
//    }

//    void CreateGrid()
//    {
//        grid = new Node[gridWidth, gridHeight];
//        Vector3 worldBottomLeft = transform.position
//                                  - Vector3.right * (gridWorldWidth / 2)
//                                  - Vector3.forward * (gridWorldHeight / 2);

//        for (int x = 0; x < gridWidth; x++)
//        {
//            for (int y = 0; y < gridHeight; y++)
//            {
//                Vector3 worldPoint = worldBottomLeft
//                                     + Vector3.right * (x * nodeSize + nodeSize / 2)
//                                     + Vector3.forward * (y * nodeSize + nodeSize / 2);

//                bool walkable = !Physics.CheckSphere(worldPoint, nodeSize / 2, unwalkableMask);
//                grid[x, y] = new Node(worldPoint, walkable, x, y);

//                // Debug log setiap node dibuat
//                Debug.Log($"Node created at {worldPoint}: Walkable = {walkable}");
//            }
//        }
//    }

//    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
//    {
//        float percentX = Mathf.Clamp01((worldPosition.x + gridWorldWidth / 2) / gridWorldWidth);
//        float percentY = Mathf.Clamp01((worldPosition.z + gridWorldHeight / 2) / gridWorldHeight);

//        int x = Mathf.RoundToInt((gridWidth - 1) * percentX);
//        int y = Mathf.RoundToInt((gridHeight - 1) * percentY);

//        Node node = grid[x, y];
//        Debug.Log($"Node retrieved at grid position ({x}, {y}) for world position {worldPosition}: Walkable = {node.isWalkable}");
//        return node;
//    }

//    public List<Node> GetNeighbors(Node node)
//    {
//        List<Node> neighbors = new List<Node>();

//        for (int dx = -1; dx <= 1; dx++)
//        {
//            for (int dy = -1; dy <= 1; dy++)
//            {
//                if (dx == 0 && dy == 0) continue;

//                int checkX = node.gridX + dx;
//                int checkY = node.gridY + dy;

//                if (checkX >= 0 && checkX < gridWidth && checkY >= 0 && checkY < gridHeight)
//                {
//                    neighbors.Add(grid[checkX, checkY]);
//                }
//            }
//        }

//        return neighbors;
//    }

//    void OnDrawGizmos()
//    {
//        Gizmos.color = Color.white;

//        if (grid != null)
//        {
//            foreach (Node n in grid)
//            {
//                Gizmos.color = n.isWalkable ? Color.green : Color.red;
//                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeSize - 0.1f));
//            }
//        }
//    }
//}
