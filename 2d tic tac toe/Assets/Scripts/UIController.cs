using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject StartMenu;
    public GameObject OptionsMenu;
    public GameObject Game;
    public GameObject GameOverMenu;
    // Start is called before the first frame update
    void Start()
    {
        ShowInitialMenu();
    }

    public void ShowInitialMenu()
    {
        StartMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        Game.SetActive(false);
        GameOverMenu.SetActive(false);
    }

    public void StartGame()
    {
        StartMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        Game.SetActive(true);
        GameOverMenu.SetActive(false);
    }

    public void GameOver(int player)
    {
        StartMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        Game.SetActive(false);
        GameOverMenu.SetActive(true);
        
        // Displaying which player number wins!
        GameOverMenu.GetComponentInChildren<TextMeshProUGUI>().text = string.Format("Congratulations!\nPlayer {0} Wins!", player+1);
    }

    public void ShowOptionsMenu()
    {
        StartMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        Game.SetActive(false);
        GameOverMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
