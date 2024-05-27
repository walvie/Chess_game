using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    private bool _canCastleQueenSide = true;
    private bool _canCastleKingSide = true;

    private Tile _kingSideRook = null;
    private Tile _queenSideRook = null;

    private const int KingSideRookTilesAmount = 2;
    private const int QueenSideRookTilesAmount = 3;
    private const int KingCastlingDistance = 2;

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

    /// <summary>
    /// Initialize the tile on which the rooks of the king is on.
    /// Used later for the kings castling logic.
    /// </summary>
    public void InitializeKingRooks()
    {
        Tile[,] boardPieces = _board.GetTiles;
        Team pieceTeam = Team;

        if (_kingSideRook == null && _queenSideRook == null)
        {
            int lastIndex = Board.boardSize - 1;

            if (pieceTeam == Team.White)
            {
                _queenSideRook = boardPieces[0, 0];

                _kingSideRook = boardPieces[0, lastIndex];
            }
            else
            {
                _queenSideRook = boardPieces[lastIndex, 0];

                _kingSideRook = boardPieces[lastIndex, lastIndex];
            }
        }
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
            (int, int) direction = _directions[i];

            fileToMove = _pieceFile;
            rankToMove = _pieceRank;

            fileToMove += direction.Item1;
            rankToMove += direction.Item2;

            if (Board.IsInBoardLimits(fileToMove, rankToMove))
            {
                movePositionPiece = gamePieces[fileToMove, rankToMove];
                moveTile = _gameTiles[fileToMove, rankToMove];

                if (movePositionPiece == null || movePositionPiece.Team != _team)
                {
                    _validTilesToMove.Add(moveTile);
                }
            }
        }

        CheckCastleMoves(gamePieces);

        if (validateMoves)
        {
            _moveValidatorManager.ValidateMoves(this);
        }

        return _validTilesToMove;
    }

    /// <summary>
    /// Check if the king can castle on either side.
    /// </summary>
    /// <param name="gamePieces">The piece list from which to check if the king can castle</param>
    private void CheckCastleMoves(Piece[,] gamePieces)
    {
        // King side castling
        if (_canCastleKingSide)
        {
            bool canCastle = true;

            // Check if no pieces are between the king and the rook
            for (int i = 1; i <= KingSideRookTilesAmount; i++)
            {
                if (gamePieces[_pieceFile, _pieceRank + i] != null)
                {
                    canCastle = false;
                    break;
                }
            }

            // Adds castling as a valid move.
            if (canCastle)
            {
                int rankToMove = _pieceRank + KingCastlingDistance;
                Tile moveTile = _gameTiles[_pieceFile, rankToMove];
                Tile rookMoveTile = _gameTiles[_pieceFile, rankToMove - 1];
                Rook rook = _kingSideRook.OccupyingPiece as Rook;

                _validTilesToMove.Add(moveTile);

                rook.AddCastlingSquare(rookMoveTile);
            }
        }

        // Queen side castling
        if (_canCastleQueenSide)
        {
            bool canCastle = true;

            // Check if no pieces are between the king and the rook
            for (int i = 1; i <= QueenSideRookTilesAmount; i++)
            {
                if (gamePieces[_pieceFile, _pieceRank - i] != null)
                {
                    canCastle = false;
                    break;
                }
            }

            // Adds castling as a valid move.
            if (canCastle)
            {
                int rankToMove = _pieceRank - KingCastlingDistance;
                Tile moveTile = _gameTiles[_pieceFile, rankToMove];
                Tile rookMoveTile = _gameTiles[_pieceFile, rankToMove + 1];
                Rook rook = _kingSideRook.OccupyingPiece as Rook;

                _validTilesToMove.Add(moveTile);

                rook.AddCastlingSquare(rookMoveTile);
            }
        }
    }

    /// <summary>
    /// Check if the current move being made is a castle.
    /// </summary>
    /// <param name="destinationTile">The tile on which the king is being moved to.</param>
    public void CheckIfMoveIsCastling(Tile destinationTile)
    {
        if (!_canCastleKingSide && !_canCastleQueenSide) return;

        Tile[,] gameTiles = _gameTiles;

        int queenSideRankToMove = _pieceRank - KingCastlingDistance;
        int kingSideRankToMove = _pieceRank + KingCastlingDistance;


        Tile queenSideCastleTile = (Board.IsInBoardLimits(_pieceFile, queenSideRankToMove)) ? gameTiles[_pieceFile, queenSideRankToMove] : null;
        Tile kingSideCastleTile = (Board.IsInBoardLimits(_pieceFile, kingSideRankToMove)) ? gameTiles[_pieceFile, kingSideRankToMove] : null;

        // If the king is castling, move the rook on the correct tile.
        if (queenSideCastleTile == destinationTile)
        {
            _board.MovePiece(_queenSideRook, _gameTiles[_pieceFile, queenSideRankToMove + 1], false);
        }

        if (kingSideCastleTile == destinationTile)
        {
            _board.MovePiece(_kingSideRook, _gameTiles[_pieceFile, kingSideRankToMove - 1], false);
        }
    }

    /// <summary>
    /// Check if the rook has moved. If it has, then lose the castling rights for that side.
    /// </summary>
    /// <param name="rookCurrentTile">The rooks current tile</param>
    public void RookMoved(Tile rookCurrentTile)
    {
        Tile[,] boardPieces = _board.GetTiles;

        bool queenSideRookMoved = false;
        bool kingSideRookMoved = false;

        Team pieceTeam = Team;

        int lastIndex = Board.boardSize - 1;

        if (pieceTeam == Team.White)
        {
            Tile queenSideRookTile = boardPieces[0, 0];

            Tile kingSideRookTile = boardPieces[0, lastIndex];

            if (queenSideRookTile == rookCurrentTile)
            {
                queenSideRookMoved = true;
            }

            if (kingSideRookTile == rookCurrentTile)
            {
                kingSideRookMoved = true;
            }
        }
        else
        {
            Tile queenSideRookTile = boardPieces[lastIndex, 0];

            Tile kingSideRookTile = boardPieces[lastIndex, lastIndex];

            if (queenSideRookTile == rookCurrentTile)
            {
                queenSideRookMoved = true;
            }

            if (kingSideRookTile == rookCurrentTile)
            {
                kingSideRookMoved = true;
            }
        }

        if (queenSideRookMoved)
        {
            this.LoseCastlingRightsQueenSide();
        }

        if (kingSideRookMoved)
        {
            this.LoseCastlingRightsKingSide();
        }
    }

    /// <summary>
    /// Lose the right to castle king side.
    /// </summary>
    public void LoseCastlingRightsKingSide()
    {
        Debug.Log("lost castling rights");
        _canCastleKingSide = false;
    }

    /// <summary>
    /// Lose the right to castle queen side.
    /// </summary>
    public void LoseCastlingRightsQueenSide()
    {
        Debug.Log("lost castling rights");
        _canCastleQueenSide = false;
    }
}
