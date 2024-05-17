using System;
using System.Collections;
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

    public abstract void GeneratePieceMove();

    public List<Tile> GetValidTilesToMove
    {
        get { return _validTilesToMove; }
    }

    public Team GetTeam
    {
        get { return _team; }
    }
}