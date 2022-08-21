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

    public bool CheckIfRecipeCanBeBuilt()
    {
        Dictionary<string, int> buildingMaterialCountDictionary = new Dictionary<string, int>();

        // build up the supply count
        foreach (BuildMaterial buildMaterial in gameState.MaterialsNeeded)
        {
            if (buildingMaterialCountDictionary.ContainsKey(buildMaterial.MaterialName))
            {
                buildingMaterialCountDictionary[buildMaterial.MaterialName] += 1;
            }
            else
            {
                buildingMaterialCountDictionary[buildMaterial.MaterialName] = 1;
            }
        }

        // remove one for each common element (if an element reaches 0, take it out of dictionary)
        foreach (KeyValuePair<string, Vector3Int> kvp in gameState.MaterialsSelected)
        {
            Debug.Log("Key = " + kvp.Key + " Value = " + kvp.Value);
            BoardHex boardHex = gameState.GetBoardHexAtPosition(kvp.Value);
            if (buildingMaterialCountDictionary.ContainsKey(boardHex.BuildMaterial.MaterialName))
            {
                buildingMaterialCountDictionary[boardHex.BuildMaterial.MaterialName] -= 1;
                if (buildingMaterialCountDictionary[boardHex.BuildMaterial.MaterialName] == 0)
                {
                    buildingMaterialCountDictionary.Remove(boardHex.BuildMaterial.MaterialName);
                }
            }
            else
            {
                return false;
            }
        }

        foreach (KeyValuePair<string, int> kvp in buildingMaterialCountDictionary)
            Debug.Log("Key = " + kvp.Key + " Value = " + kvp.Value);
        
        // if the dictionary is empty and the materials list had no extras you can build
        return buildingMaterialCountDictionary.Count == 0;
        
    }

    // Given a selection in the menu

    // Highlight all board pieces that can be used to create the object

    // Player selects the resources needed from the options

    // Game validates selection meet build requirements on each click and/or updates the resources to choose from

    // Highlights the potential placements for the building

    // Player selects place to play building 

    // Confirms and completes
}