using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameState : Singleton<GameState>
{
    // Tile Maps
    [SerializeField] Tilemap defaultMap;
    [SerializeField] Tilemap previewMap;
    [SerializeField] Tilemap selectMap;

    [SerializeField] Sprite blankBox;
    [SerializeField] GameObject selectableLocationBox;
    
    // Data Collections
    [SerializeField] List<BuildMaterial> buildMaterialList;
    [SerializeField] List<BuildMaterial> starterBuildMaterials;
    Dictionary<Vector3Int, int> coordinateIndexDictionary = new Dictionary<Vector3Int, int>();
    Dictionary<string, int> tileCountDictionary = new Dictionary<string, int>();
    List<BoardHex> boardHexList = new List<BoardHex>();

    //Player Input Data
    Vector2 mousePosition;
    Vector3Int gridPosition;
    Vector3Int currentGridPosition;
    Vector3Int lastGridPosition;
    
    // Game metadata
    int turn;
    
    // Recipe info
    BuildMaterial currentRecipe;

    //Materials needed
    List<BuildMaterial> materialsNeeded = new List<BuildMaterial>();
    
    // Materials selected
    Dictionary<string, Vector3Int> materialsSelected = new Dictionary<string, Vector3Int>();
    
    // Selectable functionality
    BuildMaterial selectedBuildMaterial;
    bool isSelectableEngaged;
    GameObject selectionBox;
    
    // Build Selectable
    GameObject buildSelectionBox;
    bool isBuildSelectableEngaged;
    Vector3Int buildLocation;
    bool isBuildLocationSet;
    
    
    // Menu items
    List<GameObject> menuItems = new List<GameObject>();
    
    // Selectable locations
    List<Vector3Int> selectableLocations;
    
    //Messages
    [SerializeField] GameObject messageBoard;
    private float timeToAppear = 2f;
    private float timeWhenDisappear;
    
    
    //Win Screen
    [SerializeField] GameObject winScreen;
    
    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (messageBoard.GetComponent<Text>().enabled && (Time.time >= timeWhenDisappear) )
        {
            messageBoard.GetComponent<Text>().text = "";
            messageBoard.GetComponent<Text>().enabled = false;
        }
    }
    public void SendMessageToMessageBoard(string message)
    {
        messageBoard.GetComponent<Text>().text = message;
        messageBoard.gameObject.GetComponent<Text>().enabled = true;
        timeWhenDisappear = Time.time + timeToAppear;
    }
    
    public void EvaluateIfRecipeCanBeBuild()
    {
        foreach (GameObject menuItem in menuItems)
        {
            BuildMaterial material = menuItem.GetComponent<BuildingButtonHandler>().BuildMaterial;
            bool result = CheckIfRecipeIsPossible(material.BuildRecipe);
            menuItem.GetComponent<Button>().interactable = result;
        }
    }

    public bool CheckIfRecipeIsPossible(List<BuildMaterial> materials)
    {
        Dictionary<string, int> materialDictionary = new Dictionary<string, int>();

        foreach (BuildMaterial buildMaterial in materials)
        {
            if (materialDictionary.ContainsKey(buildMaterial.MaterialName))
            {
                materialDictionary[buildMaterial.MaterialName] += 1;
            }
            else
            {
                materialDictionary[buildMaterial.MaterialName] = 1;
            }
        }
        
        // foreach (KeyValuePair<string, int> kvp in materialDictionary)
        //     Debug.Log("Key = " + kvp.Key + " Value = " + kvp.Value);
        //

        foreach (KeyValuePair<string, int> kvp in materialDictionary)
        {
            if (tileCountDictionary.ContainsKey(kvp.Key))
            {
                if (tileCountDictionary[kvp.Key] < kvp.Value)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // foreach (KeyValuePair<string, int> kvp in tileCountDictionary)
        //     Debug.Log("Key = " + kvp.Key + " Value = " + kvp.Value);
        
        return true;
    }

    public List<string> GetCurrentMaterialRecipeItemNames()
    {
        List<string> validTiles = new List<string>();
        foreach (BuildMaterial material in currentRecipe.BuildRecipe)
        {
            validTiles.Add(material.MaterialName);
        }

        return validTiles;
    }
    
    public void UpdateBoardTileCounts()
    {
        tileCountDictionary.Clear();
        foreach (BoardHex boardHex in boardHexList)
        {
            if (tileCountDictionary.ContainsKey(boardHex.BuildMaterial.MaterialName))
            {
                int count = tileCountDictionary[boardHex.BuildMaterial.MaterialName];
                count++;
                tileCountDictionary[boardHex.BuildMaterial.MaterialName] = count;
            }
            else
            {
                tileCountDictionary.Add(boardHex.BuildMaterial.MaterialName, 1);
            }
        }

        if (tileCountDictionary.ContainsKey("RocketShip"))
        {
            Debug.Log("You Win!");
            winScreen.transform.GetChild(1).GetComponent<Text>().text = "Turns Taken: " + turn;
            winScreen.SetActive(true);
        }
        
        // foreach (KeyValuePair<string, int> kvp in tileCountDictionary)
            // Debug.Log("Key = " + kvp.Key + " Value = " + kvp.Value);
    }
    
    public void ClearBuildRecipeOverlayData()
    {
        materialsNeeded.Clear();
        currentRecipe = null;
        GameEvents.current.RecipeUnselected();
        GameEvents.current.SelectableDisengaged();
        ClearBuildLocationData();
    }

    public void ClearBuildLocationData()
    {
        selectableLocationBox.GetComponent<Image>().sprite = blankBox;
        isBuildSelectableEngaged = false;
        buildLocation = Vector3Int.zero;
        isBuildLocationSet = false;
    }

    public BoardHex GetBoardHexAtPosition(Vector3Int position)
    {
        foreach (BoardHex boardHex in boardHexList)
        {
            if (boardHex.Position == position)
            {
                return boardHex;
            }
        }

        return null;
    }
    
    public List<Vector3Int> GetStartingBoardPositions()
    {
        List<Vector3Int> startingBoardPos = new List<Vector3Int>();
        startingBoardPos.Add(new Vector3Int(1, 2, 0));
        startingBoardPos.Add(new Vector3Int(2, 2, 0));
        startingBoardPos.Add(new Vector3Int(3, 2, 0));
        startingBoardPos.Add(new Vector3Int(0, 1, 0));
        startingBoardPos.Add(new Vector3Int(1, 1, 0));
        startingBoardPos.Add(new Vector3Int(2, 1, 0));
        startingBoardPos.Add(new Vector3Int(3, 1, 0));
        startingBoardPos.Add(new Vector3Int(0, 0, 0));
        startingBoardPos.Add(new Vector3Int(1, 0, 0));
        startingBoardPos.Add(new Vector3Int(2, 0, 0));
        startingBoardPos.Add(new Vector3Int(3, 0, 0));
        startingBoardPos.Add(new Vector3Int(4, 0, 0));
        startingBoardPos.Add(new Vector3Int(0, -1, 0));
        startingBoardPos.Add(new Vector3Int(1, -1, 0));
        startingBoardPos.Add(new Vector3Int(2, -1, 0));
        startingBoardPos.Add(new Vector3Int(3, -1, 0));
        startingBoardPos.Add(new Vector3Int(1, -2, 0));
        startingBoardPos.Add(new Vector3Int(2, -2, 0));
        startingBoardPos.Add(new Vector3Int(3, -2, 0));

        return startingBoardPos;
    }

    /// Getter/Setter below

    public Tilemap DefaultMap
    {
        get => defaultMap;
        set => defaultMap = value;
    }

    public Tilemap PreviewMap
    {
        get => previewMap;
        set => previewMap = value;
    }

    public Tilemap SelectMap
    {
        get => selectMap;
        set => selectMap = value;
    }

    public List<BuildMaterial> BuildMaterialList
    {
        get => buildMaterialList;
        set => buildMaterialList = value;
    }

    public List<BuildMaterial> StarterBuildMaterials
    {
        get => starterBuildMaterials;
        set => starterBuildMaterials = value;
    }

    public Dictionary<Vector3Int, int> CoordinateIndexDictionary
    {
        get => coordinateIndexDictionary;
        set => coordinateIndexDictionary = value;
    }

    public Dictionary<string, int> TileCountDictionary
    {
        get => tileCountDictionary;
        set => tileCountDictionary = value;
    }

    public List<BoardHex> BoardHexList
    {
    get => boardHexList;
    set => boardHexList = value;
    }

    public Vector2 MousePosition
    {
        get => mousePosition;
        set => mousePosition = value;
    }

    public Vector3Int GridPosition
    {
        get => gridPosition;
        set => gridPosition = value;
    }

    public Vector3Int CurrentGridPosition
    {
        get => currentGridPosition;
        set => currentGridPosition = value;
    }

    public Vector3Int LastGridPosition
    {
        get => lastGridPosition;
        set => lastGridPosition = value;
    }

    public int Turn
    {
        get => turn;
        set => turn = value;
    }

    public Dictionary<string, Vector3Int> MaterialsSelected
    {
        get => materialsSelected;
        set => materialsSelected = value;
    }

    public BuildMaterial SelectedBuildMaterial
    {
        get => selectedBuildMaterial;
        set => selectedBuildMaterial = value;
    }

    public bool IsSelectableEngaged
    {
        get => isSelectableEngaged;
        set => isSelectableEngaged = value;
    }

    public GameObject SelectionBox
    {
        get => selectionBox;
        set => selectionBox = value;
    }

    public List<BuildMaterial> MaterialsNeeded
    {
        get => materialsNeeded;
        set => materialsNeeded = value;
    }

    public BuildMaterial CurrentRecipe
    {
        get => currentRecipe;
        set => currentRecipe = value;
    }

    public GameObject BuildSelectionBox
    {
        get => buildSelectionBox;
        set => buildSelectionBox = value;
    }

    public bool IsBuildSelectableEngaged
    {
        get => isBuildSelectableEngaged;
        set => isBuildSelectableEngaged = value;
    }

    public Vector3Int BuildLocation
    {
        get => buildLocation;
        set => buildLocation = value;
    }

    public bool IsBuildLocationSet
    {
        get => isBuildLocationSet;
        set => isBuildLocationSet = value;
    }

    public List<GameObject> MenuItems
    {
        get => menuItems;
        set => menuItems = value;
    }

    public List<Vector3Int> SelectableLocations
    {
        get => selectableLocations;
        set => selectableLocations = value;
    }

    public GameObject MessageBoard
    {
        get => messageBoard;
        set => messageBoard = value;
    }
}