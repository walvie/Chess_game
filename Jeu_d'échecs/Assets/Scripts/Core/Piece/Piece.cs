using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    private Team _team;
    private Tile[][] _validTilesToMove;

    public abstract void GeneratePieceMove();

    public Tile[][] GetValidTilesToMove
    {
        get { return _validTilesToMove; }
    }
}

public enum Team
{
    White,
    Black
}