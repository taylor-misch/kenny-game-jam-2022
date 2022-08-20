using UnityEngine;
using UnityEngine.InputSystem;

public class GameControls : Singleton<GameControls>
{
    GameState gameState;
    PlayerInput playerInput;
    Camera _camera;

    protected override void Awake()
    {
        base.Awake();

        gameState = GameState.GetInstance();

        playerInput = new PlayerInput();
        _camera = Camera.main;
    }
    
    void Update()
    {
        Vector3 position = _camera.ScreenToWorldPoint(new Vector3(gameState.MousePosition.x, gameState.MousePosition.y, 0));
        Vector3Int gridPosition = gameState.PreviewMap.WorldToCell(position);
        gridPosition.z = 0;
        
        if (gridPosition != gameState.CurrentGridPosition)
        {
            gameState.LastGridPosition = gameState.CurrentGridPosition;
            gameState.CurrentGridPosition = gridPosition;
        }
        // Debug.Log("Current grid position: " + gameState.CurrentGridPosition);
    }
    
    void OnMouseMove(InputAction.CallbackContext ctx)
    {
        gameState.MousePosition = ctx.ReadValue<Vector2>();
    }
    
    void OnEnable()
    {
        playerInput.Enable();
        playerInput.Gameplay.MousePosition.performed += OnMouseMove;
    }

    void OnDisable()
    {
        playerInput.Disable();
        playerInput.Gameplay.MousePosition.performed -= OnMouseMove;
    }
}