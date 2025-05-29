
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Controller
{
    View view;
    Board board;

    const int ROWS = 9;
    const int COLS = 9;

    Team currentTurn = Team.White;
    Piece selectedPiece = null;

    Player whitePlayer;
    Player blackPlayer;

    List<int2> validMoves = new List<int2>();

    public Controller(View view) {
        this.view = view;
        board = new Board(ROWS, COLS);
        view.CreateGrid(ref board, ROWS, COLS);
        whitePlayer = new Player(Team.White);
        blackPlayer = new Player(Team.Black);
        SetBoard();
        view.EnableTeamCemetary(currentTurn);
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
            if (selectedSquare.piece == null) { //Mover
                if (!IsValidMove(selectedSquare.Coor) && selectedPiece.coor.x >= 0) return;
                if (selectedPiece.coor.x < 0) UpdateCemetaryCount(selectedPiece.type);
                MoveSelectedPiece(selectedSquare);
                SwitchTeam();
            }
            else if (selectedSquare.piece.team == currentTurn) { //Cambiar de Seleccion

                if (selectedPiece.coor.x < 0) EatPiece(ref selectedPiece);
                SelectNewPiece(selectedSquare.piece);
            }
            else if (selectedPiece.coor.x >= 0) { //Comer
                if (!IsValidMove(selectedSquare.Coor)) return;
                EatPiece(ref selectedSquare.piece);
                MoveSelectedPiece(selectedSquare);
                SwitchTeam();
            }
        }
        else { //Select
            if (selectedSquare.piece == null) return;
            if (selectedSquare.piece.team != currentTurn) return;
            SelectNewPiece(selectedSquare.piece);
        }
    }

    #region Peace Eat
    public void SelectCemetarySquare(PieceType pieceType) {
        Player currentPlayer = currentTurn == Team.White ? whitePlayer : blackPlayer;
        selectedPiece = pieceType switch
        {
            PieceType.Pawn => currentPlayer.sideBoard.pawns.Dequeue(),
            PieceType.Spear => currentPlayer.sideBoard.spears.Dequeue(),
            PieceType.Horse => currentPlayer.sideBoard.horses.Dequeue(),
            PieceType.Silver => currentPlayer.sideBoard.silvers.Dequeue(),
            PieceType.Gold => currentPlayer.sideBoard.golds.Dequeue(),
            PieceType.Tower => currentPlayer.sideBoard.towers.Dequeue(),
            PieceType.Bishop => currentPlayer.sideBoard.bishops.Dequeue(),
            _ => null
        };
        
    }
    
    void UpdateCemetaryCount(PieceType pieceType) {
        Player currentPlayer = currentTurn == Team.White ? whitePlayer : blackPlayer;
        switch (pieceType) {
            case PieceType.Pawn:
                view.UpdateCemetary(currentTurn, pieceType, currentPlayer.sideBoard.pawns.Count);
                break;
            case PieceType.Spear:
                view.UpdateCemetary(currentTurn, pieceType, currentPlayer.sideBoard.spears.Count);
                break;
            case PieceType.Horse:
                view.UpdateCemetary(currentTurn, pieceType, currentPlayer.sideBoard.horses.Count);
                break;
            case PieceType.Silver:
                view.UpdateCemetary(currentTurn, pieceType, currentPlayer.sideBoard.silvers.Count);
                break;
            case PieceType.Gold:
                view.UpdateCemetary(currentTurn, pieceType, currentPlayer.sideBoard.golds.Count);
                break;
            case PieceType.Tower:
                view.UpdateCemetary(currentTurn, pieceType, currentPlayer.sideBoard.towers.Count);
                break;
            case PieceType.Bishop:
                view.UpdateCemetary(currentTurn, pieceType, currentPlayer.sideBoard.bishops.Count);
                break;
        }
    }

    void EatPiece(ref Piece eatenPiece) { 
        if(eatenPiece.type == PieceType.King)
        {
            string win = currentTurn == Team.White ? "whitePlayer wins" : "blackPlayer wins";
            Debug.Log(win);
        }
        eatenPiece.coor = new int2(-1, -1);
        eatenPiece.team = currentTurn;

        if (eatenPiece.upgradable) //Eat Upgradable unupgradaded piece
        {
            eatenPiece.otherSidePiece.team = currentTurn;
        }

        if (eatenPiece.GetType().IsSubclassOf(typeof(UpgradedPiece))) { // Eat Upgraded Piece
            eatenPiece.otherSidePiece.team = currentTurn;
            eatenPiece = eatenPiece.otherSidePiece;
        }


        Player currentPlayer = currentTurn == Team.White ? whitePlayer : blackPlayer;

        switch (eatenPiece.type) {
            case PieceType.Pawn:
                currentPlayer.sideBoard.pawns.Enqueue((Pawn)eatenPiece);
                view.UpdateCemetary(currentTurn, eatenPiece.type, currentPlayer.sideBoard.pawns.Count);
                break;
            case PieceType.Spear:
                currentPlayer.sideBoard.spears.Enqueue((Spear)eatenPiece);
                view.UpdateCemetary(currentTurn, eatenPiece.type, currentPlayer.sideBoard.spears.Count);
                break;
            case PieceType.Horse:
                currentPlayer.sideBoard.horses.Enqueue((Horse)eatenPiece);
                view.UpdateCemetary(currentTurn, eatenPiece.type, currentPlayer.sideBoard.horses.Count);
                break;
            case PieceType.Silver:
                currentPlayer.sideBoard.silvers.Enqueue((Silver)eatenPiece);
                view.UpdateCemetary(currentTurn, eatenPiece.type, currentPlayer.sideBoard.silvers.Count);
                break;
            case PieceType.Gold:
                currentPlayer.sideBoard.golds.Enqueue((Gold)eatenPiece);
                view.UpdateCemetary(currentTurn, eatenPiece.type, currentPlayer.sideBoard.golds.Count);
                break;
            case PieceType.Tower:
                currentPlayer.sideBoard.towers.Enqueue((Tower)eatenPiece);
                view.UpdateCemetary(currentTurn, eatenPiece.type, currentPlayer.sideBoard.towers.Count);
                break;
            case PieceType.Bishop:
                currentPlayer.sideBoard.bishops.Enqueue((Bishop)eatenPiece);
                view.UpdateCemetary(currentTurn, eatenPiece.type, currentPlayer.sideBoard.bishops .Count);
                break;
        }
    }
    #endregion

    bool IsValidMove(int2 move)
    {
        foreach (int2 validMove in validMoves)
        {
            if (move.x != validMove.x) continue;
            if (move.y == validMove.y) return true;
        }
        return false;
    }

    void SelectNewPiece(Piece piece) {
        selectedPiece = piece;
        validMoves.Clear();
        List<int2> pieceMoves = selectedPiece.GetMoves();
        int2 pieceCoor = selectedPiece.coor;

        if (selectedPiece.GetType().IsSubclassOf(typeof(ComplexUpgradedPiece)))
        {
            ComplexUpgradedPiece complexPiece = (ComplexUpgradedPiece)piece;
            foreach (int2 move in complexPiece.GetComplexMoves().moves)
            {
                int2 newCoor;
                newCoor.y = move.x;
                newCoor.x = currentTurn == Team.White ? move.y : -move.y;
                newCoor += pieceCoor;
                if (newCoor.x < 0 || newCoor.x >= ROWS) continue;
                if (newCoor.y < 0 || newCoor.y >= COLS) continue;
                if (board.GetSquare(newCoor.x, newCoor.y).piece != null)
                {
                    if (board.GetSquare(newCoor.x, newCoor.y).piece.team == currentTurn) continue;
                }
                validMoves.Add(newCoor);
            }
            foreach (int2 direction in complexPiece.GetComplexMoves().directions)
            {
                for (int i = 1; i <= 8; i++)
                {
                    int2 newCoor = pieceCoor + direction * i;
                    if (newCoor.x < 0 || newCoor.x >= ROWS) break;
                    if (newCoor.y < 0 || newCoor.y >= COLS) break;
                    if (board.GetSquare(newCoor.x, newCoor.y).piece != null)
                    {
                        if (board.GetSquare(newCoor.x, newCoor.y).piece.team == currentTurn) break;
                        validMoves.Add(newCoor);
                        break;
                    }
                    validMoves.Add(newCoor);
                }
            }
        }
        else if (selectedPiece.GetType().IsSubclassOf(typeof(SingleMovePiece))) {
            foreach (int2 move in pieceMoves) {
                int2 newCoor;
                newCoor.y = move.x;
                newCoor.x = currentTurn == Team.White ? move.y : -move.y;
                newCoor += pieceCoor;
                if (newCoor.x < 0 || newCoor.x >= ROWS) continue;
                if (newCoor.y < 0 || newCoor.y >= COLS) continue;
                if (board.GetSquare(newCoor.x, newCoor.y).piece != null)
                {
                    if (board.GetSquare(newCoor.x, newCoor.y).piece.team == currentTurn) continue;
                }
                validMoves.Add(newCoor);
            }
        } else if (selectedPiece.GetType().IsSubclassOf(typeof(DirectionalMovePiece)))
        {
            foreach (int2 direction in pieceMoves)
            {
                for(int i = 1; i <= 8; i++)
                {
                    int2 newCoor = pieceCoor + direction * i;
                    if (newCoor.x < 0 || newCoor.x >= ROWS) break;
                    if (newCoor.y < 0 || newCoor.y >= COLS) break;
                    if (board.GetSquare(newCoor.x, newCoor.y).piece != null)
                    {
                        if (board.GetSquare(newCoor.x, newCoor.y).piece.team == currentTurn) break;
                        validMoves.Add(newCoor);
                        break;
                    }
                    validMoves.Add(newCoor);
                }
            }
        }
    }

    void MoveSelectedPiece(Square selectedSquare) {
        int2 prevPos = selectedPiece.coor; //
        if (selectedPiece.coor.x >= 0) RemovePiece(selectedPiece.coor);
        AddPiece(ref selectedPiece, selectedSquare.Coor);
        CheckForUpgradeRequirements(selectedPiece, prevPos); //
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

    void SwitchTeam() {
        currentTurn = currentTurn == Team.White ? Team.Black : Team.White;
        view.EnableTeamCemetary(currentTurn);
    }

    #region Piece Upgrading
    void UpgradePiece(Piece pieceToUpgrade) {
        if (!pieceToUpgrade.upgradable) return;
        RemovePiece(pieceToUpgrade.coor);
        AddPiece(ref pieceToUpgrade.otherSidePiece, pieceToUpgrade.coor);
        pieceToUpgrade.coor = new int2(-1, -1);
    }

    void CheckForUpgradeRequirements(Piece pieceToUpgrade, int2 prevPos)
    {
        if (!pieceToUpgrade.upgradable) return;
        if (prevPos.x < 0) return;
        int upperEnemyRow = currentTurn == Team.White ? 0 : ROWS - 3;
        int lowerEnemyRow = currentTurn == Team.White ? 2 : ROWS - 1;

        int pieceRow = pieceToUpgrade.coor.x;

        if (pieceRow >= upperEnemyRow && pieceRow <= lowerEnemyRow) UpgradePiece(pieceToUpgrade);
        else if(prevPos.x >= upperEnemyRow && prevPos.x <= lowerEnemyRow) UpgradePiece(pieceToUpgrade);
    }
    #endregion

    ~Controller(){}
}
