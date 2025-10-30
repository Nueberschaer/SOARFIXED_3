using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class PauseMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static bool Paused = false;
    public GameObject PauseMenuCanvas;
    
    void Awake()
    {
        Time.timeScale = 1f;
        PauseMenuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
    }

    void Stop()
    {
        PauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        Paused = true;
    }

    public void Play()
    {
        Debug.Log("PLAYSTART");
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Time.TimeScale =" + Time.timeScale);
        Paused = false;
        Debug.Log("PLAYEND");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
