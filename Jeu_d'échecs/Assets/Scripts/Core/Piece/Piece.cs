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

    /// <summary>
    /// Initialize the directions of which the piece can move.
    /// </summary>
    protected abstract void InitializeDirections();

    /// <summary>
    /// Generate the pieces possible pseudo-legal moves according to the pieces provided.
    /// </summary>
    /// <param name="gamePieces">The piece list from which to generate the pseudo-legal moves</param>
    /// <param name="validateMoves">If the generated moves should be validated to ensure that they are legal or not</param>
    /// <returns>The generated tiles that the pieces can move to</returns>
    public abstract List<Tile> GeneratePieceMoves(Piece[,] gamePieces, bool validateMoves);

    /// <summary>
    /// Initialize the necessary piece varaibles
    /// </summary>
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

        // Iterate over all of the board tiles to retrieve the file and rank indexes of the piece's tile.
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

        // Iterate over all of the provided pieces to retrieve the file and rank indexes of that piece.
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

    /// <summary>
    /// Resets all the pawns en passant status.
    /// </summary>
    /// <param name="boardPieces">The boards pieces that it should reset the pawns en passant status.</param>
    public static void ResetEnPassant(Piece[,] boardPieces)
    {
        // Iterate over the provided pieces, find all the pawns and reset their en passant status.
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

    /// <summary>
    /// Retrieve the <c>_validTilesToMove</c> variable of the piece.
    /// </summary>
    public List<Tile> GetValidTilesToMoves
    {
        get { return _validTilesToMove; }
    }

    /// <summary>
    /// Retrieve the <c>_team</c> variable of the piece.
    /// </summary>
    public Team Team
    {
        get { return _team; }
        set { _team = value; }
    }
}