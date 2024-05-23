using System.Collections.Generic;

public class King : Piece
{
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
            (1, 1), // Up right
            (0, 1), // Right
            (-1, 1),  // Down right
            (-1, 0),  // Down
            (-1, -1),  // Down left
            (0, -1),  // Left
            (1, -1)  // Up left
        };
    }

    public override List<Tile> GeneratePieceMoves()
    {
        (_pieceFile, _pieceRank) = GetPieceTileIndexes(_gameTiles);

        int fileToMove;
        int rankToMove;

        Tile moveTile;

        for (int i = 0; i < _directions.Length; i++)
        {
            (int, int) direction = _directions[i];

            fileToMove = _pieceFile;
            rankToMove = _pieceRank;

            fileToMove += direction.Item1;
            rankToMove += direction.Item2;

            if (IsInBoardLimits(fileToMove, rankToMove))
            {
                moveTile = _gameTiles[fileToMove, rankToMove];

                if (moveTile.OccupyingPiece == null || moveTile.OccupyingPiece.Team != _team)
                {
                    _validTilesToMove.Add(moveTile);
                }
            }
        }

        return _validTilesToMove;
    }
}
