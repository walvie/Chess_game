using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum InputState
{
    None,
    PieceSelected,
    DraggingPiece
}

public class InputManager : MonoBehaviour
{
    public Color32 selectedTileColor = new Color32(90, 200, 90, 255);

    [SerializeField]
    private GameObject _boardGameObject;

    private Board _boardScript;
    private Camera _camera;
    private Tile _selectedTile;
    private InputState _currentState;

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
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 worldPosition = mousePosition;

            // Create a raycast to detect the tile game object
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject tileGameObject = hit.collider.gameObject;
                Image tileImage = tileGameObject.GetComponent<Image>();
                Tile tileScript = tileGameObject.GetComponent<Tile>();
                Piece tilePiece = tileScript.GetOccupyingPiece;

                if (tilePiece != null)
                {
                    _selectedTile = tileScript;
                    tileImage.color = selectedTileColor;
                    _currentState = InputState.PieceSelected;
                }
            }
        }
    }

    private void HandleDragMovement(Vector2 mousePosition)
    {
        throw new NotImplementedException();
    }

    private void HandlePointAndClickMovement(Vector2 mousePosition)
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 worldPosition = mousePosition;

            // Create a raycast to detect the tile game object
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject tileGameObject = hit.collider.gameObject;
                Image tileImage = tileGameObject.GetComponent<Image>();
                Tile tileScript = tileGameObject.GetComponent<Tile>();
                Piece tilePiece = tileScript.GetOccupyingPiece;
                Piece selectedTilePiece = _selectedTile.GetOccupyingPiece;

                if (tilePiece == null)
                {
                    _boardScript.MovePiece(_selectedTile, tileScript);
                    CancelPieceSelection();
                }
                else if (tilePiece.team != selectedTilePiece.team) 
                {
                    _boardScript.MovePiece(_selectedTile, tileScript);
                    CancelPieceSelection();
                }
                else
                {
                    CancelPieceSelection();
                    _selectedTile = tileScript;
                    tileImage.color = selectedTileColor;
                    _currentState = InputState.PieceSelected;
                }
            }
        }
    }

    private void CancelPieceSelection()
    {
        if (_selectedTile == null) return;

        GameObject tileGameObject = _selectedTile.gameObject;
        Image tileImage = tileGameObject.GetComponent<Image>();

        tileImage.color = _selectedTile.defaultColor;
        _selectedTile = null;
        _currentState = InputState.None;
}
}