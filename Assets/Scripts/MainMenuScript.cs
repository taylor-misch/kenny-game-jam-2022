using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadInstructions()
    {
        SceneManager.LoadScene("InstructionsScene");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void LoadWinScreen()
    {
        
    }
}
