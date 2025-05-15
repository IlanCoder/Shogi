using System.Collections.Generic;

public class Player {
    public Team team { get; private set; }
    public SideBoard sideBoard { get; private set; }

    public Player(Team team) { 
        this.team = team;
        sideBoard = new SideBoard();
    }
}

public class SideBoard {
    public Queue<Pawn> pawns = new Queue<Pawn>();
    public Queue<Spear> spears = new Queue<Spear>();
    public Queue<Horse> horses = new Queue<Horse>();
    public Queue<Silver> silvers = new Queue<Silver>();
    public Queue<Gold> golds = new Queue<Gold>();
    public Queue<Tower> towers = new Queue<Tower>();
    public Queue<Bishop> bishops = new Queue<Bishop>();
}
