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
    protected Team _team;
    protected List<Tile> _validTilesToMove = new List<Tile>();

    protected Tile[,] _gameTiles;
    protected int _pieceFile;
    protected int _pieceRank;
    protected int _boardFileLimit;
    protected int _boardRankLimit;

    public PieceType pieceType;

    public abstract List<Tile> GeneratePieceMoves();

    protected void InitializePieceVariables()
    {
        _gameTiles = GetBoardTiles();
        (_pieceFile, _pieceRank) = GetPieceTileIndexes(_gameTiles);
        _boardFileLimit = _gameTiles.GetLength(0);
        _boardRankLimit = _gameTiles.GetLength(1);
    }

    /// <summary>
    /// Resets the pieces list of valid moves.
    /// </summary>
    public void ResetGeneratedMoves()
    {
        _validTilesToMove = new List<Tile>();
    }

    /// <summary>
    /// Checks if the specified file and rank are within the board limits.
    /// </summary>
    /// <param name="fileToMove">The file to move the piece to.</param>
    /// <param name="rankToMove">The rank to move the piece to.</param>
    /// <returns>True if the position is within the board limits, false otherwise.</returns>
    protected bool IsInBoardLimits(int fileToMove, int rankToMove)
    {
        bool isInBoardLimits = (fileToMove >= 0 && rankToMove >= 0 && fileToMove < _boardFileLimit && rankToMove < _boardRankLimit);

        return isInBoardLimits;
    }

    /// <summary>
    /// Finds and returns the indexes (file and rank) of the tile that the current piece is on.
    /// </summary>
    /// <param name="gameTiles">A 2D array of tiles representing the game board.</param>
    /// <returns>A tuple containing the file and rank of the tile the piece is on.</returns>
    protected (int, int) GetPieceTileIndexes(Tile[,] gameTiles)
    {
        Tile pieceTile = transform.parent.GetComponent<Tile>();

        for (int rank = 0; rank < gameTiles.GetLength(0); rank++)
        {
            // Iterate over columns
            for (int file = 0; file < gameTiles.GetLength(1); file++)
            {
                Tile currentTile = gameTiles[file, rank];

                if (currentTile.Equals(pieceTile))
                {
                    return (file, rank);
                }
            }
        }

        throw new Exception("Tile not found");
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