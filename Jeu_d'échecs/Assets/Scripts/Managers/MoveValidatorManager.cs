using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveValidatorManager : MonoBehaviour
{
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    public bool ValidatePosition(Piece[,] boardPieces, Piece pieceToProtect)
    {
        foreach (var piece in boardPieces)
        {
            if (piece.Team != pieceToProtect.Team)
            {
                List<Tile> pieceMoves = piece.GeneratePieceMoves(boardPieces);

                foreach (Tile tileToMove in pieceMoves)
                {
                    if (tileToMove.OccupyingPiece == pieceToProtect)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public bool ValidateMoves(Piece pieceToMove)
    {
        Tile[,] boardTiles = GetBoardTiles();
        Piece[,] boardPieces = GetPieceArray(boardTiles);
        List<Tile> pieceMoves = pieceToMove.GetValidTilesToMoves;
        Tile pieceCurrentTile = pieceToMove.transform.parent.GetComponent<Tile>();

        string currentTilePosition = pieceCurrentTile.name;

        int[] pieceOriginalIndexes = TilePositionToIndexes(currentTilePosition);

        int originalFile = pieceOriginalIndexes[0];
        int originalRank = pieceOriginalIndexes[1];

        foreach (Tile move in pieceMoves)
        {
            bool positionIsValid = false;

            string pieceNewPosition = move.name;

            int[] pieceNewIndexes = TilePositionToIndexes(pieceNewPosition);

            int newFile = pieceNewIndexes[0];
            int newRank = pieceNewIndexes[1];

            Debug.Log(boardPieces);

            if (Board.IsInBoardLimits(newFile, newRank))
            {
                boardPieces[originalFile, originalRank] = null;
                boardPieces[newFile, newRank] = pieceToMove;

                PieceType typeOfPieceToProtect = (_gameManager.GetCurrentTurn ==  Team.White) ? PieceType.WhiteKing : PieceType.BlackKing;

                Piece pieceToProtect = FindPiece(typeOfPieceToProtect, boardPieces);

                positionIsValid = ValidatePosition(boardPieces, pieceToProtect);
            }
            else
            {
                Debug.LogError("Invalid move passed.");
            }

            if (!positionIsValid)
            {
                pieceToMove.RemoveTileAsValidMove(move);
            }
        }

        return true;
    }

    private Piece FindPiece(PieceType pieceToFind, Piece[,] pieceList)
    {
        foreach (Piece piece in pieceList)
        {
            if (piece.pieceType == pieceToFind)
            {
                return piece;
            }
        }

        return null;
    }

    private Tile[,] GetBoardTiles()
    {
        // piece is a child of the tile, which is a child of the board.
        Board board = transform.parent.parent.GetComponent<Board>();

        return board.GetTiles;
    }

    /// <summary>
    /// Converts a tile position in algebraic notation (e.g., "c6") to its corresponding indexes on the board.
    /// </summary>
    /// <param name="tilePosition"></param>
    /// <returns>An array containing the file index (horizontal) and the rank index (vertical).</returns>
    private int[] TilePositionToIndexes(string tilePosition)
    {
        int file = tilePosition[0] - 'a';

        int rank = int.Parse(tilePosition[1].ToString()) - 1;

        return new int[] { file, rank };
    }

    private Piece[,] GetPieceArray(Tile[,] boardTiles)
    {
        int boardSize = Board.boardSize;
        Piece[,] boardPieces = new Piece[boardSize, boardSize];
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                boardPieces[x, y] = boardTiles[x, y].OccupyingPiece;
            }
        }
        return boardPieces;
    }
}