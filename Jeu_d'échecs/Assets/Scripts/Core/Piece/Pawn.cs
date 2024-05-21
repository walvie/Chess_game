using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    private bool _hasMoved = false;

    private int _pieceFile;
    private int _pieceRank;
    
    private Tile[,] _gameTiles;

    private void Awake()
    {
        _gameTiles = GetBoardTiles();
        (_pieceFile, _pieceRank) = GetPieceTileIndexes();
    }

    public override void GeneratePieceMove()
    {
        int moveDirection = 1;

        // Move forward
        Tile moveTile = _gameTiles[_pieceFile + moveDirection, _pieceRank];

        if (moveTile.OccupyingPiece == null )
        {
            _validTilesToMove.Add(moveTile);

            if (_hasMoved == false)
            {
                moveTile = _gameTiles[_pieceFile + moveDirection * 2, _pieceRank];

                if (moveTile.OccupyingPiece == null)
                {
                    _validTilesToMove.Add(moveTile);
                }
            }
        }

        // Take diagonally
        moveTile = _gameTiles[_pieceFile + moveDirection, _pieceRank - 1];

        if (moveTile.OccupyingPiece.Team != _team)
        {
            _validTilesToMove.Add(moveTile);
        }

        moveTile = _gameTiles[_pieceFile + moveDirection, _pieceRank + 1];

        if (moveTile.OccupyingPiece.Team != _team)
        {
            _validTilesToMove.Add(moveTile);
        }
    }

    private (int, int) GetPieceTileIndexes()
    {
        Tile pieceTile = transform.parent.GetComponent<Tile>();

        for (int rank = 0; rank < _gameTiles.GetLength(0); rank++)
        {
            // Iterate over columns
            for (int file = 0; file < _gameTiles.GetLength(1); file++)
            {
                Tile currentTile = _gameTiles[file, rank];

                if (currentTile.Equals(pieceTile))
                { 
                    return (file, rank);
                }
            }
        }

        throw new Exception("Tile not found");
    }

    private Tile[,] GetBoardTiles()
    {
        // piece is a child of the tile, which is a child of the board.
        Board board = transform.parent.parent.GetComponent<Board>();

        return board.GetTiles;
    }
}
