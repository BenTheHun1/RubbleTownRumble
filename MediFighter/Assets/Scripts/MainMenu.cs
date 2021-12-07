using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject credits;

    public void StartGame()
    {
        SceneManager.LoadScene("Main"); //Do the level one;
    }

    public void ShowCredits()
    {
        credits.SetActive(!credits.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
