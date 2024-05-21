using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Color defaultColor;

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

    public Piece GetOccupyingPiece
    {
        get { return _occupyingPiece; }
    }

    public Piece PlacePiece(PieceType pieceToPlace)
    {
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

            pieceScript.team = (((int)pieceType) < 7) ? Team.White : Team.Black;

            return pieceScript;
        }

        Debug.LogError($"Invalid piece type: {pieceType}");

        return null;
    }

    public void RemovePiece()
    {
        _occupyingPiece = null;

        GameObject pieceObject = transform.Find("Piece").gameObject;
        Piece pieceScript = pieceObject.GetComponent<Piece>();
        Image pieceImage = pieceObject.GetComponent<Image>();

        pieceImage.sprite = null;
        Destroy(pieceScript);
        pieceObject.SetActive(false);
    }

    public void ChangeTileColor(Color32 color)
    {
        gameObject.GetComponent<Image>().color = color;
    }
}
