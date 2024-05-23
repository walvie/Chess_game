using System;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    private bool _hasMoved = false;

    private int _pieceFile;
    private int _pieceRank;
    private Tile[,] _gameTiles;

    private int _boardFileLimit;
    private int _boardRankLimit;

    private void Awake()
    {
        _gameTiles = GetBoardTiles();
        (_pieceFile, _pieceRank) = GetPieceTileIndexes();
        _boardFileLimit = _gameTiles.GetLength(0);
        _boardRankLimit = _gameTiles.GetLength(1);
    }

    public override List<Tile> GeneratePieceMoves()
    {
        (_pieceFile, _pieceRank) = GetPieceTileIndexes();

        int moveDirection = 1;

        if (_team == Team.Black)
        {
            moveDirection = -1;
        }

        int fileToMove = _pieceFile + moveDirection;
        int rankToMove = _pieceRank;

        Tile moveTile;

        // Move forward
        if (IsInBoardLimits(fileToMove, rankToMove))
        {
            moveTile = _gameTiles[fileToMove, rankToMove];

            if (moveTile.OccupyingPiece == null)
            {
                _validTilesToMove.Add(moveTile);

                fileToMove += moveDirection;

                if (_hasMoved == false && IsInBoardLimits(fileToMove, rankToMove))
                {
                    moveTile = _gameTiles[fileToMove, rankToMove];

                    if (moveTile.OccupyingPiece == null)
                    {
                        _validTilesToMove.Add(moveTile);
                    }
                }
            }
        }

        fileToMove = _pieceFile + moveDirection;
        rankToMove = _pieceRank - 1;

        // Take diagonally
        if (IsInBoardLimits(fileToMove, rankToMove))
        {
            moveTile = _gameTiles[fileToMove, rankToMove];

            if (moveTile.OccupyingPiece != null && moveTile.OccupyingPiece.Team != _team)
            {
                _validTilesToMove.Add(moveTile);
            }
        }

        rankToMove = _pieceRank + 1;

        if (IsInBoardLimits(fileToMove, rankToMove))
        {
            moveTile = _gameTiles[fileToMove, rankToMove];

            if (moveTile.OccupyingPiece != null && moveTile.OccupyingPiece.Team != _team)
            {
                _validTilesToMove.Add(moveTile);
            }
        }

        return _validTilesToMove;
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

    /// <summary>
    /// Checks if the specified file and rank are within the board limits.
    /// </summary>
    /// <param name="fileToMove">The file to move the piece to.</param>
    /// <param name="rankToMove">The rank to move the piece to.</param>
    /// <returns>True if the position is within the board limits, false otherwise.</returns>
    private bool IsInBoardLimits(int fileToMove, int rankToMove)
    {
        bool isInBoardLimits = (fileToMove >= 0 && rankToMove >= 0 && fileToMove < _boardFileLimit && rankToMove < _boardRankLimit);

        return isInBoardLimits;
    }

    private Tile[,] GetBoardTiles()
    {
        // piece is a child of the tile, which is a child of the board.
        Board board = transform.parent.parent.GetComponent<Board>();

        return board.GetTiles;
    }

    public void SetPieceHasMoved()
    {
        _hasMoved = true;
    }
}