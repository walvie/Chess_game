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
            _boardScript.MovePiece(_selectedTile, tileScript);
            CancelPieceSelection();
        }
        else if (tilePiece.Team != selectedTilePiece.Team) 
        {
            _boardScript.MovePiece(_selectedTile, tileScript);
            CancelPieceSelection();
        }
        else
        {
            CancelPieceSelection();
            _selectedTile = tileScript;
            _selectedTile.ChangeTileColor(selectedTileColor);
            _currentState = InputState.PieceSelected;
        }
    }

    private void CancelPieceSelection()
    {
        if (_selectedTile == null) return;

        GameObject tileGameObject = _selectedTile.gameObject;
        Tile tileScript = tileGameObject.GetComponent<Tile>();

        tileScript.ChangeTileColor(_selectedTile.DefaultColor);
        _selectedTile = null;
        _currentState = InputState.None;
    }
}