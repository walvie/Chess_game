using System;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    White,
    Black
}

public abstract class Piece : MonoBehaviour
{
    protected (int, int)[] _directions;

    protected Team _team;
    protected List<Tile> _validTilesToMove = new List<Tile>();

    protected Piece[,] _gamePieces;
    protected Tile[,] _gameTiles;

    protected int _pieceFile;
    protected int _pieceRank;
    protected int _boardFileLimit;
    protected int _boardRankLimit;

    protected MoveValidatorManager _moveValidatorManager;
    protected Board _board;

    public PieceType pieceType;

    protected abstract void InitializeDirections();

    public abstract List<Tile> GeneratePieceMoves(Piece[,] gamePieces, bool validateMoves);

    protected void InitializePieceVariables()
    {
        _board = Board.Instance;
        _gameTiles = _board.GetTiles;

        (_pieceFile, _pieceRank) = GetPieceIndexesFromTiles(_gameTiles);
        _boardFileLimit = Board.boardSize;
        _boardRankLimit = Board.boardSize;
        _moveValidatorManager = MoveValidatorManager.Instance;
    }

    /// <summary>
    /// Resets the pieces list of valid moves.
    /// </summary>
    public void ResetGeneratedMoves()
    {
        _validTilesToMove = new List<Tile>();
    }

    /// <summary>
    /// Finds and removes the tile in <c>_validTilesToMove</c> as a possible move for that piece
    /// </summary>
    /// <param name="tile"></param>
    public void RemoveTileAsValidMove(Tile tile)
    {
        // Used reverse iteration so that when the list is modified during the iteration, it won't cause errors.
        for (int i = _validTilesToMove.Count - 1; i >= 0; i--)
        {
            if (_validTilesToMove[i] == tile)
            {
                _validTilesToMove.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Finds and returns the indexes (file and rank) of the piece.
    /// </summary>
    /// <param name="gameTiles">A 2D array of the tiles representing the game board.</param>
    /// <returns>A tuple containing the file and rank of the pieces position according to the list provided.</returns>
    protected (int, int) GetPieceIndexesFromTiles(Tile[,] gameTiles)
    {
        Tile pieceTile = GetTile(this);

        for (int rank = 0; rank < gameTiles.GetLength(0); rank++) 
        {
            for (int file = 0; file < gameTiles.GetLength(1); file++)
            {
                Tile currentTile = gameTiles[file, rank];

                if (currentTile.Equals(pieceTile))
                {
                    return (file, rank);
                }
            }
        }

        throw new Exception("Piece tile not found");
    }

    /// <summary>
    /// Finds and returns the indexes (file and rank) of the piece from a piece list.
    /// </summary>
    /// <param name="gamePieces">A 2D array of the pieces representing the game board.</param>
    /// <returns>A tuple containing the file and rank of pieces position according to the list provided.</returns>
    protected (int, int) GetPieceIndexesFromPieces(Piece[,] gamePieces)
    {
        Piece piece = this;

        for (int rank = 0; rank < gamePieces.GetLength(0); rank++)
        {
            for (int file = 0; file < gamePieces.GetLength(1); file++)
            {
                Piece currentPiece = gamePieces[file, rank];

                if (currentPiece != null && currentPiece.Equals(piece))
                {
                    return (file, rank);
                }
            }
        }

        throw new Exception("Piece not found");
    }

    /// <summary>
    /// Finds the tile on which the piece is on.
    /// </summary>
    /// <param name="piece">The piece to find the tile.</param>
    /// <returns>The tile on which the piece is on</returns>
    public static Tile GetTile(Piece piece)
    {
        if (piece == null)
        {
            throw null;
        }

        Tile pieceTile = piece.transform.parent.GetComponent<Tile>();

        return pieceTile;
    }

    public static string GetTileNameFromPiece(Piece piece, Piece[,] gamePieces)
    {
        string position = "a1";

        for (int rank = 0; rank < gamePieces.GetLength(0); rank++)
        {
            for (int file = 0; file < gamePieces.GetLength(1); file++)
            {
                Piece currentPiece = gamePieces[file, rank];

                if (currentPiece != null && currentPiece.Equals(piece))
                {
                    char rankChar = (char)('a' + rank);
                    string fileString = (file + 1).ToString();

                    position = rankChar + fileString;
                }
            }
        }

        return position;
    }

    /// <summary>
    /// Resets all the pawns en passant status.
    /// </summary>
    /// <param name="boardPieces">The boards pieces that it should reset the pawns en passant status.</param>
    public static void ResetEnPassant(Piece[,] boardPieces)
    {
        for (int rank = 0; rank < boardPieces.GetLength(0); rank++)
        {
            for (int file = 0; file < boardPieces.GetLength(1); file++)
            {
                Piece piece = boardPieces[file, rank];

                if (piece != null && (piece.pieceType == PieceType.WhitePawn || piece.pieceType == PieceType.BlackPawn))
                {
                    Pawn pawn = piece as Pawn;

                    pawn.SetEnPassant(null, null);
                }
            }
        }
    }

    public List<Tile> GetValidTilesToMoves
    {
        get { return _validTilesToMove; }
    }

    public Team Team
    {
        get { return _team; }
        set { _team = value; }
    }
}