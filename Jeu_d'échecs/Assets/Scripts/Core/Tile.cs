using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private Color _defaultColor;

    private Piece _occupyingPiece;

    private Dictionary<PieceType, Type> pieceComponentTypes = new Dictionary<PieceType, Type>
    {
        { PieceType.WhitePawn, typeof(Pawn) },
        { PieceType.BlackPawn, typeof(Pawn) },
        { PieceType.WhiteKnight, typeof(Knight) },
        { PieceType.BlackKnight, typeof(Knight) },
        { PieceType.WhiteBishop, typeof(Bishop) },
        { PieceType.BlackBishop, typeof(Bishop) },
        { PieceType.WhiteRook, typeof(Rook) },
        { PieceType.BlackRook, typeof(Rook) },
        { PieceType.WhiteQueen, typeof(Queen) },
        { PieceType.BlackQueen, typeof(Queen) },
        { PieceType.WhiteKing, typeof(King) },
        { PieceType.BlackKing, typeof(King) }
    };

    /// <summary>
    /// Place the provided piece <c>GameObject</c> on the tile.
    /// </summary>
    /// <param name="pieceObject">The <c>GameObject</c> to place on the tile.</param>
    /// <param name="isPromoting">If it is a pawn promoting, find the correct <c>Piece</c> component.</param>
    public void PlacePiece(GameObject pieceObject, bool isPromoting)
    {
        Piece pieceScript = pieceObject.GetComponent<Piece>();

        if (isPromoting)
        {
            Piece[] pieces = pieceObject.GetComponents<Piece>();

            foreach (Piece piece in pieces)
            {
                if (piece is Queen queen)
                {
                    pieceScript = queen;
                }
            }
        }

        _occupyingPiece = pieceScript;

        pieceObject.transform.SetParent(transform, false);
    }

    /// <summary>
    /// Initialize the piece on the tile according to the <c>PieceType</c>
    /// </summary>
    /// <param name="pieceToPlace">The type of the piece to place</param>
    public void InitializePiece(PieceType pieceToPlace = PieceType.None)
    {
        // Remove the game object if no piece type is provided.
        if (pieceToPlace == PieceType.None)
        {
            RemovePiece();

            return;
        }

        PieceType occupyingPieceType = pieceToPlace;
        GameObject pieceObject = transform.Find("Piece").gameObject;
        Image pieceImage = pieceObject.GetComponent<Image>();

        // Set piece sprite
        SpriteManager.Instance.SetImageSprite(occupyingPieceType, pieceImage);

        // Initialize piece script and tile variables
        bool hasRemovedScript = RemovePieceScript(pieceObject);
        Piece pieceScript = AddPieceScript(pieceObject, occupyingPieceType, hasRemovedScript);

        pieceObject.SetActive(true);

        _occupyingPiece = pieceScript;

        return;
    }

    /// <summary>
    /// Adds the corresponding script of the piece to the gameObject
    /// </summary>
    /// <param name="pieceObject">The GameObject representing the piece.</param>
    /// <param name="pieceType">The type of the piece to initialize.</param>
    private Piece AddPieceScript(GameObject pieceObject, PieceType pieceType, bool checkForMultipleScripts)
    {
        if (pieceComponentTypes.TryGetValue(pieceType, out Type componentType))
        {
            pieceObject.AddComponent(componentType);

            Piece pieceScript = pieceObject.GetComponent<Piece>();

            // If there are multiple scripts, that mean a pawn has promoted. Get the queen script instead of the pawn one.
            if (checkForMultipleScripts)
            {
                Piece[] pieces = pieceObject.GetComponents<Piece>();

                foreach (Piece piece in pieces)
                {
                    if (piece is Queen queen)
                    {
                        pieceScript = queen;
                    }
                }
            }

            pieceScript.pieceType = pieceType;

            // the first 7 pieceTypes are the white pieces, the others are black.
            pieceScript.Team = (((int)pieceType) < 7) ? Team.White : Team.Black;

            return pieceScript;
        }

        Debug.LogError($"Invalid piece type: {pieceType}");

        return null;
    }

    /// <summary>
    /// Check for a piece script and delete it if found. This will happen at the end of the frame.
    /// </summary>
    /// <param name="pieceObject"></param>
    /// <returns>If a piece script was removed</returns>
    private bool RemovePieceScript(GameObject pieceObject)
    {
        Piece pieceScript = pieceObject.GetComponent<Piece>();

        if (pieceScript != null)
        {
            Destroy(pieceScript);

            return true;
        }
        
        return false;
    }

    /// <summary>
    /// Converts a tile position in algebraic notation (e.g., "c6") to its corresponding indexes on the board.
    /// </summary>
    /// <param name="tilePosition"></param>
    /// <returns>An array containing the file index (horizontal) and the rank index (vertical).</returns>
    public static int[] TilePositionToIndexes(Tile tile)
    {
        string tileName = tile.name;

        int rank = tileName[0] - 'a';
        int file = int.Parse(tileName[1].ToString()) - 1;

        return new int[] { file, rank };
    }

    /// <summary>
    /// Remove the piece <c>GameObject</c>
    /// </summary>
    public void RemovePiece()
    {
        _occupyingPiece = null;

        Transform pieceTransform = transform.Find("Piece");

        if (pieceTransform == null) return;

        Destroy(pieceTransform.gameObject);
    }

    /// <summary>
    /// Change the tiles current color.
    /// </summary>
    /// <param name="color">The color to replace the current tile's color.</param>
    public void ChangeTileColor(Color32 color)
    {
        gameObject.GetComponent<Image>().color = color;
    }

    /// <summary>
    /// Get the <c>_occupyingPiece</c> attribute of the tile
    /// </summary>
    public Piece OccupyingPiece
    {
        get
        {
            return _occupyingPiece;
        }
    }

    /// <summary>
    /// Get the <c>_defaultColor</c> of the tile
    /// </summary>
    public Color DefaultColor
    {
        get { return _defaultColor; }
        set { _defaultColor = value; }
    }
}
