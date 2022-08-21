using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameBoard : Singleton<GameBoard>
{
    GameState gameState;
    PlayerInput playerInput;
    BuildSystem buildSystem;

    [SerializeField] SelectObjectBase greenCheck;
    [SerializeField] SelectObjectBase redX;
    [SerializeField] SelectObjectBase blueTarget;
    [SerializeField] GameObject buildButton;

    protected override void Awake()
    {
        base.Awake();
        gameState = GameState.GetInstance();
        playerInput = new PlayerInput();
        buildSystem = BuildSystem.GetInstance();

        GameEvents.current.onSelectableEnaged += DisplayTargets;
        GameEvents.current.onSelectableDisenaged += HideTargets;
        GameEvents.current.onSelectableSelection += UpdateBuildButton;

        GameEvents.current.onBuildSelectableEngaged += DisplayBuildTargets;
        GameEvents.current.onBuildSelectableDisengaged += HideBuildTargets;
        GameEvents.current.onBuildSelectableSelection += UpdateBuildButton;
    }

    public void SetupGame()
    {
        RenderGameBoard(gameState.GetStartingBoardPositions());
    }


    void RenderGameBoard(List<Vector3Int> tilePositions)
    {
        int index = 0;

        foreach (Vector3Int position in tilePositions)
        {
            BuildMaterial buildMaterial = gameState.StarterBuildMaterials[Random.Range(0, 3)];
            BoardHex boardHex = new BoardHex(index, position, buildMaterial);
            gameState.BoardHexList.Add(boardHex);
            gameState.DefaultMap.SetTile(position, buildMaterial.TileBase);
            gameState.CoordinateIndexDictionary.Add(position, buildMaterial.BuildMaterialIndex);
            index++;
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

    // public void UpdateBoardTileCounts()
    // {
    //     gameState.TileCountDictionary.Clear();
    //     foreach (BoardHex boardHex in gameState.BoardHexList)
    //     {
    //         if (gameState.TileCountDictionary.ContainsKey(boardHex.BuildMaterial.MaterialName))
    //         {
    //             int count = gameState.TileCountDictionary[boardHex.BuildMaterial.MaterialName];
    //             count++;
    //             gameState.TileCountDictionary[boardHex.BuildMaterial.MaterialName] = count;
    //         }
    //         else
    //         {
    //             gameState.TileCountDictionary.Add(boardHex.BuildMaterial.MaterialName, 1);
    //         }
    //     }
    //
    //     foreach (KeyValuePair<string, int> kvp in gameState.TileCountDictionary)
    //         Debug.Log("Key = " + kvp.Key + " Value = " + kvp.Value);
    // }

    public void DisplayTargets()
    {
       
        foreach (BoardHex boardHex in gameState.BoardHexList)
        {
            if (gameState.GetCurrentMaterialRecipeItemNames().Contains(boardHex.BuildMaterial.MaterialName))
            {
                gameState.SelectableLocations.Add(boardHex.Position);
                gameState.SelectMap.SetTile(boardHex.Position, blueTarget.TileBase);
            }
        }
    }

    public void HideTargets()
    {
        gameState.IsSelectableEngaged = false;
        foreach (BoardHex boardHex in gameState.BoardHexList)
        {
            gameState.SelectMap.SetTile(boardHex.Position, null);
        }

        gameState.SelectableLocations = new List<Vector3Int>();
    }

    public void DisplayBuildTargets()
    {
        foreach (KeyValuePair<string, Vector3Int> kvp in gameState.MaterialsSelected)
        {
            gameState.SelectableLocations.Add(kvp.Value);
            gameState.SelectMap.SetTile(kvp.Value, blueTarget.TileBase);
        }
    }

    public void HideBuildTargets()
    {
        gameState.IsBuildSelectableEngaged = false;
        foreach (BoardHex boardHex in gameState.BoardHexList)
        {
            gameState.SelectMap.SetTile(boardHex.Position, null);
        }
        gameState.SelectableLocations = new List<Vector3Int>();
    }

    void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (gameState.IsSelectableEngaged)
        {
            // assign selected material
            Vector3Int currentPosition = gameState.CurrentGridPosition;
            if (gameState.SelectableLocations.Contains(currentPosition))
            {
                int itemIndex = gameState.CoordinateIndexDictionary[currentPosition];
                gameState.SelectedBuildMaterial = gameState.BuildMaterialList[itemIndex];
                gameState.SelectionBox.GetComponent<Image>().sprite = gameState.SelectedBuildMaterial.Sprite;
                gameState.MaterialsSelected[gameState.SelectionBox.name] = currentPosition;
                gameState.SelectableLocations = new List<Vector3Int>();

            }
            else
            {
                gameState.SendMessageToMessageBoard("Not A Valid Material");
            }

            GameEvents.current.SelectableSelection();
            GameEvents.current.SelectableDisengaged();
        }

        if (gameState.IsBuildSelectableEngaged)
        {
            // assign selected material
            Vector3Int currentPosition = gameState.CurrentGridPosition;
            if (gameState.SelectableLocations.Contains(currentPosition))
            {
                int itemIndex = gameState.CoordinateIndexDictionary[currentPosition];
                gameState.SelectedBuildMaterial = gameState.BuildMaterialList[itemIndex];
                gameState.BuildSelectionBox.GetComponent<Image>().sprite = gameState.SelectedBuildMaterial.Sprite;
                gameState.BuildLocation = currentPosition;
                gameState.IsBuildLocationSet = true;
                gameState.DowngradeLocations = new List<Vector3Int>(gameState.SelectableLocations);
                gameState.SelectableLocations = new List<Vector3Int>();

            }
            else
            {
                gameState.SendMessageToMessageBoard("You Can't Build There");
            }

            GameEvents.current.BuildSelectableSelection();
            GameEvents.current.BuildSelectableDisengaged();
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

    void UpdateBuildButton()
    {
        bool canBuild = buildSystem.CheckIfRecipeCanBeBuilt();
        if (canBuild)
        {
            buildButton.GetComponent<Button>().interactable = true;
            buildButton.transform.GetChild(0).GetComponent<Text>().text = "Build!";
        }
        else
        {
            buildButton.GetComponent<Button>().interactable = false;
            buildButton.transform.GetChild(0).GetComponent<Text>().text = "Can't Build";
        }
    }
}