using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public enum PieceType
{
    None,
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

public enum InputState
{
    None,
    PieceSelected,
    DraggingPiece
}

public class Board : MonoBehaviour
{
    [Header("TileColors")]
    public Color32 darkSquareColor = new Color32(171, 122, 101, 255);
    public Color32 lightSquareColor = new Color32(238, 216, 192, 255);

    [Header("BoardSize")]
    public int boardSize = 8;

    [Header("Prefabs")]
    public GameObject tilePrefab;

    // Tiles
    private Tile[,] _tiles;
    private const int XOffset = 25;
    private const int YOffset = 60;

    private const string PiecesTexture = "Graphics/Sprites/Pieces/Texture/Pieces";

    private Camera _camera;
    private Tile _selectedPiece;
    private InputState _currentState;

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

    private Dictionary<PieceType, string> pieceSpriteNames = new Dictionary<PieceType, string>
    {
        { PieceType.WhitePawn, "PawnW" },
        { PieceType.BlackPawn, "PawnB" },
        { PieceType.WhiteKnight, "KnightW" },
        { PieceType.BlackKnight, "KnightB" },
        { PieceType.WhiteBishop, "BishopW" },
        { PieceType.BlackBishop, "BishopB" },
        { PieceType.WhiteRook, "RookW" },
        { PieceType.BlackRook, "RookB" },
        { PieceType.WhiteQueen, "QueenW" },
        { PieceType.BlackQueen, "QueenB" },
        { PieceType.WhiteKing, "KingW" },
        { PieceType.BlackKing, "KingB" }
    };

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

    private Dictionary<string, Sprite> _cachedPieceSprites;

    private void Start()
    {
        _tiles = new Tile[boardSize, boardSize];
        _cachedPieceSprites = LoadPieceSprites();

        InitializeBoard();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        Mouse mouse = Mouse.current;
        Vector2 mousePosition = _camera.ScreenToWorldPoint(mouse.position.ReadValue());

        if (_currentState == InputState.None)
        {
            HandlePieceSelection(mousePosition);
        }
        else if (_currentState == InputState.DraggingPiece)
        {
            HandleDragMovement(mousePosition);
        }
        else if (_currentState == InputState.PieceSelected)
        {
            HandlePointAndClickMovement(mousePosition);
        }

        if (mouse.rightButton.wasPressedThisFrame)
        {
            CancelPieceSelection();
        }
    }

    private void HandlePieceSelection(Vector2 mousePosition)
    {
        throw new NotImplementedException();
    }

    private void HandleDragMovement(Vector2 mousePosition)
    {
        throw new NotImplementedException();
    }

    private void HandlePointAndClickMovement(Vector2 mousePosition)
    {
        throw new NotImplementedException();
    }

    private void CancelPieceSelection()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Initialize the tiles of the board
    /// </summary>
    public void InitializeBoard()
    {
        // cache the size of the tile prefab
        RectTransform tilePrefabRectTransform = tilePrefab.GetComponent<RectTransform>();
        float tileWidth = tilePrefabRectTransform.sizeDelta.x;
        float tileHeight = tilePrefabRectTransform.sizeDelta.y;

        for (int rank = 0; rank < boardSize; rank++)
        {
            for (int file = 0; file < boardSize; file++)
            {
                // Get the position in which the tile should be placed
                Vector3 tilePosition = new Vector3(XOffset + (tileWidth * rank), YOffset + (tileHeight * file), 0);

                // Initialise the tile
                GameObject tileObject = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                tileObject.transform.SetParent(transform);
                tileObject.name = (char)('a' + rank) + (file + 1).ToString();

                // Change the color of the tile alternating from dark to light squares
                Image tileImage = tileObject.GetComponent<Image>();
                tileImage.color = ((file + rank) % 2 == 0) ? darkSquareColor : lightSquareColor;

                // Add tile to the tiles array
                Tile tileScript = tileObject.GetComponent<Tile>();
                _tiles[file, rank] = tileScript;

                GameObject tilePieceObject = tileObject.transform.Find("Piece").gameObject;

                InitializePiece(_initialBoardPosition[boardSize - 1 - file, rank], tilePieceObject);
            }
        }
    }

    /// <summary>
    /// Initialize the piece according to it's type
    /// </summary>
    /// <param name="pieceType">The type of the piece to initialize.</param>
    /// <param name="pieceObject">The GameObject representing the piece.</param>
    private void InitializePiece(PieceType pieceType, GameObject pieceObject)
    {
        if (pieceType == PieceType.None) return;

        if (!pieceSpriteNames.TryGetValue(pieceType, out string spriteName))
        {
            Debug.LogError($"No sprite name found for {pieceType}");
            return;
        }

        if (!_cachedPieceSprites.TryGetValue(spriteName, out Sprite sprite))
        {
            Debug.LogError($"No sprite found for {spriteName}");
            return;
        }

        Image pieceImage = pieceObject.GetComponent<Image>();
        pieceImage.sprite = sprite;

        AddPieceScript(pieceObject, pieceType);
        pieceObject.SetActive(true);
    }

    /// <summary>
    /// Adds the corresponding script of the piece to the gameObject
    /// </summary>
    /// <param name="pieceObject">The GameObject representing the piece.</param>
    /// <param name="pieceType">The type of the piece to initialize.</param>
    private void AddPieceScript(GameObject pieceObject, PieceType pieceType)
    {
        if (pieceComponentTypes.TryGetValue(pieceType, out Type componentType))
        {
            pieceObject.AddComponent(componentType);
        }
    }

    /// <summary>
    /// Loads all the sprites of the pieces in the dictionary
    /// </summary>
    private Dictionary<string, Sprite> LoadPieceSprites()
    {
        Sprite[] allPieceSprites = Resources.LoadAll<Sprite>(PiecesTexture);
        Dictionary<string, Sprite> spriteDictionary = new Dictionary<string, Sprite>();

        foreach (Sprite pieceSprite in allPieceSprites)
        {
            spriteDictionary[pieceSprite.name] = pieceSprite;
        }

        return spriteDictionary;
    }

    public void PlacePiece(int index, Piece piece)
    {
        throw new NotImplementedException();
    }

    public Tile[,] GetTiles
    { 
        get { return  _tiles; }
    }

    public Tile GetTile(int file, int rank)
    {
        return _tiles[file, rank];
    }
}
