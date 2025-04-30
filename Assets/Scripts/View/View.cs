using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class View : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject squarePref;

    [Header("View Objects")]
    [SerializeField] Transform gridParent;

    Controller controller;

    SquareView[,] gridView;
    
    void Awake()
    {
        controller = new Controller(this);
    }

    public void CreateGrid(ref Board board, int rows, int cols)
    {
        gridView = new SquareView[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                gridView[i,j] = Instantiate(squarePref, gridParent).GetComponent<SquareView>();
                int2 coor = board.GetSquare(i, j).Coor;
                gridView[i, j].SetSquare(coor.x, coor.y);
            }
        }
    }

    public void AddPiece(ref Piece piece, int2 coor)
    {
        gridView[coor.x, coor.y].AddPiece(ref piece);
    }

    public void RemovePiece(int2 coor)
    {
        gridView[coor.x, coor.y].RemovePiece();
    }
}
