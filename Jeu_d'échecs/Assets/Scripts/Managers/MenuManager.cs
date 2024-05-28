using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject timerMenu;

    public void ChangeMenus()
    {
        if (mainMenu.activeSelf)
        {
            mainMenu.SetActive(false);
            timerMenu.SetActive(true);
        }
        else
        {
            timerMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void StartLongGame()
    {
        throw new NotImplementedException();
    }

    public void StartShortGame()
    {
        throw new NotImplementedException();
    }
}
