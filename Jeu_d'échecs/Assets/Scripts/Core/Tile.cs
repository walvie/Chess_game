using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Color32 _color;
    private Piece _occupyingPiece;

    public Piece GetOccupyingPiece 
    {  
        get { return _occupyingPiece; }
    }

    public Piece PlacePiece(Piece pieceToPlace)
    {
        throw new System.NotImplementedException();
    }

    public void RemovePiece()
    {
        throw new System.NotImplementedException();
    }

    public void ChangeTileColor(Color32 color)
    {
        throw new System.NotImplementedException();
    }
}
