using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonHandler : MonoBehaviour
{
    //Keep
    BuildingCreator buildingCreator;
    GameBoard gameBoard;
    [SerializeField] BuildMaterial buildMaterial;
    Button button;
    GameState gameState;
    
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
        buildingCreator = BuildingCreator.GetInstance();
        gameBoard = GameBoard.GetInstance();
        gameState = GameState.GetInstance();
    }


    private void ButtonClicked()
    {
        buildingCreator.MaterialSelected(buildMaterial);
        gameBoard.PrintOptions();
        gameState.ClearBuildRecipeOverlayData();
        GameEvents.current.RecipeSelected(buildMaterial.BuildMaterialIndex);
    }

    
    // So I can set it on prefab instantiation
    public BuildMaterial BuildMaterial
    {
        get => buildMaterial;
        set => buildMaterial = value;
    }
}