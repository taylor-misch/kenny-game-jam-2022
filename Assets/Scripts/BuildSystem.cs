using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : Singleton<BuildSystem>
{
    GameState gameState;
    protected override void Awake()
    {
        base.Awake();
        gameState = GameState.GetInstance();
    }

    public List<BuildMaterial> GetRecipe(int recipeIndex)
    {
        return gameState.BuildMaterialList[recipeIndex].BuildRecipe;
    }

    public bool CheckIfRecipeCanBeBuild()
    {
        Vector3Int item1 = gameState.MaterialsSelected["selectable0"];
        Vector3Int item2 = gameState.MaterialsSelected["selectable1"];
        Vector3Int item3 = gameState.MaterialsSelected["selectable2"];

        if (gameState.MaterialsNeeded.Count != gameState.MaterialsSelected.Count)
        {
            return false;
        }
        
        // int index = 0;
        foreach (BuildMaterial buildMaterial in gameState.MaterialsNeeded)
        {
            
        }
        
        return true;
    }
    
    // Given a selection in the menu

    // Highlight all board pieces that can be used to create the object

    // Player selects the resources needed from the options

    // Game validates selection meet build requirements on each click and/or updates the resources to choose from

    // Highlights the potential placements for the building

    // Player selects place to play building 

    // Confirms and completes
}