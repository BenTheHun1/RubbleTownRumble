using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject credits;
	public Text versionText;

    private void Start()
    {
        credits.SetActive(false);
		versionText.text = Application.version;
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
