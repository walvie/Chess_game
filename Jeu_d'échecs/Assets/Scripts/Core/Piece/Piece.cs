using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    enum Team
    {
        White = 0x1,
        Black = 0x2,
    }

    private Team _team;
    private Tile[][] _validTilesToMove;

    public abstract void GeneratePieceMove();

    public Tile[][] GetValidTilesToMove
    {
        get { return _validTilesToMove; }
    }
}
