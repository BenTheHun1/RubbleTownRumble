using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject credits;

    private void Start()
    {
        credits.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Final");
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
