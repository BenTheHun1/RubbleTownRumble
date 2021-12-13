using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;

    public bool press_esc = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Cancel") > 0)
        {
            if (!press_esc)
            {
                if (Time.timeScale == 1)
                {
                    PauseGame();
                }
                else if (Time.timeScale == 0)
                {
                    ResumeGame();
                }
                press_esc = true;
            }
        }
        if (Input.GetAxisRaw("Cancel") == 0)
        {
            press_esc = false;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Options()
    {
        optionsMenu.SetActive(!optionsMenu.activeSelf);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
