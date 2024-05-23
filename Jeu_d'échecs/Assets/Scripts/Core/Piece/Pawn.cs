using System.Collections.Generic;

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

    public override List<Tile> GeneratePieceMoves()
    {
        (_pieceFile, _pieceRank) = GetPieceTileIndexes(_gameTiles);

        int moveDirection = _directions[0].Item1;

        if (_team == Team.Black)
        {
            moveDirection = _directions[1].Item1;
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
        rankToMove = _pieceRank + _directions[2].Item2;

        // Take diagonally
        if (IsInBoardLimits(fileToMove, rankToMove))
        {
            moveTile = _gameTiles[fileToMove, rankToMove];

            if (moveTile.OccupyingPiece != null && moveTile.OccupyingPiece.Team != _team)
            {
                _validTilesToMove.Add(moveTile);
            }
        }

        rankToMove = _pieceRank + _directions[3].Item2;

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

    public void SetPieceHasMoved()
    {
        _hasMoved = true;
    }
}