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
    private Camera _camera;
    private Tile _selectedPiece;
    private InputState _currentState;

    private void Start()
    {
        _camera = Camera.main;
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

            Debug.Log(worldPosition);

            // Create a raycast to detect the tile game object
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject tileGameObject = hit.collider.gameObject;
                tileGameObject.GetComponent<Image>().color = new Color32(155, 0, 0, 255);
            }
        }
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
        
    }
}