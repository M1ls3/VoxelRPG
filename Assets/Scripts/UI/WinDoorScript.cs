using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDoorScript : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;

    private void Awake()
    {
        WinTrigger.OnPlayerWin.AddListener(ShowWinScreen);
        winPanel.SetActive(false);
    }

    private void ShowWinScreen()
    {
        Time.timeScale = 0f; // Остановка времени
        winPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        winPanel.SetActive(false);
        // Дополнительно: перезагрузка сцены 
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        WinTrigger.OnPlayerWin.RemoveListener(ShowWinScreen);
    }
}

