using System.Collections.Generic;

public class Rook : Piece
{
    private (int, int)[] _directions =
    {
        (1, 0),  // Up
        (-1, 0), // Down
        (0, 1),  // Right
        (0, -1)  // Left
    };

    private void Awake()
    {
        InitializePieceVariables();
    }

    public override List<Tile> GeneratePieceMoves()
    {
        (_pieceFile, _pieceRank) = GetPieceTileIndexes(_gameTiles);
        
        int fileToMove;
        int rankToMove;

        Tile moveTile;

        for (int i = 0; i < 4; i++)
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