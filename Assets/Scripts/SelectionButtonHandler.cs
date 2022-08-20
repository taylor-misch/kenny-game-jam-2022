using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionButtonHandler : MonoBehaviour
{
    Button button;
    GameState gameState;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
        gameState = GameState.GetInstance();
    }


    private void ButtonClicked()
    {
        gameState.SelectionBox = gameObject;
        GameEvents.current.SelectableEngaged();
    }
    
}
