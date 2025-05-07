
using Unity.Mathematics;
using UnityEngine;

public class Controller
{
    View view;
    Board board;

    const int ROWS = 9;
    const int COLS = 9;

    Team currentTurn = Team.White;
    Piece selectedPiece = null;

    public Controller(View view)
    {
        this.view = view;
        board = new Board(ROWS, COLS);
        view.CreateGrid(ref board, ROWS, COLS);
        SetBoard();
    }

    void SetBoard()
    {
        CreatePawns();
        CreateSpears();
        CreateKnights();
        CreateSilvers();
        CreateGolds();
        CreateKings();
        CreateTowers();
        CreateBishops();
    }

    #region Piece Creation
    void CreatePawns()
    {
        for (int i = 0; i < COLS; i++)
        {
            CreatePiece(new int2(ROWS - 3, i), PieceType.Pawn, Team.White);
            CreatePiece(new int2(2, i), PieceType.Pawn, Team.Black);
        }
    }

    void CreateSpears()
    {
        CreatePiece(new int2(ROWS - 1, 0), PieceType.Spear, Team.White);
        CreatePiece(new int2(ROWS - 1, 8), PieceType.Spear, Team.White);
        CreatePiece(new int2(0, 0), PieceType.Spear, Team.Black);
        CreatePiece(new int2(0, 8), PieceType.Spear, Team.Black);
    }

    void CreateKnights()
    {
        CreatePiece(new int2(ROWS - 1, 1), PieceType.Horse, Team.White);
        CreatePiece(new int2(ROWS - 1, 7), PieceType.Horse, Team.White);
        CreatePiece(new int2(0, 1), PieceType.Horse, Team.Black);
        CreatePiece(new int2(0, 7), PieceType.Horse, Team.Black);
    }

    void CreateSilvers()
    {
        CreatePiece(new int2(ROWS - 1, 2), PieceType.Silver, Team.White);
        CreatePiece(new int2(ROWS - 1, 6), PieceType.Silver, Team.White);
        CreatePiece(new int2(0, 2), PieceType.Silver, Team.Black);
        CreatePiece(new int2(0, 6), PieceType.Silver, Team.Black);
    }

    void CreateGolds()
    {
        CreatePiece(new int2(ROWS - 1, 3), PieceType.Gold, Team.White);
        CreatePiece(new int2(ROWS - 1, 5), PieceType.Gold, Team.White);
        CreatePiece(new int2(0, 3), PieceType.Gold, Team.Black);
        CreatePiece(new int2(0, 5), PieceType.Gold, Team.Black);
    }

    void CreateKings()
    {
        CreatePiece(new int2(ROWS - 1, 4), PieceType.King, Team.White);
        CreatePiece(new int2(0, 4), PieceType.King, Team.Black);
    }

    void CreateTowers()
    {
        CreatePiece(new int2(ROWS - 2, 7), PieceType.Tower, Team.White);
        CreatePiece(new int2(1, 1), PieceType.Tower, Team.Black);
    }

    void CreateBishops()
    {
        CreatePiece(new int2(ROWS - 2, 1), PieceType.Bishop, Team.White);
        CreatePiece(new int2(1, 7), PieceType.Bishop, Team.Black);
    }

    void CreatePiece(int2 coor, PieceType type, Team team)
    {
        Piece piece = type switch {
            PieceType.Pawn => new Pawn(coor, team),
            PieceType.Spear => new Spear(coor, team),
            PieceType.Horse => new Horse(coor, team),
            PieceType.Silver => new Silver(coor, team),
            PieceType.Gold => new Gold(coor, team),
            PieceType.Tower => new Tower(coor, team),
            PieceType.Bishop => new Bishop(coor, team),
            PieceType.King => new King(coor, team),
            _ => null
        };
        if (piece == null) return;
        board.GetSquare(coor.x, coor.y).piece = piece;
        view.AddPiece(ref piece, coor);
    }
    #endregion

    public void SelectSquare(int2 gridPos) {
        ref Square selectedSquare = ref board.GetSquare(gridPos.x, gridPos.y);
        if (selectedPiece != null) {
            if (selectedSquare.piece == null) MoveSelectedPiece(selectedSquare);
            else if (selectedSquare.piece.team == currentTurn) selectedPiece = selectedSquare.piece;
            else {
                EatPiece(selectedSquare.Coor);
                MoveSelectedPiece(selectedSquare);
            }
        }
        else {
            if (selectedSquare.piece == null) return;
            if (selectedSquare.piece.team != currentTurn) return;
            selectedPiece = selectedSquare.piece;
        }
    }

    void EatPiece(int2 coor) { 

    }

    void MoveSelectedPiece(Square selectedSquare) {
        RemovePiece(selectedPiece.coor);
        AddPiece(ref selectedPiece, selectedSquare.Coor);
        selectedPiece = null;
    }

    void RemovePiece(int2 coor) {
        board.GetSquare(coor.x, coor.y).piece = null;
        view.RemovePiece(coor);
    }

    void AddPiece(ref Piece piece, int2 coor) {
        board.GetSquare(coor.x, coor.y).piece = piece;
        piece.coor = coor;
        view.AddPiece(ref piece, coor);
    }

    ~Controller(){}
}
