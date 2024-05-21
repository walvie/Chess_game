using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    White,
    Black
}

public abstract class Piece : MonoBehaviour
{
    protected Team _team;
    protected List<Tile> _validTilesToMove = new List<Tile>();

    public PieceType pieceType;

    public abstract List<Tile> GeneratePieceMoves();

    public List<Tile> GetValidTilesToMoves
    {
        get { return _validTilesToMove; }
    }

    public Team Team
    {
        get { return _team; }
        set { _team = value; }
    }
}