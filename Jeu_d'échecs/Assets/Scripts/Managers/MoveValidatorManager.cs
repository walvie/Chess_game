using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveValidatorManager : MonoBehaviour
{
    private GameManager _gameManager;
    private Board _board;
    private static MoveValidatorManager _instance;
    private List<Tile> reusableTileList = new List<Tile>();

    /// <summary>
    /// Get the <c>MoveValidatorManager</c> instance, or create if it doesn't exist.
    /// </summary>
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

    /// <summary>
    /// Check if the king is in check with the provided position.
    /// </summary>
    /// <param name="boardPieces">The pieces array to check if it is valid</param>
    /// <returns>True if the position is valid, False if it isn't</returns>
    /// <exception cref="Exception">If the king to check if the position is valid isn't found, then throw an exception</exception>
    public bool ValidatePosition(Piece[,] boardPieces)
    {
        PieceType kingType = (_gameManager.GetCurrentTurn == Team.White) ? PieceType.WhiteKing : PieceType.BlackKing;

        King currentTeamKing = Board.FindKing(boardPieces, kingType);

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

    /// <summary>
    /// Validate the generated moves of a piece.
    /// </summary>
    /// <param name="pieceToMove">The piece to check the generated moves</param>
    public void ValidateMoves(Piece pieceToMove)
    {
        Piece[,] boardPieces = _board.GetPieces;
        List<Tile> pieceMoves = pieceToMove.GetValidTilesToMoves;
        Tile pieceCurrentTile = pieceToMove.transform.parent.GetComponent<Tile>();
        List<Tile> movesToRemove = new List<Tile>();

        int[] pieceOriginalIndexes = Tile.TilePositionToIndexes(pieceCurrentTile);

        int originalFile = pieceOriginalIndexes[0];
        int originalRank = pieceOriginalIndexes[1];

        // Check if the position is valid, if the move was played.
        foreach (Tile move in pieceMoves)
        {
            bool positionIsValid = false;

            int[] pieceNewIndexes = Tile.TilePositionToIndexes(move);

            int newFile = pieceNewIndexes[0];
            int newRank = pieceNewIndexes[1];

            // Simulate the piece move and check if valid.
            if (Board.IsInBoardLimits(newFile, newRank))
            {
                boardPieces[originalFile, originalRank] = null;
                boardPieces[newFile, newRank] = pieceToMove;

                positionIsValid = ValidatePosition(boardPieces);
            }
            else
            {
                Debug.LogError("Invalid move passed.");
            }

            // if the move isn't valid, add the move to the moves to be removed.
            if (!positionIsValid)
            {
                movesToRemove.Add(move);
            }

            // Undo the simulated piece move.
            boardPieces = _board.GetPieces;
        }

        RemoveInvalidMoves(movesToRemove, pieceToMove);
    }

    /// <summary>
    /// Remove all the moves of the provided piece of the list provided.
    /// </summary>
    /// <param name="movesToRemove">The moves to remove from the piece.</param>
    /// <param name="pieceToRemoveMoves">The piece to remove the moves from.</param>
    private void RemoveInvalidMoves(List<Tile> movesToRemove, Piece pieceToRemoveMoves)
    {
        foreach (Tile tile in movesToRemove)
        {
            pieceToRemoveMoves.RemoveTileAsValidMove(tile);
        }
    }
}