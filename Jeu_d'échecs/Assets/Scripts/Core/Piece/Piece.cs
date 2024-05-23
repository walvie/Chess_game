using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    public PieceType pieceType;

    protected abstract void InitializeDirections();

    public abstract List<Tile> GeneratePieceMoves(Piece[,] gamePieces);

    protected void InitializePieceVariables()
    {
        _gamePieces = GetBoardPieces();
        _gameTiles = GetBoardTiles();

        (_pieceFile, _pieceRank) = GetPieceIndexes(_gameTiles);
        _boardFileLimit = Board.boardSize;
        _boardRankLimit = Board.boardSize;
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
        foreach (Tile validTile in _validTilesToMove)
        {
            if (tile == validTile)
            {
                _validTilesToMove.Remove(validTile);
            }
        }
    }

    /// <summary>
    /// Finds and returns the indexes (file and rank) of the piece.
    /// </summary>
    /// <param name="gameTiles">A 2D array of the pieces representing the game board.</param>
    /// <returns>A tuple containing the file and rank of pieces position.</returns>
    protected (int, int) GetPieceIndexes(Tile[,] gameTiles)
    {
        Tile pieceTile = transform.parent.GetComponent<Tile>();

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

        throw new Exception("Piece not found");
    }

    /// <summary>
    /// Retrieves the pieces of the game board.
    /// </summary>
    /// <returns>A 2D array of the pieces representing the game board.</returns>
    protected Piece[,] GetBoardPieces()
    {
        // piece is a child of the tile, which is a child of the board.
        Board board = transform.parent.parent.GetComponent<Board>();

        return board.GetPieces;
    }

    /// <summary>
    /// Retrieves the tiles of the game board.
    /// </summary>
    /// <returns>A 2D array of tiles representing the game board.</returns>
    protected Tile[,] GetBoardTiles()
    {
        // piece is a child of the tile, which is a child of the board.
        Board board = transform.parent.parent.GetComponent<Board>();

        return board.GetTiles;
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