using System;
using UnityEngine;
using UnityEngine.UI;

public enum PieceType
{
    None, // Empty square
    WhitePawn,
    WhiteKnight,
    WhiteBishop,
    WhiteRook,
    WhiteQueen,
    WhiteKing,
    BlackPawn,
    BlackKnight,
    BlackBishop,
    BlackRook,
    BlackQueen,
    BlackKing
}

public class Board : MonoBehaviour
{
    [Header("TileColors")]
    public Color32 darkSquareColor = new Color32(171, 122, 101, 255);
    public Color32 lightSquareColor = new Color32(238, 216, 192, 255);

    [Header("BoardSize")]
    public int boardSize = 8;

    [Header("Prefabs")]
    public GameObject _tilePrefab;

    private Tile[,] _tiles;
    private const int _xOffset = 25;
    private const int _yOffset = 60;

    private static readonly PieceType[,] _initialBoardPosition = new PieceType[8, 8]
    {
        { PieceType.BlackRook, PieceType.BlackKnight, PieceType.BlackBishop, PieceType.BlackQueen, PieceType.BlackKing, PieceType.BlackBishop, PieceType.BlackKnight, PieceType.BlackRook },
        { PieceType.BlackPawn, PieceType.BlackPawn, PieceType.BlackPawn, PieceType.BlackPawn, PieceType.BlackPawn, PieceType.BlackPawn, PieceType.BlackPawn, PieceType.BlackPawn },
        { PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None },
        { PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None },
        { PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None },
        { PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None },
        { PieceType.WhitePawn, PieceType.WhitePawn, PieceType.WhitePawn, PieceType.WhitePawn, PieceType.WhitePawn, PieceType.WhitePawn, PieceType.WhitePawn, PieceType.WhitePawn },
        { PieceType.WhiteRook, PieceType.WhiteKnight, PieceType.WhiteBishop, PieceType.WhiteQueen, PieceType.WhiteKing, PieceType.WhiteBishop, PieceType.WhiteKnight, PieceType.WhiteRook }
    };

    void Start()
    {
        _tiles = new Tile[boardSize, boardSize];

        InitializeBoard();
    }

    public void InitializeBoard()
    {
        for (int rank = 0; rank < boardSize; rank++)
        {
            for (int file = 0; file < boardSize; file++)
            {
                // Get the position in which the tile should be placed
                RectTransform tilePrefabRectTransform = _tilePrefab.GetComponent<RectTransform>();
                float tileWidth = tilePrefabRectTransform.sizeDelta.x;
                float tileHeight = tilePrefabRectTransform.sizeDelta.y;
                Vector3 tilePosition = new Vector3(_xOffset + (tileWidth * rank), _yOffset + (tileHeight * file), 0);

                // Initialise the tile 
                GameObject tile = Instantiate(_tilePrefab, tilePosition, Quaternion.identity);
                tile.transform.SetParent(transform);
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

                GameObject tilePiece = tile.transform.Find("Piece").gameObject;

                switch (_initialBoardPosition[rank, file])
                {
                    case PieceType.WhitePawn:
                        tilePiece.GetComponent<Image>().sprite = LoadPieceSprite(GetSpriteName(PieceType.WhitePawn));
                        tilePiece.AddComponent<Pawn>();
                        break;
                }

                _tiles[file, rank] = tile.GetComponent<Tile>();
            }
        }
    }

    public void PlacePiece(int index, Piece piece)
    {
        throw new NotImplementedException();
    }

    public Tile[,] GetTiles
    { 
        get { return  _tiles; }
    }

    public Tile GetTile(int row, int column)
    {
        return _tiles[row, column];
    }

    private Sprite LoadPieceSprite(string pieceName)
    {
        return Resources.Load<Sprite>($"Graphics/Sprites/Pieces/Texture/{pieceName}");
    }

    private string GetSpriteName(PieceType pieceType)
    {
        switch (pieceType)
        {
            case PieceType.WhitePawn: return "PawnW";
            case PieceType.WhiteKnight: return "KnightW";
            case PieceType.WhiteBishop: return "BishopW";
            case PieceType.WhiteRook: return "RookW";
            case PieceType.WhiteQueen: return "QueenW";
            case PieceType.WhiteKing: return "KingW";
            case PieceType.BlackPawn: return "PawnB";
            case PieceType.BlackKnight: return "KnightB";
            case PieceType.BlackBishop: return "BishopB";
            case PieceType.BlackRook: return "RookB";
            case PieceType.BlackQueen: return "QueenB";
            case PieceType.BlackKing: return "KingB";
            default: return null;
        }
    }
}
