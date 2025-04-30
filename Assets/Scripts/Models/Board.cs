using UnityEngine;
using Unity.Mathematics;
using System.Runtime.InteropServices.WindowsRuntime;

public struct Board
{
    Square[,] grid;

    public Board(int rows, int cols) {
        grid = new Square[rows, cols];
        for(int i = 0; i < rows; i++) {
            for(int j = 0; j < cols; j++) {
                grid[i, j] = new Square(i, j);
            }
        }
    }

    public ref Square GetSquare(int row, int col) => ref grid[row, col];
}

public struct Square 
{
    int2 coor;
    public Piece piece;

    public Square(int x, int y){
        coor = new int2(x, y);
        piece = null;
    }

    public int2 Coor => coor;
}
