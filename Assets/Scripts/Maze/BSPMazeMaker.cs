using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSPMazeMaker : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;

    private MazeCell[,] _mazeGrid;

    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        GenerateBSP(0, 0, _mazeWidth, _mazeDepth);
    }

    // Fungsi untuk mengenerate maze menggunakan BSP Algorithm
    private void GenerateBSP(int xStart, int zStart, int width, int height)
    {
        // Basis rekursi: jika ruang terlalu kecil untuk dibagi lagi, berhenti
        if (width < 2 || height < 2)
            return;

        // Tentukan apakah kita akan membagi ruang secara vertikal atau horizontal
        bool splitVertically = Random.Range(0, 2) == 0;

        if (splitVertically)
        {
            // Pembagian vertikal
            int xSplit = Random.Range(xStart + 1, xStart + width - 1);
            CreateWall(xStart, zStart, xSplit, height, true);

            // Rekursi untuk bagian kiri dan kanan
            GenerateBSP(xStart, zStart, xSplit - xStart, height); // Area kiri
            GenerateBSP(xSplit, zStart, xStart + width - xSplit, height); // Area kanan
        }
        else
        {
            // Pembagian horizontal
            int zSplit = Random.Range(zStart + 1, zStart + height - 1);
            CreateWall(xStart, zStart, width, zSplit, false);

            // Rekursi untuk bagian bawah dan atas
            GenerateBSP(xStart, zStart, width, zSplit - zStart); // Area bawah
            GenerateBSP(xStart, zSplit, width, zStart + height - zSplit); // Area atas
        }
    }

    // Fungsi untuk membuat dinding pada bagian tertentu dari maze
    private void CreateWall(int xStart, int zStart, int width, int height, bool isVertical)
    {
        if (isVertical)
        {
            // Buat dinding vertikal
            for (int z = zStart; z < zStart + height; z++)
            {
                MazeCell wallCell = _mazeGrid[xStart, z];
                wallCell.Visit(); // Tandai sebagai dinding
                wallCell.ClearLeftWall(); // Hapus dinding kiri
                wallCell.ClearRightWall(); // Hapus dinding kanan
            }
        }
        else
        {
            // Buat dinding horizontal
            for (int x = xStart; x < xStart + width; x++)
            {
                MazeCell wallCell = _mazeGrid[x, zStart];
                wallCell.Visit(); // Tandai sebagai dinding
                wallCell.ClearFrontWall(); // Hapus dinding depan
                wallCell.ClearBackWall(); // Hapus dinding belakang
            }
        }
    }
}
