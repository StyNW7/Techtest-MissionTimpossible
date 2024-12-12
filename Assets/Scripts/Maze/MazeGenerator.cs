using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int mazeWidth = 20; // Lebar maze
    public int mazeHeight = 20; // Tinggi maze
    public GameObject wallPrefab; // Prefab untuk dinding
    public GameObject floorPrefab; // Prefab untuk lantai

    void Start()
    {
        GenerateMaze();
    }

    // Fungsi untuk memulai pembuatan maze
    void GenerateMaze()
    {
        Room initialRoom = new Room(0, 0, mazeWidth, mazeHeight);
        SplitRoom(initialRoom);
    }

    // Fungsi untuk membagi ruang menggunakan BSP
    void SplitRoom(Room room)
    {
        if (room.Width <= 2 || room.Height <= 2)
            return;

        // Tentukan cara membagi ruang secara vertikal atau horizontal
        bool splitVertically = Random.Range(0, 2) == 0;
        if (splitVertically)
            SplitVertically(room);
        else
            SplitHorizontally(room);
    }

    // Membagi ruang secara vertikal
    void SplitVertically(Room room)
    {
        int splitX = Random.Range(room.X + 1, room.X + room.Width - 1);
        int doorY = Random.Range(room.Y + 1, room.Y + room.Height - 1);

        // Membuat dinding vertikal
        for (int y = room.Y; y < room.Y + room.Height; y++)
        {
            if (y != doorY) // Jangan membuat dinding pada tempat pintu
                CreateWall(splitX, y);
        }

        // Buat ruang kiri dan kanan
        Room leftRoom = new Room(room.X, room.Y, splitX - room.X, room.Height);
        Room rightRoom = new Room(splitX, room.Y, room.Width - (splitX - room.X), room.Height);

        // Panggil rekursi untuk membagi lagi
        SplitRoom(leftRoom);
        SplitRoom(rightRoom);
    }

    // Membagi ruang secara horizontal
    void SplitHorizontally(Room room)
    {
        int splitY = Random.Range(room.Y + 1, room.Y + room.Height - 1);
        int doorX = Random.Range(room.X + 1, room.X + room.Width - 1);

        // Membuat dinding horizontal
        for (int x = room.X; x < room.X + room.Width; x++)
        {
            if (x != doorX) // Jangan membuat dinding pada tempat pintu
                CreateWall(x, splitY);
        }

        // Buat ruang atas dan bawah
        Room topRoom = new Room(room.X, room.Y, room.Width, splitY - room.Y);
        Room bottomRoom = new Room(room.X, splitY, room.Width, room.Y + room.Height - splitY);

        // Panggil rekursi untuk membagi lagi
        SplitRoom(topRoom);
        SplitRoom(bottomRoom);
    }

    // Membuat dinding pada koordinat x, y
    void CreateWall(int x, int y)
    {
        Vector3 wallPosition = new Vector3(x, 0, y);
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, transform);
    }

    // Membuat lantai pada seluruh area maze
    void CreateFloor()
    {
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                Vector3 floorPosition = new Vector3(x, -1, y);
                Instantiate(floorPrefab, floorPosition, Quaternion.identity, transform);
            }
        }
    }
}

public class Room
{
    public int X, Y, Width, Height;

    public Room(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
}
