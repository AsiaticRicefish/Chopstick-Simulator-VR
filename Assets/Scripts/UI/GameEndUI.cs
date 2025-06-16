using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndUI : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
