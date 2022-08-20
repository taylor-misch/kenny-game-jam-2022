using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameBoard : Singleton<GameBoard>
{
    GameState gameState;
    PlayerInput playerInput;

    [SerializeField] SelectObjectBase greenCheck;
    [SerializeField] SelectObjectBase redX;
    [SerializeField] SelectObjectBase blueTarget;

    protected override void Awake()
    {
        base.Awake();
        gameState = GameState.GetInstance();
        playerInput = new PlayerInput();

        GameEvents.current.onSelectableEnaged += DisplayTargets;
        GameEvents.current.onSelectableDisenaged += HideTargets;
    }

    public void SetupGame()
    {
        RenderGameBoard(gameState.GetStartingBoardPositions());
        gameState.Turn = 0;
    }


    void RenderGameBoard(List<Vector3Int> tilePositions)
    {
        // int index = 0;

        foreach (Vector3Int position in tilePositions)
        {
            BuildMaterial buildMaterial = gameState.StarterBuildMaterials[Random.Range(0, 3)];
            BoardHex boardHex = new BoardHex(position, buildMaterial);
            gameState.BoardHexList.Add(boardHex);
            gameState.DefaultMap.SetTile(position, buildMaterial.TileBase);
            gameState.CoordinateIndexDictionary.Add(position, buildMaterial.BuildMaterialIndex);
            // index++;
        }
    }

    public bool CanDrawHex(Vector3Int pos)
    {
        return gameState.CoordinateIndexDictionary.ContainsKey(pos);
    }

    public void PrintOptions()
    {
        foreach (BoardHex boardHex in gameState.BoardHexList)
        {
            // Debug.Log("Board Hex: " + boardHex);
            if (boardHex.BuildMaterial.MaterialName.Equals("Grass"))
            {
                gameState.SelectMap.SetTile(boardHex.Position, greenCheck.TileBase);
            }
            else
            {
                gameState.SelectMap.SetTile(boardHex.Position, redX.TileBase);
            }
        }
    }

    public void UpdateBoardTileCounts()
    {
        gameState.TileCountDictionary.Clear();
        foreach (BoardHex boardHex in gameState.BoardHexList)
        {
            if (gameState.TileCountDictionary.ContainsKey(boardHex.BuildMaterial.MaterialName))
            {
                int count = gameState.TileCountDictionary[boardHex.BuildMaterial.MaterialName];
                count++;
                gameState.TileCountDictionary[boardHex.BuildMaterial.MaterialName] = count;
            }
            else
            {
                gameState.TileCountDictionary.Add(boardHex.BuildMaterial.MaterialName, 1);
            }
        }

        foreach (KeyValuePair<string, int> kvp in gameState.TileCountDictionary)
            Debug.Log("Key = " + kvp.Key + " Value = " + kvp.Value);
    }

    public void DisplayTargets()
    {
        foreach (BoardHex boardHex in gameState.BoardHexList)
        {
            gameState.SelectMap.SetTile(boardHex.Position, blueTarget.TileBase);
        }
    }

    public void HideTargets()
    {
        foreach (BoardHex boardHex in gameState.BoardHexList)
        {
            gameState.SelectMap.SetTile(boardHex.Position, null);
        }
    }

    void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (gameState.IsSelectableEngaged)
        {
            // assign selected material
            Vector3Int currentPosition = gameState.CurrentGridPosition;
            if (gameState.GetStartingBoardPositions().Contains(currentPosition))
            {
                int itemIndex = gameState.CoordinateIndexDictionary[currentPosition];
                Debug.Log("Index of item: " + itemIndex);
                gameState.SelectedBuildMaterial = gameState.BuildMaterialList[itemIndex];
                gameState.SelectionBox.GetComponent<Image>().sprite = gameState.SelectedBuildMaterial.Sprite;
                gameState.MaterialsSelected[gameState.SelectionBox.name] = currentPosition;
            }

            GameEvents.current.SelectableDisengaged();
        }
    }

    void OnEnable()
    {
        playerInput.Enable();
        playerInput.Gameplay.MouseLeftClick.performed += OnLeftClick;
    }
    
    void OnDisable()
    {
        playerInput.Disable();
        playerInput.Gameplay.MouseLeftClick.performed -= OnLeftClick;
    }
}