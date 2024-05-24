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

    public Piece OccupyingPiece
    {
        get { return _occupyingPiece; }
        set { _occupyingPiece = value; }
    }

    public Color DefaultColor
    {
        get { return _defaultColor; }
        set { _defaultColor = value; }
    }

    public void PlacePiece(GameObject pieceObject)
    {
        Piece pieceScript = pieceObject.GetComponent<Piece>();
        _occupyingPiece = pieceScript;

        pieceObject.transform.SetParent(transform, false);
    }

    public Piece InitializePiece(PieceType pieceToPlace)
    {
        if (pieceToPlace == PieceType.None)
        {
            Debug.Log(gameObject.name);
            RemovePiece();

            return null;
        }

        PieceType occupyingPieceType = pieceToPlace;
        GameObject pieceObject = transform.Find("Piece").gameObject;
        Image pieceImage = pieceObject.GetComponent<Image>();

        if (occupyingPieceType != PieceType.None)
        {
            SpriteManager.Instance.SetImageSprite(occupyingPieceType, pieceImage);
        }

        Piece pieceScript = AddPieceScript(pieceObject, occupyingPieceType);

        pieceObject.SetActive(true);

        _occupyingPiece = pieceScript;

        return _occupyingPiece;
    }

    /// <summary>
    /// Adds the corresponding script of the piece to the gameObject
    /// </summary>
    /// <param name="pieceObject">The GameObject representing the piece.</param>
    /// <param name="pieceType">The type of the piece to initialize.</param>
    private Piece AddPieceScript(GameObject pieceObject, PieceType pieceType)
    {
        if (pieceComponentTypes.TryGetValue(pieceType, out Type componentType))
        {
            pieceObject.AddComponent(componentType);

            Piece pieceScript = pieceObject.GetComponent<Piece>();

            pieceScript.pieceType = pieceType;

            pieceScript.Team = (((int)pieceType) < 7) ? Team.White : Team.Black;

            return pieceScript;
        }

        Debug.LogError($"Invalid piece type: {pieceType}");

        return null;
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

    public void RemovePiece()
    {
        _occupyingPiece = null;

        Transform pieceTransform = transform.Find("Piece");

        if (pieceTransform == null) return;

        Destroy(pieceTransform.gameObject);
    }

    public void ChangeTileColor(Color32 color)
    {
        gameObject.GetComponent<Image>().color = color;
    }
}
