//using UnityEngine;

//public class Node
//{
//    // Posisi dunia dari node
//    public Vector3 worldPosition;

//    // Apakah node dapat dilalui
//    public bool isWalkable;

//    // Biaya G (jarak dari start node ke node ini)
//    public int gCost;

//    // Biaya H (perkiraan jarak dari node ini ke target node)
//    public int hCost;

//    // Node induk, digunakan untuk retracing path
//    public Node parent;

//    // Biaya F (total: gCost + hCost)
//    public int fCost => gCost + hCost;

//    // Konfigurasi grid posisi untuk debug atau log
//    public int gridX;
//    public int gridY;

//    // Konstruktor
//    public Node(Vector3 worldPosition, bool isWalkable)
//    {
//        this.worldPosition = worldPosition;
//        this.isWalkable = isWalkable;
//    }

//    // Overloading konstruktor untuk mendukung grid posisi
//    public Node(Vector3 worldPosition, bool isWalkable, int gridX, int gridY)
//    {
//        this.worldPosition = worldPosition;
//        this.isWalkable = isWalkable;
//        this.gridX = gridX;
//        this.gridY = gridY;
//    }

//    // Fungsi untuk debugging informasi node
//    public override string ToString()
//    {
//        return $"Node Position: {worldPosition}, Walkable: {isWalkable}, GridPos: ({gridX}, {gridY}), gCost: {gCost}, hCost: {hCost}, fCost: {fCost}";
//    }
//}


using System;
using UnityEngine;

public class Node
{
    public bool walkable; // Apakah node bisa dilewati
    public Vector3 worldPosition; // Posisi dunia dari node
    public int gridX, gridZ; // Posisi grid pada array
    public int gCost; // Biaya dari node awal ke node ini
    public int hCost; // Estimasi biaya dari node ini ke node tujuan
    public Node parent; // Node sebelumnya dalam jalur

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridZ)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridZ = gridZ;
    }

    // Total biaya (fCost) untuk node ini
    public int fCost
    {
        get { return gCost + hCost; }
    }

    public static implicit operator Node(Node3 v)
    {
        throw new NotImplementedException();
    }
}
