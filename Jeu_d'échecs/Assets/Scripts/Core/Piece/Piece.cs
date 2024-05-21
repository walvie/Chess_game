using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    White,
    Black
}

public abstract class Piece : MonoBehaviour
{
    public Team team;
    protected List<Tile> _validTilesToMove = new List<Tile>();

    public PieceType pieceType;

    public abstract void GeneratePieceMove();

    public List<Tile> GetValidTilesToMove
    {
        get { return _validTilesToMove; }
    }
}