using System.Collections.Generic;

public class Knight : Piece
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
            (2, -1),  // Up left
            (2, 1), // Up right
            (1, 2), // Right up
            (-1, 2),  // Right down
            (-2, 1),  // Down right
            (-2, -1),  // Down left
            (-1, -2),  // Left down
            (1, -2)  // Left up
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