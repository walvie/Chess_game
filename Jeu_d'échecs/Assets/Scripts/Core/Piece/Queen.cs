using System.Collections.Generic;

public class Queen : Piece
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
            bool hasReachedEndOfGeneration = false;
            int iterationCount = 1;

            (int, int) direction = _directions[i];

            do
            {
                fileToMove = _pieceFile;
                rankToMove = _pieceRank;

                fileToMove += direction.Item1 * iterationCount;
                rankToMove += direction.Item2 * iterationCount;

                if (IsInBoardLimits(fileToMove, rankToMove))
                {
                    moveTile = _gameTiles[fileToMove, rankToMove];

                    if (moveTile.OccupyingPiece == null || moveTile.OccupyingPiece.Team != _team)
                    {
                        _validTilesToMove.Add(moveTile);
                    }

                    if (moveTile.OccupyingPiece != null)
                    {
                        hasReachedEndOfGeneration = true;
                    }
                }
                else
                {
                    hasReachedEndOfGeneration = true;
                }

                ++iterationCount;
            }
            while (!hasReachedEndOfGeneration);
        }

        return _validTilesToMove;
    }
}