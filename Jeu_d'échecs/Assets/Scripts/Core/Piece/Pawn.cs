using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    private bool _hasMoved = false;

    private Tile _canTakeEnPassantTile;
    private Piece _pieceToTakeEnPassant;

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

        int moveDirection = _directions[0].Item1;

        if (_team == Team.Black)
        {
            moveDirection = _directions[1].Item1;
        }

        int fileToMove = _pieceFile + moveDirection;
        int rankToMove = _pieceRank;

        Tile moveTile;
        Piece movePositionPiece;

        this.ResetGeneratedMoves();

        if (_canTakeEnPassantTile)
        {
            _validTilesToMove.Add(_canTakeEnPassantTile);
        }

        // Move forward
        if (Board.IsInBoardLimits(fileToMove, rankToMove))
        {
            movePositionPiece = gamePieces[fileToMove, rankToMove];
            moveTile = _gameTiles[fileToMove, rankToMove];

            if (movePositionPiece == null)
            {
                _validTilesToMove.Add(moveTile);

                fileToMove += moveDirection;

                if (_hasMoved == false && Board.IsInBoardLimits(fileToMove, rankToMove))
                {
                    movePositionPiece = gamePieces[fileToMove, rankToMove];
                    moveTile = _gameTiles[fileToMove, rankToMove];

                    if (movePositionPiece == null)
                    {
                        _validTilesToMove.Add(moveTile);
                    }
                }
            }
        }

        fileToMove = _pieceFile + moveDirection;
        rankToMove = _pieceRank + _directions[2].Item2;

        // Take diagonally
        if (Board.IsInBoardLimits(fileToMove, rankToMove))
        {
            movePositionPiece = gamePieces[fileToMove, rankToMove];
            moveTile = _gameTiles[fileToMove, rankToMove];

            if (movePositionPiece != null && movePositionPiece.Team != _team)
            {
                _validTilesToMove.Add(moveTile);
            }
        }

        rankToMove = _pieceRank + _directions[3].Item2;

        if (Board.IsInBoardLimits(fileToMove, rankToMove))
        {
            movePositionPiece = gamePieces[fileToMove, rankToMove];
            moveTile = _gameTiles[fileToMove, rankToMove];

            if (movePositionPiece != null && movePositionPiece.Team != _team)
            {
                _validTilesToMove.Add(moveTile);
            }
        }

        if (validateMoves)
        {
            _moveValidatorManager.ValidateMoves(this);
        }

        return _validTilesToMove;
    }

    /// <summary>
    /// Check if there is an en passant move if the pawn has moved forwards two tiles.
    /// </summary>
    /// <param name="pawnTileToMove">The tile to check if the pawn has moved forwards two tiles.</param>
    public void CheckEnPassant(Tile pawnTileToMove)
    {
        Tile[,] boardTiles = _board.GetTiles;
        (_pieceFile, _pieceRank) = GetPieceIndexesFromTiles(boardTiles);

        int moveDirection = (_team == Team.Black) ? _directions[1].Item1 : _directions[0].Item1;

        int fileToMove = _pieceFile + moveDirection * 2;
        int rankToMove = _pieceRank;

        // If the pawn has moved two tiles forwards, then check for adjacent pawns
        if (Board.IsInBoardLimits(fileToMove, rankToMove) && boardTiles[fileToMove, rankToMove].Equals(pawnTileToMove))
        {
            Tile enPassantTile = boardTiles[fileToMove - moveDirection, rankToMove];
            CheckAdjacentPawn(fileToMove, rankToMove + _directions[2].Item2, enPassantTile);
            CheckAdjacentPawn(fileToMove, rankToMove + _directions[3].Item2, enPassantTile);
        }
    }

    /// <summary>
    /// Check the piece adjacent of the pawn if it is a pawn.
    /// </summary>
    /// <param name="file">The file to check if the piece is a pawn</param>
    /// <param name="rank">The rank to check if the piece is a pawn</param>
    /// <param name="enPassantTile">The original pawn tile to check the adjacent piece</param>
    private void CheckAdjacentPawn(int file, int rank, Tile enPassantTile)
    {
        Tile adjacentTile = (Board.IsInBoardLimits(file, rank)) ? _board.GetTiles[file, rank] : null;
        Piece adjacentPiece = (adjacentTile) ? adjacentTile.OccupyingPiece : null;

        if (adjacentPiece == null) return;

        // If the piece adjacent to the pawn is also a pawn, then define the adjacent pawn being able to take en passant.
        if (adjacentPiece is Pawn adjacentPawn &&
            (adjacentPawn.pieceType == PieceType.WhitePawn || adjacentPawn.pieceType == PieceType.BlackPawn))
        {
            adjacentPawn.SetEnPassant(enPassantTile, this);
        }
    }

    /// <summary>
    /// Define the pawns variables for en passant.
    /// </summary>
    /// <param name="canTakeTile">The tile which the pawn can move to take en passant.</param>
    /// <param name="pieceToTake">The piece to take if the pawn takes en passant.</param>
    public void SetEnPassant(Tile canTakeTile, Piece pieceToTake)
    {
        _canTakeEnPassantTile = canTakeTile;
        _pieceToTakeEnPassant = pieceToTake;
    }

    /// <summary>
    /// Define that the pawn has already moved, and so can't move forwards twice.
    /// </summary>
    public void SetPieceHasMoved()
    {
        _hasMoved = true;
    }

    /// <summary>
    /// Check if the pawn needs to promote.
    /// </summary>
    /// <param name="destinationTile">The tile to which the pawn will move to.</param>
    /// <returns>True if the pawn will promote, false otherwise.</returns>
    public bool CheckPromotion(Tile destinationTile)
    {
        int[] destinationTileIndexes = Tile.TilePositionToIndexes(destinationTile);

        int destinationFileIndex = destinationTileIndexes[0];

        bool pawnPromotion = false;

        // check if pawn has reached the end of the board. You don't need to check which direction since the pawn can't move backwards.
        if (destinationFileIndex == 0 || destinationFileIndex == Board.boardSize - 1)
        {
            pawnPromotion = true;
        }

        if (pawnPromotion)
        {
            this.Promote();
        }

        return pawnPromotion;
    }

    /// <summary>
    /// Promote the pawn to a queen.
    /// </summary>
    private void Promote()
    {
        Tile pawnTile = transform.parent.GetComponent<Tile>();
        PieceType pawnType = pieceType;

        PieceType queenType = (pawnType == PieceType.WhitePawn) ? PieceType.WhiteQueen : PieceType.BlackQueen;

        _board.CreatePiece(pawnTile, queenType);
    }

    /// <summary>
    /// Retrieve the <c>_canTakeEnPassantTile</c> varible.
    /// </summary>
    public Tile GetCanTakeEnPassantTile
    {
        get { return _canTakeEnPassantTile; }
    }

    /// <summary>
    /// Retrieve the <c>_pieceToTakeEnPassant</c> varible.
    /// </summary>
    public Piece GetPieceToTakeEnPassant
    {
        get { return _pieceToTakeEnPassant; }
    }
}