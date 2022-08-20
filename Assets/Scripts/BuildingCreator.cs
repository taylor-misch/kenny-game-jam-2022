using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class BuildingCreator : Singleton<BuildingCreator>
{
    GameState gameState;
    BuildMaterial selectedMaterial;
    GameBoard gameBoard;
    TileBase tileBase;
    PlayerInput playerInput;

    protected override void Awake()
    {
        base.Awake();

        gameState = GameState.GetInstance();
        gameBoard = GameBoard.GetInstance();

        playerInput = new PlayerInput();
    }

    void OnEnable()
    {
        playerInput.Enable();
        playerInput.Gameplay.MouseLeftClick.performed += OnLeftClick;
        playerInput.Gameplay.MouseRightClick.performed += OnRightClick;
    }
    
    void OnDisable()
    {
        playerInput.Disable();
        playerInput.Gameplay.MouseLeftClick.performed -= OnLeftClick;
        playerInput.Gameplay.MouseRightClick.performed -= OnRightClick;
    }

    BuildMaterial SelectedMaterial
    {
        set
        {
            selectedMaterial = value;
            tileBase = selectedMaterial != null ? selectedMaterial.TileBase : null;
            UpdatePreview();
        }
    }

    private void Update()
    {
        // if (selectedMaterial != null)
        // {
            // Vector3 pos = _camera.ScreenToWorldPoint(mousePos);
            // pos.z = 0f;
            // Vector3Int gridPosition = gameState.PreviewMap.WorldToCell(pos);

            // if (gameState.GridPosition != gameState.CurrentGridPosition)
            // {
                // lastGridPosition = currentGridPosition;
                // currentGridPosition = gridPosition;

                // UpdatePreview();
            // }
        // }
    }
    
    void OnLeftClick(InputAction.CallbackContext ctx)
    {
        // if (selectedMaterial != null && !EventSystem.current.IsPointerOverGameObject())
        // if (selectedMaterial != null)
        // {
        //     // Debug.Log("Current Grid Position: " + currentGridPosition);
        //     if (gameBoard.CanDrawHex(gameState.CurrentGridPosition))
        //     {
        //         HandleDrawing();
        //     }
        //     else
        //     {
        //         Debug.Log("You can't draw here");
        //     }
        // }
    }

    void OnRightClick(InputAction.CallbackContext ctx)
    {
        SelectedMaterial = null;
        gameState.ClearBuildRecipeOverlayData();
    }

    
    public void MaterialSelected(BuildMaterial buildMaterial)
    {
        SelectedMaterial = buildMaterial;
    }

    void UpdatePreview()
    {
        if (gameBoard.CanDrawHex(gameState.CurrentGridPosition))
        {
            gameState.PreviewMap.SetTile(gameState.LastGridPosition, null);
            gameState.PreviewMap.SetTile(gameState.CurrentGridPosition, tileBase);
        }
        else
        {
            gameState.PreviewMap.SetTile(gameState.CurrentGridPosition, tileBase);
            gameState.PreviewMap.SetTileFlags(gameState.CurrentGridPosition, TileFlags.None);
            gameState.PreviewMap.SetColor(gameState.CurrentGridPosition, new Color(.64f, .05f, .05f, .75f));
            gameState.PreviewMap.SetTile(gameState.LastGridPosition, null);
        }
    }

    void HandleDrawing()
    {
        DrawItem();
    }

    void DrawItem()
    {
        gameState.DefaultMap.SetTile(gameState.CurrentGridPosition, tileBase);
    }
}