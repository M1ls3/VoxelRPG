/*
 *  Author: ariel oliveira [o.arielg@gmail.com]
 */

using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    #region Sigleton
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerStats>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("PlayerStats");
                    instance = obj.AddComponent<PlayerStats>();
                }
            }
            return instance;
        }
    }
    #endregion

    [Header("Player Reference")]
    [SerializeField] private GameObject playerPrefab; // Добавлено поле для префаба
    private Health playerHealth; // Ссылка на компонент здоровья игрока

    private float health;
    private float maxHealth;
    private float maxTotalHealth;

    public float Health { get { return health; } }
    public float MaxHealth { get { return maxHealth; } }
    public float MaxTotalHealth { get { return maxTotalHealth; } }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Инициализация ссылки на здоровье игрока
        if (playerPrefab != null)
        {
            playerHealth = playerPrefab.GetComponent<Health>();
            if (playerHealth != null)
            {
                maxHealth = playerHealth.MaxHealth;
                health = maxHealth;
            }
        }
    }

    public void Initialize(float initialHealth, float initialMaxHealth)
    {
        health = initialHealth;
        maxHealth = initialMaxHealth;
        ClampHealth();
    }

    // Обновленный метод инициализации
    public void InitializeFromPrefab()
    {
        if (playerHealth == null) return;

        health = playerHealth.CurrentHealth;
        maxHealth = playerHealth.MaxHealth;
        ClampHealth();
    }

    private void Update()
    {
        ClampHealth();
    }

    public void Heal(float health)
    {
        this.health += health;
        ClampHealth();
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        ClampHealth();
    }

    public void AddHealth()
    {
        if (maxHealth < maxTotalHealth)
        {
            maxHealth += 1;
            health = maxHealth;

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }   
    }

    void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }
}
