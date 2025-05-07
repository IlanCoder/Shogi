using Unity.Mathematics;

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

public abstract class Piece
{
    public int2 coor;
    public PieceType type;
    public Team team;

    public Piece(int2 coor, PieceType type, Team team)
    {
        this.coor = coor;
        this.type = type;
        this.team = team;
    }
}

public class Pawn : Piece {
    public Pawn(int2 coor, Team team) : base(coor, PieceType.Pawn, team) {}
}

public class Spear : Piece {
    public Spear(int2 coor, Team team) : base(coor, PieceType.Spear, team) {}
}

public class Horse : Piece {
    public Horse(int2 coor, Team team) : base(coor, PieceType.Horse, team) {}
}

public class Silver : Piece {
    public Silver(int2 coor, Team team) : base(coor, PieceType.Silver, team) {}
}

public class Gold : Piece {
    public Gold(int2 coor, Team team) : base(coor, PieceType.Gold, team) {}
}

public class Tower : Piece {
    public Tower(int2 coor, Team team) : base(coor, PieceType.Tower, team) {}
}

public class Bishop : Piece {
    public Bishop(int2 coor, Team team) : base(coor, PieceType.Bishop, team) {}
}

public class King : Piece {
    public King(int2 coor, Team team) : base(coor, PieceType.King, team) {}
}
