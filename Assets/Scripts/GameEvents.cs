using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    void Awake()
    {
        current = this;
    }

    public event Action<int> onRecipeSelected;
    public void RecipeSelected(int recipeIndex)
    {
        if (onRecipeSelected != null)
        {
            onRecipeSelected(recipeIndex);
        }
    }

    public event Action onRecipeUnselected;
    public void RecipeUnselected()
    {
        if (onRecipeUnselected != null)
        {
            onRecipeUnselected();
        }
    }

    public event Action onSelectableEnaged;

    public void SelectableEngaged()
    {
        if (onSelectableEnaged != null)
        {
            onSelectableEnaged();
        }
    }
    
    public event Action onSelectableDisenaged;

    public void SelectableDisengaged()
    {
        if (onSelectableDisenaged != null)
        {
            onSelectableDisenaged();
        }
    }

}