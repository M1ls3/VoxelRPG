using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    private bool _isPaused;

    public static bool IsPaused { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;

        Time.timeScale = _isPaused ? 0f : 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(_isPaused);
        }
    }

    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    public void ExitGame()
    {

        Application.Quit();
        //try
        //{
        //    Debug.Log("Button to next Scene!");
        //    SceneManager.LoadScene("Menu");
        //}
        //catch { Debug.Log("Exit Error"); }

    }
}

