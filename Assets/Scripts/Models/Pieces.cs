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
    King,
    UpPawn,
    UpSpear,
    UpHorse,
    UpSilver,
    UpTower,
    UpBishop
}

public enum Team
{
    White,
    Black
}

public abstract class Piece {
    public int2 coor;
    public readonly PieceType type;
    public Team team;
    public readonly bool upgradable;
    public Piece otherSidePiece;

    public Piece(int2 coor, PieceType type, Team team, bool upgreadable = false)
    {
        this.coor = coor;
        this.type = type;
        this.team = team;
        this.upgradable = upgreadable;
    }
    public abstract List<int2> GetMoves();
}

public abstract class SingleMovePiece : Piece {
    protected List<int2> moves = new List<int2>();
    public SingleMovePiece(int2 coor, PieceType type, Team team, bool upgreadable = false) : base(coor, type, team, upgreadable) { }

    public override List<int2> GetMoves()
    {
        return moves;
    }
}

public abstract class DirectionalMovePiece : Piece
{
    protected List<int2> directions = new List<int2>();
    public DirectionalMovePiece(int2 coor, PieceType type, Team team, bool upgreadable = false) : base(coor, type, team, upgreadable) { }
    public override List<int2> GetMoves()
    {
        return directions;
    }
}

public abstract class UpgradedPiece : SingleMovePiece {
    public UpgradedPiece(int2 coor, PieceType type, Team team, Piece normalSide) : base(coor, type, team) {
        otherSidePiece = normalSide;
    }
}

public abstract class ComplexUpgradedPiece : UpgradedPiece
{
    protected List<int2> directions = new List<int2>();

    public ComplexUpgradedPiece(int2 coor, PieceType type, Team team, Piece normalSide) : base(coor, type, team, normalSide) { }

    public (List<int2> directions, List<int2> moves) GetComplexMoves()
    {
        return (directions, moves);
    }
}

public class Pawn : SingleMovePiece {
    public Pawn(int2 coor, Team team) : base(coor, PieceType.Pawn, team, true) {
        moves = new List<int2>() {
            new int2(0, -1),
        };
        otherSidePiece = new UpGold(new int2(-1, -1), PieceType.UpPawn, team, this);
    }
}

public class Spear : DirectionalMovePiece {
    public Spear(int2 coor, Team team) : base(coor, PieceType.Spear, team, true) {
        directions = new List<int2>() {
            new int2(-1, 0),
        };
        otherSidePiece = new UpGold(new int2(-1, -1), PieceType.UpSpear, team, this);
    }
}

public class Horse : SingleMovePiece {
    public Horse(int2 coor, Team team) : base(coor, PieceType.Horse, team, true) {
        moves = new List<int2>() {
            new int2(-1, -2),
            new int2(1, -2),
        };
        otherSidePiece = new UpGold(new int2(-1, -1), PieceType.UpHorse, team, this);
    }
}

public class Silver : SingleMovePiece {
    public Silver(int2 coor, Team team) : base(coor, PieceType.Silver, team, true) {
        moves = new List<int2>() {
            new int2(-1, -1),
            new int2(0, -1),
            new int2(1, -1),
            new int2(-1, 1),
            new int2(1, 1),
        };
        otherSidePiece = new UpGold(new int2(-1, -1), PieceType.UpSilver, team, this);
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
    public Tower(int2 coor, Team team) : base(coor, PieceType.Tower, team, true) {
        directions = new List<int2>() {
            new int2(-1, 0),
            new int2(1, 0),
            new int2(0, -1),
            new int2(0, 1),
        };
        otherSidePiece = new UpTower(new int2(-1, -1), team, this);
    }

}

public class Bishop : DirectionalMovePiece {
    public Bishop(int2 coor, Team team) : base(coor, PieceType.Bishop, team, true) {
        directions = new List<int2>() {
            new int2(-1, -1),
            new int2(1, -1),
            new int2(-1, 1),
            new int2(1, 1),
        };
        otherSidePiece = new UpBishop(new int2(-1,-1), team, this);
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

public class UpGold : UpgradedPiece
{
    public UpGold(int2 coor, PieceType type, Team team, Piece normalSide) : base(coor, type, team, normalSide)
    {
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

public class UpTower : ComplexUpgradedPiece
{
    public UpTower(int2 coor, Team team, Piece normalSide) : base(coor, PieceType.UpTower, team, normalSide)
    {
        directions = new List<int2>() {
            new int2(-1, 0),
            new int2(1, 0),
            new int2(0, -1),
            new int2(0, 1),
        };
        moves = new List<int2>()
        {
            new int2(-1, -1),
            new int2(1, -1),
            new int2(-1, 1),
            new int2(1, 1),
        };
    }
}

public class UpBishop : ComplexUpgradedPiece
{
    public UpBishop(int2 coor, Team team, Piece normalSide) : base(coor, PieceType.UpBishop, team, normalSide)
    {
        directions = new List<int2>() {
            new int2(-1, -1),
            new int2(1, -1),
            new int2(-1, 1),
            new int2(1, 1),
        };
        moves = new List<int2>()
        {
            new int2(-1, 0),
            new int2(1, 0),
            new int2(0, -1),
            new int2(0, 1),
        };
    }
}
