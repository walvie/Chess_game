using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum InputState
{
    None,
    PieceSelected
}

public class InputManager : MonoBehaviour
{
    public Color32 selectedTileColor = new Color32(90, 200, 90, 255);
    public Color32 possibleTilesToMoveColor = new Color32(90, 90, 90, 255);

    [SerializeField]
    private GameObject _boardGameObject;

    private Board _boardScript;
    private Camera _camera;
    private Tile _selectedTile;
    private InputState _currentState;
    private List<Tile> _possibleTilesToMove;

    private void Awake()
    {
        _camera = Camera.main;
        _boardScript = _boardGameObject.GetComponent<Board>();
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
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        Vector2 worldPosition = mousePosition;

        // Create a raycast to detect the clicked tile game object
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider == null) return;

        GameObject tileGameObject = hit.collider.gameObject;
        Tile tileScript = tileGameObject.GetComponent<Tile>();
        Piece tilePiece = tileScript.OccupyingPiece;

        if (tilePiece == null) return;

        _selectedTile = tileScript;
        tileScript.ChangeTileColor(selectedTileColor);

        _possibleTilesToMove = GeneratePieceMoves(tilePiece);

        _currentState = InputState.PieceSelected;

    }

    private void HandlePointAndClickMovement(Vector2 mousePosition)
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        Vector2 worldPosition = mousePosition;

        // Create a raycast to detect the tile game object
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider == null) return;

        GameObject tileGameObject = hit.collider.gameObject;
        Tile tileScript = tileGameObject.GetComponent<Tile>();
        Piece tilePiece = tileScript.OccupyingPiece;
        Piece selectedTilePiece = _selectedTile.OccupyingPiece;

        if (tilePiece == null)
        {
            MovePiece(_selectedTile, tileScript);
        }
        else if (tilePiece.Team != selectedTilePiece.Team) 
        {
            MovePiece(_selectedTile, tileScript);
        }
        else
        {
            CancelPieceSelection();
            _selectedTile = tileScript;
            _selectedTile.ChangeTileColor(selectedTileColor);

            _possibleTilesToMove = GeneratePieceMoves(tilePiece);

            _currentState = InputState.PieceSelected;
        }
    }

    private void MovePiece(Tile departureTile, Tile destinationTile) 
    {
        List<Tile> listOfDestinationTiles = departureTile.OccupyingPiece.GetValidTilesToMoves;

        bool destinationTileIsValid = false;

        foreach(Tile tile in listOfDestinationTiles)
        {
            if (tile == destinationTile)
            {
                destinationTileIsValid = true;
            }
        }

        if (destinationTileIsValid)
        {
            _boardScript.MovePiece(departureTile, destinationTile);
        }

        CancelPieceSelection();
    }

    private void CancelPieceSelection()
    {
        if (_selectedTile == null) return;

        GameObject tileGameObject = _selectedTile.gameObject;
        Tile tileScript = tileGameObject.GetComponent<Tile>();

        tileScript.ChangeTileColor(_selectedTile.DefaultColor);
        _selectedTile = null;

        if (_possibleTilesToMove.Count != 0)
        {
            foreach(Tile tile in _possibleTilesToMove)
            {
                tile.ChangeTileColor(tile.DefaultColor);
            }

            _possibleTilesToMove = new List<Tile>();
        }

        _currentState = InputState.None;
    }

    private List<Tile> GeneratePieceMoves(Piece selectedTilePiece)
    {
        List<Tile> possibleTiles = selectedTilePiece.GeneratePieceMoves();

        foreach (Tile tile in possibleTiles)
        {
            tile.ChangeTileColor(possibleTilesToMoveColor);
        }

        return possibleTiles;
    }
}