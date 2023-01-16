using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void startEndlessHard()
    {
        SceneManager.LoadScene((int)Random.Range(6, 13));
    }

    public void startEndlessEasy()
    {
        SceneManager.LoadScene((int)Random.Range(14, 21));
    }

    public void select(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void levelSelect()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
