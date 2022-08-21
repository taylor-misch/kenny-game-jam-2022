using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameState gameState;
    GameBoard gameBoard;
    BuildSystem buildSystem;
    [SerializeField] GameObject buildRecipeOverlay;
    
    // Materials needed section
    [SerializeField] GameObject materialsNeededItems;
    [SerializeField] GameObject materialsSelectedItems;
    
    // Build Menu 
    [SerializeField] GameObject buildMenuItemContainer;
    [SerializeField] GameObject buildMenuItemPrefab;
    [SerializeField] GameObject menuItemPrefab;

    [SerializeField] GameObject selectableBoxPrefab;

    [SerializeField] GameObject turnCounter;
    
    void Start()
    {
        gameState = GameState.GetInstance();
        gameBoard = GameBoard.GetInstance();
        gameBoard.SetupGame();
        gameState.Turn = 1;
        turnCounter.GetComponent<Text>().text = "Turn: " + gameState.Turn;
        gameState.IsBuildLocationSet = false;
        
        buildRecipeOverlay.SetActive(false);
        
        buildSystem = BuildSystem.GetInstance();
        GameEvents.current.onRecipeSelected += DisplayBuildRecipePanel;
        GameEvents.current.onRecipeUnselected += HideBuildRecipePanel;
        GameEvents.current.onRecipeUnselected += DestroyChildren;
        PopulateBuildMenu();

        GameEvents.current.onSelectableEnaged += PrepareForSelection;
        GameEvents.current.onBuildSelectableEngaged += PrepareForBuildLocationSelection;
    }


    public void CancelBuild()
    {
        gameState.ClearBuildRecipeOverlayData();
        Debug.Log("Cancel Build");
    }


    public void BuildRecipe()
    {
        if (gameState.IsBuildLocationSet)
        {
            Vector3Int buildPosition = gameState.BuildLocation;
            gameState.DefaultMap.SetTile(buildPosition, gameState.CurrentRecipe.TileBase);
            gameState.CoordinateIndexDictionary[buildPosition] =
                gameState.CurrentRecipe.BuildMaterialIndex;
            
            // update turn
            gameState.Turn = gameState.Turn + 1;
            turnCounter.GetComponent<Text>().text = "Turn: " + gameState.Turn;
            
            // refresh board hex list
            BoardHex boardHex = gameState.GetBoardHexAtPosition(buildPosition);
            boardHex.BuildMaterial = gameState.CurrentRecipe;
            
            // refresh the tile count
            gameState.UpdateBoardTileCounts();
        }
        CancelBuild();
        Debug.Log("Build It!");
    }

    private void DisplayBuildRecipePanel(int recipeIndex)
    {
        gameState.CurrentRecipe = gameState.BuildMaterialList[recipeIndex];
        gameState.MaterialsNeeded = new List<BuildMaterial>(buildSystem.GetRecipe(recipeIndex));
        buildRecipeOverlay.SetActive(true);
        int index = 0;
        foreach (BuildMaterial material in gameState.MaterialsNeeded)
        {
            InstantiateChildMenuItem(material.Sprite, materialsNeededItems);
            InstantiateSelectableItems("selectable" + index, materialsSelectedItems);
            index++;
        }
    }

    private void HideBuildRecipePanel()
    {
        buildRecipeOverlay.SetActive(false);
    }

    private void InstantiateChildMenuItem(Sprite sprite, GameObject parent)
    {
        GameObject item = Instantiate(menuItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        item.GetComponent<Image>().sprite = sprite;
        item.transform.SetParent(parent.transform);
        
    }
    
    private void InstantiateSelectableItems(string selectableName,GameObject parent)
    {
        GameObject item = Instantiate(selectableBoxPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        item.name = selectableName;
        item.transform.SetParent(parent.transform);
        item.transform.localScale = new Vector3(1, 1, 1);
        
    }

    private void DestroyChildren()
    {
        foreach (Transform child in materialsNeededItems.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in materialsSelectedItems.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void PopulateBuildMenu()
    {
        foreach (BuildMaterial buildMaterial in gameState.BuildMaterialList)
        {
            GameObject buildItem = Instantiate(buildMenuItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            buildItem.GetComponent<Image>().sprite = buildMaterial.Sprite;
            buildItem.transform.SetParent(buildMenuItemContainer.transform);
            buildItem.transform.localScale = new Vector3(1, 1, 1);
            buildItem.GetComponent<BuildingButtonHandler>().BuildMaterial = buildMaterial;
        }
    }

    private void PrepareForSelection()
    {
        gameState.IsSelectableEngaged = true;
    }

    private void PrepareForBuildLocationSelection()
    {
        gameState.IsBuildSelectableEngaged = true;
    }
}