using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MoveValidatorManager : MonoBehaviour
{
    private GameManager _gameManager;
    private Board _board;
    private static MoveValidatorManager _instance;
    private List<Tile> reusableTileList = new List<Tile>();
    private Tile _piecePreviousPosition;
    private Tile _pieceNextPosition;
    private Piece _pieceToMove;

    public static MoveValidatorManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MoveValidatorManager>();

                if (_instance == null)
                {
                    GameObject moveValidatorManagerObject = new GameObject("MoveValidatorManager");
                    _instance = moveValidatorManagerObject.AddComponent<MoveValidatorManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _board = Board.Instance;
    }

    public bool ValidatePosition(Piece[,] boardPieces)
    {
        PieceType kingType = (_gameManager.GetCurrentTurn == Team.White) ? PieceType.WhiteKing : PieceType.BlackKing;

        Piece currentTeamKing = FindKing(boardPieces, kingType);

        if (currentTeamKing == null)
        {
            throw new Exception("Could not find teams king");
        }

        // Iterate over all of the current teams pieces
        foreach (Piece piece in boardPieces)
        {
            if (piece != null && piece.Team != currentTeamKing.Team)
            {
                // Get pieces possible moves
                reusableTileList.Clear();
                reusableTileList.AddRange(piece.GeneratePieceMoves(boardPieces, false));

                // Check if king is still in check
                foreach (Tile tileToMove in reusableTileList)
                {
                    int[] tileIndexes = Tile.TilePositionToIndexes(tileToMove);

                    int file = tileIndexes[0];
                    int rank = tileIndexes[1];

                    if (boardPieces[file, rank] == currentTeamKing)
                    {
                        reusableTileList.Clear();
                        return false;
                    }
                }
            }
        }

        reusableTileList.Clear();
        return true;
    }

    public bool ValidateMoves(Piece pieceToMove)
    {
        Tile[,] boardTiles = _board.GetTiles;
        Piece[,] boardPieces = _board.GetPieces;
        List<Tile> pieceMoves = pieceToMove.GetValidTilesToMoves;
        Tile pieceCurrentTile = pieceToMove.transform.parent.GetComponent<Tile>();

        _pieceToMove = pieceToMove;

        List<Tile> movesToRemove = new List<Tile>();

        int[] pieceOriginalIndexes = Tile.TilePositionToIndexes(pieceCurrentTile);

        int originalFile = pieceOriginalIndexes[0];
        int originalRank = pieceOriginalIndexes[1];

        foreach (Tile move in pieceMoves)
        {
            bool positionIsValid = false;

            int[] pieceNewIndexes = Tile.TilePositionToIndexes(move);

            int newFile = pieceNewIndexes[0];
            int newRank = pieceNewIndexes[1];

            if (Board.IsInBoardLimits(newFile, newRank))
            {
                _piecePreviousPosition = pieceCurrentTile;

                boardPieces[originalFile, originalRank] = null;
                boardPieces[newFile, newRank] = pieceToMove;

                _pieceNextPosition = boardTiles[newFile, newRank];

                positionIsValid = ValidatePosition(boardPieces);
            }
            else
            {
                Debug.LogError("Invalid move passed.");
            }

            if (!positionIsValid)
            {
                movesToRemove.Add(move);
            }

            boardPieces = _board.GetPieces;
        }

        RemoveInvalidMoves(movesToRemove, pieceToMove);

        return true;
    }

    private Piece FindKing(Piece[,] boardPieces, PieceType kingTeam)
    {
        foreach (Piece piece in boardPieces)
        {
            if (piece != null && piece.pieceType == kingTeam)
            {
                return piece;
            }
        }

        return null;
    }

    private void RemoveInvalidMoves(List<Tile> movesToRemove, Piece pieceToRemoveMoves)
    {
        foreach (Tile tile in movesToRemove)
        {
            pieceToRemoveMoves.RemoveTileAsValidMove(tile);
        }
    }
}