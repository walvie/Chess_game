using System;
using System.Collections.Generic;

public class Pawn : Piece
{
    private bool _hasMoved = false;

    private void Awake()
    {
        InitializePieceVariables();
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

    public void SetPieceHasMoved()
    {
        _hasMoved = true;
    }
}