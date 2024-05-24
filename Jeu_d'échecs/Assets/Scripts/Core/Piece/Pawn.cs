using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    private bool _hasMoved = false;

    private void Awake()
    {
        InitializePieceVariables();
        InitializeDirections();
    }
    protected override void InitializeDirections()
    {
        _directions = new (int, int)[]
        {
            (1, 0),  // Up
            (-1, 0), // Down
            (0, 1),  // Right
            (0, -1)  // Left
        };
    }

    public override List<Tile> GeneratePieceMoves(Piece[,] gamePieces, bool validateMoves)
    {
        (_pieceFile, _pieceRank) = GetPieceIndexesFromPieces(gamePieces);

        int moveDirection = _directions[0].Item1;

        if (_team == Team.Black)
        {
            moveDirection = _directions[1].Item1;
        }

        int fileToMove = _pieceFile + moveDirection;
        int rankToMove = _pieceRank;

        Tile moveTile;
        Piece movePositionPiece;

        this.ResetGeneratedMoves();

        // Move forward
        if (Board.IsInBoardLimits(fileToMove, rankToMove))
        {
            movePositionPiece = gamePieces[fileToMove, rankToMove];
            moveTile = _gameTiles[fileToMove, rankToMove];

            if (movePositionPiece == null)
            {
                _validTilesToMove.Add(moveTile);

                fileToMove += moveDirection;

                if (_hasMoved == false && Board.IsInBoardLimits(fileToMove, rankToMove))
                {
                    movePositionPiece = gamePieces[fileToMove, rankToMove];
                    moveTile = _gameTiles[fileToMove, rankToMove];

                    if (movePositionPiece == null)
                    {
                        _validTilesToMove.Add(moveTile);
                    }
                }
            }
        }

        fileToMove = _pieceFile + moveDirection;
        rankToMove = _pieceRank + _directions[2].Item2;

        // Take diagonally
        if (Board.IsInBoardLimits(fileToMove, rankToMove))
        {
            movePositionPiece = gamePieces[fileToMove, rankToMove];
            moveTile = _gameTiles[fileToMove, rankToMove];

            if (movePositionPiece != null && movePositionPiece.Team != _team)
            {
                _validTilesToMove.Add(moveTile);
            }
        }

        rankToMove = _pieceRank + _directions[3].Item2;

        if (Board.IsInBoardLimits(fileToMove, rankToMove))
        {
            movePositionPiece = gamePieces[fileToMove, rankToMove];
            moveTile = _gameTiles[fileToMove, rankToMove];

            if (movePositionPiece != null && movePositionPiece.Team != _team)
            {
                _validTilesToMove.Add(moveTile);
            }
        }

        if (validateMoves)
        {
            _moveValidatorManager.ValidateMoves(this);
        }

        return _validTilesToMove;
    }

    public void SetPieceHasMoved()
    {
        _hasMoved = true;
    }
}