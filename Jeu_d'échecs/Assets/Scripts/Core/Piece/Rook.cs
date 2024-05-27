using System.Collections.Generic;

public class Rook : Piece
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
            (-1, 0), // Down
            (0, 1),  // Right
            (0, -1)  // Left
        };
    }

    public override List<Tile> GeneratePieceMoves(Piece[,] gamePieces, bool validateMoves)
    {
        (_pieceFile, _pieceRank) = GetPieceIndexesFromPieces(gamePieces);

        int fileToMove;
        int rankToMove;

        Tile moveTile;
        Piece movePositionPiece;

        this.ResetGeneratedMoves();

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
                    movePositionPiece = gamePieces[fileToMove, rankToMove];
                    moveTile = _gameTiles[fileToMove, rankToMove];

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

        if (validateMoves)
        {
            _moveValidatorManager.ValidateMoves(this);
        }

        return _validTilesToMove;
    }

    /// <summary>
    /// Adds the tile on which the rook will move to if the player castles.
    /// </summary>
    /// <param name="castlingSquare">The tile to move the rook to.</param>
    public void AddCastlingSquare(Tile castlingSquare)
    {
        _validTilesToMove.Add(castlingSquare);
    }
}