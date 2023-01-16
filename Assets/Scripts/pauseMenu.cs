using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(gamePaused) resume();
            else pause();
        }
    }
    
    public void resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void quitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        Debug.Log("Quit");
    }
}
