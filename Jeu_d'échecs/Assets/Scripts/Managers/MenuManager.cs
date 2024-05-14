using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void StartLongGame()
    {

    }

    public void StartShortGame()
    {

    }
}
