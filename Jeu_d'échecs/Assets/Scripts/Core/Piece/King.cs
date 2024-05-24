using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    private bool _canCastleQueenSide = true;
    private bool _canCastleKingSide = true;

    private Tile _kingSideRook = null;
    private Tile _queenSideRook = null;

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


    public void InitializeKingRooks()
    {
        Tile[,] boardPieces = _board.GetTiles;
        Team pieceTeam = Team;

        if (_kingSideRook == null && _queenSideRook == null)
        {
            int lastIndex = Board.boardSize - 1;

            if (pieceTeam == Team.White)
            {
                Debug.Log("team white");
                _queenSideRook = boardPieces[0, 0];

                _kingSideRook = boardPieces[0, lastIndex];
            }
            else
            {
                Debug.Log("team black");
                _queenSideRook = boardPieces[lastIndex, 0];

                _kingSideRook = boardPieces[lastIndex, lastIndex];
            }
        }

        Debug.Log("king: " + Team);
        Debug.Log("queen rook: " + _queenSideRook);
        Debug.Log("king rook: " + _kingSideRook);

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

        if (validateMoves)
        {
            _moveValidatorManager.ValidateMoves(this);
        }

        return _validTilesToMove;
    }

    public void LoseCastlingRightsKingSide()
    {
        _canCastleKingSide = false;
    }

    public void LoseCastlingRightsQueenSide()
    {
        _canCastleQueenSide = false;
    }
}
