using System.Collections.Generic;

public class Bishop : Piece
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
            (1, -1),  // Up left
            (1, 1), // Up right
            (-1, -1),  // Down left
            (-1, 1)  // Down right
        };
    }

    public override List<Tile> GeneratePieceMoves(Piece[,] gamePieces)
    {
        (_pieceFile, _pieceRank) = GetPieceIndexes(_gameTiles);

        int fileToMove;
        int rankToMove;

        Tile moveTile;
        Piece movePositionPiece;

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

                if (Board.IsInBoardLimits(fileToMove, rankToMove))
                {
                    moveTile = _gameTiles[fileToMove, rankToMove];
                    movePositionPiece = moveTile.OccupyingPiece;

                    if (movePositionPiece == null || movePositionPiece.Team != _team)
                    {
                        _validTilesToMove.Add(moveTile);
                    }

                    if (movePositionPiece != null)
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