using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class Board : MonoBehaviour
{
    [Header("TileColors")]
    public Color32 darkSquareColor = new Color32(171, 122, 101, 255);
    public Color32 lightSquareColor = new Color32(238, 216, 192, 255);

    [Header("BoardSize")]
    public int boardSize = 8;

    [Header("Prefabs")]
    public GameObject _tilePrefab;

    private Tile[][] _tiles;
    private const int _xOffset = 25;
    private const int _yOffset = 60;

    void Start()
    {
        InitializeBoard();
    }

    void Update()
    {
        throw new NotImplementedException();
    }

    public void InitializeBoard()
    {
        for (int rank = 0; rank < boardSize; rank++)
        {
            for (int file = 0; file < boardSize; file++)
            {
                // Get the position in which the tile should be placed.
                RectTransform tilePrefabRectTransform = _tilePrefab.GetComponent<RectTransform>();
                float tileWidth = tilePrefabRectTransform.sizeDelta.x;
                float tileHeight = tilePrefabRectTransform.sizeDelta.y;
                Vector3 tilePosition = new Vector3(_xOffset + (tileWidth * rank), _yOffset + (tileHeight * file), 0);

                // Initialise the tile 
                GameObject tile = Instantiate(_tilePrefab, tilePosition, Quaternion.identity);
                tile.transform.parent = transform;
                tile.name = (char)('a' + rank) + (file + 1).ToString();

                // Change the color of the tile alternating from dark to light squares
                if ((rank + file) % 2 == 0)
                {
                    tile.GetComponent<Image>().color = darkSquareColor;
                }
                else
                {
                    tile.GetComponent<Image>().color = lightSquareColor;

                }
            }
        }
    }

    public void PlacePiece(int index, Piece piece)
    {
        throw new NotImplementedException();
    }

    public Tile[][] GetTiles
    { 
        get { return  _tiles; }
    }

    public Tile GetTile(int row, int column)
    {
        return _tiles[row][column];
    }
}
