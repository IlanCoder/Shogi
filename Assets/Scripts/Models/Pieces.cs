using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.TextCore.Text;

public enum PieceType {
    Pawn,
    Spear,
    Horse,
    Silver,
    Gold,
    Tower,
    Bishop,
    King
}

public enum Team
{
    White,
    Black
}

public abstract class Piece {
    public int2 coor;
    public PieceType type;
    public Team team;

    public Piece(int2 coor, PieceType type, Team team)
    {
        this.coor = coor;
        this.type = type;
        this.team = team;
    }
    public abstract List<int2> GetMoves();
}

public abstract class SingleMovePiece : Piece {
    protected List<int2> moves = new List<int2>();
    public SingleMovePiece(int2 coor, PieceType type, Team team) : base(coor, type, team) { }

    public override List<int2> GetMoves()
    {
        return moves;
    }
}

public abstract class DirectionalMovePiece : Piece
{
    protected List<int2> directions = new List<int2>();
    public DirectionalMovePiece(int2 coor, PieceType type, Team team) : base(coor, type, team) { }
    public override List<int2> GetMoves()
    {
        return directions;
    }
}

public class Pawn : SingleMovePiece {
    public Pawn(int2 coor, Team team) : base(coor, PieceType.Pawn, team) {
        moves = new List<int2>() {
            new int2(0, -1),
        };
    }
}

public class Spear : DirectionalMovePiece {
    public Spear(int2 coor, Team team) : base(coor, PieceType.Spear, team) {
        directions = new List<int2>() {
            new int2(-1, 0),
        };
    }
}

public class Horse : SingleMovePiece {
    public Horse(int2 coor, Team team) : base(coor, PieceType.Horse, team) {
        moves = new List<int2>() {
            new int2(-1, -2),
            new int2(1, -2),
        };
    }
}

public class Silver : SingleMovePiece {
    public Silver(int2 coor, Team team) : base(coor, PieceType.Silver, team) {
        moves = new List<int2>() {
            new int2(-1, -1),
            new int2(0, -1),
            new int2(1, -1),
            new int2(-1, 1),
            new int2(1, 1),
        };
    }
}

public class Gold : SingleMovePiece {
    public Gold(int2 coor, Team team) : base(coor, PieceType.Gold, team) {
        moves = new List<int2>() {
            new int2(-1, -1),
            new int2(0, -1),
            new int2(1, -1),
            new int2(-1, 0),
            new int2(1, 0),
            new int2(0, 1),
        };
    }
}

public class Tower : DirectionalMovePiece {
    public Tower(int2 coor, Team team) : base(coor, PieceType.Tower, team) {
        directions = new List<int2>() {
            new int2(-1, 0),
            new int2(1, 0),
            new int2(0, -1),
            new int2(0, 1),
        };
    }

}

public class Bishop : DirectionalMovePiece {
    public Bishop(int2 coor, Team team) : base(coor, PieceType.Bishop, team) {
        directions = new List<int2>() {
            new int2(-1, -1),
            new int2(1, -1),
            new int2(-1, 1),
            new int2(1, 1),
        };
    }
}

public class King : SingleMovePiece {
    public King(int2 coor, Team team) : base(coor, PieceType.King, team) {
        moves = new List<int2>() {
            new int2(-1, -1),
            new int2(0, -1),
            new int2(1, -1),
            new int2(-1, 0),
            new int2(1, 0),
            new int2(-1, 1),
            new int2(0, 1),
            new int2(1, 1),
        };
    }
}
