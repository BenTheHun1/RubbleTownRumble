using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                PauseGame();
            }
            else
            {
                if (Time.timeScale == 0)
                {
                    ResumeGame();
                }
            }
            
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
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
