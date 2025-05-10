using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUIScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject deathPanel;

    private void Update()
    {
        if (PlayerStats.Instance.Health <= 0) ShowDeathScreen(deathPanel);
    }

    public void ShowDeathScreen(GameObject killer)
    {
        // Пауза игры
        Time.timeScale = 0f;

        Debug.Log($"Killer {killer}");

        // Активация панели смерти
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
    }
}
