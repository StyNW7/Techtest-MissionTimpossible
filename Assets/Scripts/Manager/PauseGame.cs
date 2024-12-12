using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{

    public bool paused = false;
    public GameObject pauseMenu;

    // Escape Button to pasue the game
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;

    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void quit()
    {
        Debug.Log("Quit Game");
        Time.timeScale = 1f;
        // Automate quit the Unity
        UnityEditor.EditorApplication.isPlaying = false;
    }

}